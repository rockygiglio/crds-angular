using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildCareDate
    {
        [JsonProperty(PropertyName = "eventDate")]
        public DateTime EventDate { get; set; }
        [JsonProperty(PropertyName = "cancelled")]
        public bool Cancelled { get; set; }
        [JsonProperty(PropertyName = "communityGroups")]
        public List<ChildcareGroup> Groups { get; set;}

        public ChildCareDate()
        {
            Groups = new List<ChildcareGroup>();
        }
    }
}