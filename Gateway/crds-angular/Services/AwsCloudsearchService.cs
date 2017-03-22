using System.Collections.Generic;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using Amazon;
using Amazon.CloudSearch;
using Amazon.CloudSearch.Model;
using Amazon.CloudSearchDomain;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Models.AwsCloudsearch;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class AwsCloudsearchService : MinistryPlatformBaseService, IAwsCloudsearchService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IFinderRepository _finderRepository;

        public AwsCloudsearchService(IConfigurationWrapper configurationWrapper,
                                     IFinderRepository finderRepository)
        {
            _configurationWrapper = configurationWrapper;
            _finderRepository = finderRepository;
        }

        private List<AwsCloudseachDto> GetDataForCloudsearch()
        {
            var awsPins = _finderRepository.GetAllPinsForAws();
        }

        private void Test()
        {
            System.Diagnostics.Debug.Write("Test");
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = "http://search-connect-64vbx6ojcohmptmsmolggqbpte.us-east-1.cloudsearch.amazonaws.com",
                //RegionEndpoint = Amazon.RegionEndpoint.SAEast1
            };

            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);
            var searchRequest = new Amazon.CloudSearchDomain.Model.SearchRequest
            {
                Query = "Dawg",
                QueryParser = QueryParser.Lucene
            };

            var response = cloudSearch.Search(searchRequest);
            System.Diagnostics.Debug.Write(response);
        }

        private void Test2()
        {
            System.Diagnostics.Debug.Write("Testing Upload");
            var domainConfig = new AmazonCloudSearchDomainConfig
            {
                ServiceURL = "http://search-connect-64vbx6ojcohmptmsmolggqbpte.us-east-1.cloudsearch.amazonaws.com",
                //RegionEndpoint = Amazon.RegionEndpoint.SAEast1
            };
            var cloudSearch = new Amazon.CloudSearchDomain.AmazonCloudSearchDomainClient(domainConfig);

            var path = @"C:\Users\Markku\Desktop\bobjson.txt";

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

    }
}