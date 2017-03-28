using crds_angular.Models.Finder;

namespace crds_angular.Models.AwsCloudsearch
{
    public class AwsBoundingBox
    {
        public GeoCoordinates UpperLeftCoordinates { get; set; }
        public GeoCoordinates BottomRightCoordinates { get; set; }
    }
}