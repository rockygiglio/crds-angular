using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.CloudSearchDomain.Model;

namespace crds_angular.Services.Interfaces
{
    public interface IAwsCloudsearchService
    {
        UploadDocumentsResponse UploadAllConnectRecordsToAwsCloudsearch();
        UploadDocumentsResponse DeleteAllConnectRecordsInAwsCloudsearch();
        SearchResponse SearchConnectAwsCloudsearch(string querystring, int size, string returnfields);
    }
}