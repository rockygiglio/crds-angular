using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Crossroads.Subscription
{
    public class OptInRequest
    {
        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty(PropertyName = "listName")]
        public string ListName { get; set; }
    }
}