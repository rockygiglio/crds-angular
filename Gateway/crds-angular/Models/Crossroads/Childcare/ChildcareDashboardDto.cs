using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareDashboardDto
    {
        [JsonProperty(PropertyName = "childcareDates")]
        public List<ChildCareDate> AvailableChildcareDates { get; set; }

        public ChildcareDashboardDto()
        {
            AvailableChildcareDates = new List<ChildCareDate>();
        }
    }


    public class ChildCareDate
    {
        [JsonProperty(PropertyName = "eventDate")]
        public DateTime EventDate { get; set; }
        [JsonProperty(PropertyName = "communityGroups")]
        public List<ChildcareGroup> Groups { get; set;}

        public ChildCareDate()
        {
            Groups = new List<ChildcareGroup>();
        }
    }

    public class ChildcareGroup
    {
        [JsonProperty(PropertyName = "eventStartTime")]
        public DateTime EventStartTime { get; set; }

        [JsonProperty(PropertyName = "eventEndTime")]
        public DateTime EventEndTime { get; set; }

        [JsonProperty(PropertyName = "groupMemberName")]
        public string GroupMemberName { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int CongregationId { get; set; }

        [JsonProperty(PropertyName = "maxAge")]
        public int MaximumAge { get; set; }

        [JsonProperty(PropertyName = "remainingCapacity")]
        public int RemainingCapacity { get; set; }
    }
}