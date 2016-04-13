using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class PreferredLaunchSite
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}