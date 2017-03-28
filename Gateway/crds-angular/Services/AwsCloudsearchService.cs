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
        private readonly IFinderRepository _finderRepository;
        private readonly IConfigurationWrapper _configurationWrapper;
        protected string AmazonSearchUrl;
        private const int ReturnRecordCount = 10000;

        public AwsCloudsearchService(IFinderRepository finderRepository,
                                     IConfigurationWrapper configurationWrapper)
        {
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
                ServiceURL = "http://search-connect-int-sdjkhnnriypxn3ijhn4k5xkxq4.us-east-1.cloudsearch.amazonaws.com"
            };

            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var path = @"C:\Code\myjsonfile.txt";

            var ms = new MemoryStream();
            FileStream jsonFileToUpload = new FileStream(path, FileMode.Open, FileAccess.Read);

            var upload = new Amazon.CloudSearchDomain.Model.UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                Documents = jsonFileToUpload
            };

            var response = cloudSearch.UploadDocuments(upload);
            System.Diagnostics.Debug.WriteLine("test");
        }


    }
}