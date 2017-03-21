using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using AWSSDK

namespace crds_angular.Services
{
    public class AwsCloudsearchService: MinistryPlatformBaseService, IAwsCloudsearchService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        public AwsCloudsearchService(
                           IConfigurationWrapper configurationWrapper)
        {
            _configurationWrapper = configurationWrapper;
        }
    }

    //TEST ENDPOINT
    //[ResponseType(typeof(AddressDTO))]
  
    public IHttpActionResult Test()
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

        return Ok(response);
    }

    //TEST ENDPOINT
    //[ResponseType(typeof(AddressDTO))]
   
    public IHttpActionResult Test2()
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

        return Ok(response);
    }

}