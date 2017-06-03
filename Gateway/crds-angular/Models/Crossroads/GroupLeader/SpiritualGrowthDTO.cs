using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GroupLeader
{
    public class SpiritualGrowthDTO
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "story")]
        public string Story { get; set; }

        [JsonProperty(PropertyName = "taught")]
        public string Taught { get; set; }
    }
}