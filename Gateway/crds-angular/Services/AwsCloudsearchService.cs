using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using Amazon;
using Amazon.CloudSearch;
using Amazon.CloudSearch.Model;
using Amazon.CloudSearchDomain;
using Amazon.CloudSearchDomain.Model;
using AutoMapper;
using crds_angular.Models.AwsCloudsearch;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class AwsCloudsearchService : MinistryPlatformBaseService, IAwsCloudsearchService
    {
        private readonly IFinderRepository _finderRepository;
        private readonly string amazonSearchURL = "https://search-connect-int-sdjkhnnriypxn3ijhn4k5xkxq4.us-east-1.cloudsearch.amazonaws.com";

        public AwsCloudsearchService(IFinderRepository finderRepository)
        {
            _finderRepository = finderRepository;
        }

        public UploadDocumentsResponse UploadAllConnectRecordsToAwsCloudsearch()
        {
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = amazonSearchURL
            };
            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var pinList = GetDataForCloudsearch();

            //serialize
            var json = JsonConvert.SerializeObject(pinList, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            System.Diagnostics.Debug.Write(json);

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
                ServiceURL = amazonSearchURL
            };
            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var results = SearchConnectAwsCloudsearch("matchall", 10000, "_no_fields");
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
                    id = pin.AddressId.ToString(),
                    fields = pin
                };
                pinlist.Add(awsRecord);
            }
            return pinlist;
        }
        

        public SearchResponse SearchConnectAwsCloudsearch(string querystring, int size, string returnfields)
        {
            System.Diagnostics.Debug.Write("Test");
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = amazonSearchURL
            };

            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);
            var searchRequest = new Amazon.CloudSearchDomain.Model.SearchRequest
            {
                Query = querystring,
                QueryParser = QueryParser.Structured,
                Size = size,
                Return = returnfields
            };

            var response = cloudSearch.Search(searchRequest);
            System.Diagnostics.Debug.Write(response);
            return (response);
        }


    }
}