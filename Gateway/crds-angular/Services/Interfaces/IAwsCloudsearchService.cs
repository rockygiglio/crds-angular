using System.Device.Location;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Models.Finder;

namespace crds_angular.Services.Interfaces
{
    public interface IAwsCloudsearchService
    {
        void UploadNewPinToAWS(PinDto pin); 
        UploadDocumentsResponse UploadAllConnectRecordsToAwsCloudsearch();
        UploadDocumentsResponse DeleteAllConnectRecordsInAwsCloudsearch();
        SearchResponse SearchConnectAwsCloudsearch(string querystring, string returnFields, GeoCoordinate originCoords = null, AwsBoundingBox boundingBox = null);
        AwsBoundingBox BuildBoundingBox(string upperleftlat, string upperleftlng, string bottomrightlat, string bottomrightlng);
    }
}