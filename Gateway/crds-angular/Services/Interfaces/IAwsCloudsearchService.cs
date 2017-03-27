using System.Device.Location;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Models.AwsCloudsearch;

namespace crds_angular.Services.Interfaces
{
    public interface IAwsCloudsearchService
    {
        UploadDocumentsResponse UploadAllConnectRecordsToAwsCloudsearch();
        UploadDocumentsResponse DeleteAllConnectRecordsInAwsCloudsearch();
        SearchResponse SearchConnectAwsCloudsearch(string querystring, string returnFields, GeoCoordinate originCoords = null, AwsBoundingBox boundingBox = null);
        AwsBoundingBox BuildBoundingBox(string upperleftlat, string upperleftlng, string bottomrightlat, string bottomrightlng);
    }
}