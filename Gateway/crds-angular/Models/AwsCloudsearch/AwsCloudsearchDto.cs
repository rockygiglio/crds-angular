using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.AwsCloudsearch
{
    public class AwsCloudsearchDto
    {
        public string type { get; set; }
        public string id { get; set; }
        public AwsConnectDto fields { get; set; }
    }
}