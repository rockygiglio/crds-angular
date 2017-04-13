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
        protected string AwsAccessKeyId;
        protected string AwsSecretAccessKey;

        public AwsCloudsearchService(IAddressGeocodingService addressGeocodingService, 
                                     IFinderRepository finderRepository,                          
                                     IConfigurationWrapper configurationWrapper)
        {
            _addressGeocodingService = addressGeocodingService;
            _finderRepository = finderRepository;
            _configurationWrapper = configurationWrapper;

            AmazonSearchUrl    = _configurationWrapper.GetEnvironmentVarAsString("CRDS_AWS_CONNECT_ENDPOINT");
            AwsAccessKeyId     = _configurationWrapper.GetEnvironmentVarAsString("CRDS_AWS_CONNECT_ACCESSKEYID");
            AwsSecretAccessKey = _configurationWrapper.GetEnvironmentVarAsString("CRDS_AWS_CONNECT_SECRETACCESSKEY");


        }

        public UploadDocumentsResponse UploadAllConnectRecordsToAwsCloudsearch()
        {
            var cloudSearch = new AmazonCloudSearchDomainClient(AwsAccessKeyId, AwsSecretAccessKey, AmazonSearchUrl);

            var pinList = GetDataForCloudsearch();

            //serialize
            var json = JsonConvert.SerializeObject(pinList, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var upload = new UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                Documents = ms
            };

            return(cloudSearch.UploadDocuments(upload));
        }

        public UploadDocumentsResponse DeleteAllConnectRecordsInAwsCloudsearch()
        {
            var cloudSearch = new AmazonCloudSearchDomainClient(AwsAccessKeyId, AwsSecretAccessKey, AmazonSearchUrl);

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
            var upload = new UploadDocumentsRequest()
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

        public SearchResponse SearchConnectAwsCloudsearch(string querystring, string returnFields, int returnSize = 10000, GeoCoordinate originCoords = null, AwsBoundingBox boundingBox = null)
        {
            var cloudSearch = new AmazonCloudSearchDomainClient(AwsAccessKeyId, AwsSecretAccessKey, AmazonSearchUrl);
            var searchRequest = new SearchRequest
            {
                Query = querystring,
                QueryParser = QueryParser.Structured,
                Size = returnSize,
                Return = returnFields + ",_score"
            };

            if (boundingBox != null)
            {
                searchRequest.FilterQuery = $"latlong:['{boundingBox.UpperLeftCoordinates.Lat},{boundingBox.UpperLeftCoordinates.Lng}','{boundingBox.BottomRightCoordinates.Lat},{boundingBox.BottomRightCoordinates.Lng}']";
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

        public void UploadNewPinToAws(PinDto pin)
        {
            var cloudSearch = new AmazonCloudSearchDomainClient(AwsAccessKeyId, AwsSecretAccessKey, AmazonSearchUrl);
            var upload = GetObjectToUploadToAws(pin);
            cloudSearch.UploadDocuments(upload);
        }

        private string GenerateAwsPinId(PinDto pin)
        {
            var awsPinId = pin.Address.AddressID + "-" + (int)pin.PinType + "-" + pin.Participant_ID + "-" + GetPinGroupIdOrEmptyString(pin);
            return awsPinId; 
        }

        private static string GetPinGroupIdOrEmptyString(PinDto pin)
        {
            var isGathering = pin.PinType == PinType.GATHERING;
            var groupIdAsString = isGathering ? pin.Gathering.GroupId.ToString() : "";

            return groupIdAsString; 
        }

        public UploadDocumentsRequest GetObjectToUploadToAws(PinDto pin)
        {
            AwsConnectDto awsPinObject = Mapper.Map<AwsConnectDto>(pin);

            AwsCloudsearchDto awsPostPinObject = new AwsCloudsearchDto("add", GenerateAwsPinId(pin), awsPinObject);

            var pinlist = new List<AwsCloudsearchDto> {awsPostPinObject};

            string jsonAwsObject = JsonConvert.SerializeObject(pinlist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            MemoryStream jsonAwsPinDtoStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonAwsObject));

            UploadDocumentsRequest upload = new UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                Documents = jsonAwsPinDtoStream
            };

            return upload;
        }

    }
}