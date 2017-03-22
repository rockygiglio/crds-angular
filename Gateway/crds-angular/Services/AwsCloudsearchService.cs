using System.Collections.Generic;
using System.Linq;
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

        public void UploadAllConnectRecordsToAwsCloudsearch()
        {
            System.Diagnostics.Debug.Write("Testing Upload");
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = amazonSearchURL
                //RegionEndpoint = Amazon.RegionEndpoint.SAEast1
            };
            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var path = @"C:\Users\Markku\Desktop\connect_json.txt";

            var pinList = GetDataForCloudsearch();

            //serialize
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Error = (serializer, err) => err.ErrorContext.Handled = true;
            string json = JsonConvert.SerializeObject(pinList, settings);
            System.Diagnostics.Debug.Write(json);

            //var ms = new MemoryStream();
            //FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            //{
            //    byte[] bytes = new byte[file.Length];
            //    file.Read(bytes, 0, (int)file.Length);
            //    ms.Write(bytes, 0, (int)file.Length);
            //}

            var upload = new Amazon.CloudSearchDomain.Model.UploadDocumentsRequest()
            {
                ContentType = ContentType.ApplicationJson,
                // Documents = file
                FilePath = path
            };

            var response = cloudSearch.UploadDocuments(upload);

            System.Diagnostics.Debug.Write(response);
        }

        

        private List<AwsCloudseachDto> GetDataForCloudsearch()
        {
            var awsPins = _finderRepository.GetAllPinsForAws();
            return awsPins.Select(Mapper.Map<AwsCloudseachDto>).ToList();
        }




        public SearchResponse SearchConnectAwsCloudsearch()
        {
            System.Diagnostics.Debug.Write("Test");
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = amazonSearchURL
                //RegionEndpoint = Amazon.RegionEndpoint.SAEast1
            };

            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);
            var searchRequest = new Amazon.CloudSearchDomain.Model.SearchRequest
            {
                Query = "group"
            };

            var response = cloudSearch.Search(searchRequest);
            System.Diagnostics.Debug.Write(response);
            return (response);
        }


    }
}