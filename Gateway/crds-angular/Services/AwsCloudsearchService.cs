using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using Amazon.CloudSearchDomain;
using Amazon.CloudSearchDomain.Model;
using AutoMapper;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Models.Finder;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class AwsCloudsearchService : MinistryPlatformBaseService, IAwsCloudsearchService
    {
        private readonly IAddressGeocodingService _addressGeocodingService;
        private readonly IFinderRepository _finderRepository;
        private readonly IConfigurationWrapper _configurationWrapper;
        protected string AmazonSearchUrl;
        private const int ReturnRecordCount = 10000;

        public AwsCloudsearchService(IAddressGeocodingService addressGeocodingService, 
                                     IFinderRepository finderRepository,                          
                                     IConfigurationWrapper configurationWrapper)
        {
            _addressGeocodingService = addressGeocodingService;
            _finderRepository = finderRepository;
            _configurationWrapper = configurationWrapper;

            AmazonSearchUrl = _configurationWrapper.GetEnvironmentVarAsString("CRDS_AWS_CONNECT_ENDPOINT");
        }

        public UploadDocumentsResponse UploadAllConnectRecordsToAwsCloudsearch()
        {
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = AmazonSearchUrl
            };
            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var pinList = GetDataForCloudsearch();

            //serialize
            var json = JsonConvert.SerializeObject(pinList, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var upload = new Amazon.CloudSearchDomain.Model.UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                Documents = ms
            };

            return(cloudSearch.UploadDocuments(upload));
        }

        public UploadDocumentsResponse DeleteAllConnectRecordsInAwsCloudsearch()
        {
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = AmazonSearchUrl
            };
            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var results = SearchConnectAwsCloudsearch("matchall", "_no_fields");
            var deletelist = new List<AwsCloudsearchDto>();
            foreach (var hit in results.Hits.Hit)
            {
                var deleterec = new AwsCloudsearchDto
                {
                    id = hit.Id,
                    type = "delete"
                };
                deletelist.Add(deleterec);
            }
            // serialize
            var json = JsonConvert.SerializeObject(deletelist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var upload = new Amazon.CloudSearchDomain.Model.UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                Documents = ms
            };

            return (cloudSearch.UploadDocuments(upload));
        }




        private List<AwsCloudsearchDto> GetDataForCloudsearch()
        {
            var pins= _finderRepository.GetAllPinsForAws().Select(Mapper.Map<AwsConnectDto>).ToList();
            var pinlist = new List<AwsCloudsearchDto>();
            foreach (var pin in pins)
            {
                var awsRecord = new AwsCloudsearchDto
                {
                    type = "add",
                    id = pin.AddressId + "-" + pin.PinType + "-" + pin.ParticipantId + "-" + pin.GroupId,
                    fields = pin
                };
                pinlist.Add(awsRecord);
            }
            return pinlist;
        }

        public AwsBoundingBox BuildBoundingBox(string upperleftlat , string upperleftlng , string bottomrightlat , string bottomrightlng )
        {
            var ulLat = Convert.ToDouble(upperleftlat.Replace("$", "."));
            var ulLng = Convert.ToDouble(upperleftlng.Replace("$", "."));

            var brLat = Convert.ToDouble(bottomrightlat.Replace("$", "."));
            var brLng = Convert.ToDouble(bottomrightlng.Replace("$", "."));

            return  new AwsBoundingBox
            {
                UpperLeftCoordinates = new GeoCoordinates(ulLat,ulLng),
                BottomRightCoordinates = new GeoCoordinates(brLat,brLng)
            };
        }

        public SearchResponse SearchConnectAwsCloudsearch(string querystring, string returnFields, GeoCoordinate originCoords = null, AwsBoundingBox boundingBox = null)
        {
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = AmazonSearchUrl
            };

            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);
            var searchRequest = new Amazon.CloudSearchDomain.Model.SearchRequest
            {
                Query = querystring,
                QueryParser = QueryParser.Structured,
                Size = ReturnRecordCount,
                Return = returnFields + ",_score",
                
            };

            if (boundingBox != null)
            {
                searchRequest.FilterQuery = $"latlong:['{boundingBox.UpperLeftCoordinates.Lat},{boundingBox.UpperLeftCoordinates.Lng}','{boundingBox.BottomRightCoordinates.Lat},{boundingBox.BottomRightCoordinates.Lng}']";
            }
            else
            {
                searchRequest.Size = 31;
            }
               
            if (originCoords != null)
            {
                searchRequest.Expr = $"{{'distance':'haversin({originCoords.Latitude},{originCoords.Longitude},latlong.latitude,latlong.longitude)'}}"; // use to sort by proximity
                searchRequest.Sort = "distance asc";
                searchRequest.Return += ",distance";
            }

            var response = cloudSearch.Search(searchRequest);
            return (response);
        }

        public void UploadNewPinToAWS(PinDto pin)
        {
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = AmazonSearchUrl
            };

            pin = SetLatAndLangOnPinForNewAddress(pin);

            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            AwsConnectDto awsPinObject = Mapper.Map<AwsConnectDto>(pin);

            AwsCloudsearchDto awsPostPinObject = new AwsCloudsearchDto("add", GenerateAwsPinId(pin), awsPinObject);

            var pinlist = new List<AwsCloudsearchDto>();
            pinlist.Add(awsPostPinObject);

            string jsonAwsObject = JsonConvert.SerializeObject(pinlist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            MemoryStream jsonAwsPinDtoStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonAwsObject));

            var upload = new Amazon.CloudSearchDomain.Model.UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                Documents = jsonAwsPinDtoStream
            };

            var response = cloudSearch.UploadDocuments(upload);
        }

        private string GenerateAwsPinId(PinDto pin)
        {
            string awsPinId = pin.Address.AddressID + "-" + pin.PinType + "-" + pin.Participant_ID + "-" + getPinGroupIdOrEmptyString(pin);
            return awsPinId; 
        }

        private string getPinGroupIdOrEmptyString(PinDto pin)
        {
            bool isGathering = pin.PinType == PinType.GATHERING;
            string groupIdAsString = isGathering ? pin.Gathering.GroupId.ToString() : "";

            return groupIdAsString; 
        }

        private PinDto SetLatAndLangOnPinForNewAddress(PinDto pin)
        {
            GeoCoordinate newAddressCoords = _addressGeocodingService.GetGeoCoordinates(pin.Address);

            pin.Address.Latitude = newAddressCoords.Latitude;
            pin.Address.Longitude = newAddressCoords.Longitude;

            return pin;
        }

    }
}