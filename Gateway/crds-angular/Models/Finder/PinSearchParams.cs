using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Finder
{
    public class PinSearchQueryParams
    {
        public string UserSearchAddress { get; set; }
        public string FinderType { get; set; }
        public int ContactId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string UpperLeftLat { get; set; }
        public string UpperLeftLng { get; set; }
        public string BottomRightLat { get; set; }
        public string BottomRightLng { get; set; }
    }
}
