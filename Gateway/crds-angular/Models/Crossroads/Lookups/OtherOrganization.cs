using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Lookups
{
    public class OtherOrganization
    {
        public OtherOrganization(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; }
        
    }
}