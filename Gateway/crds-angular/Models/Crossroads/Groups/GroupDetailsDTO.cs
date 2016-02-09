using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Groups
{
    
    public class GroupDetailsDTO
    {
        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "groupType")]
        public bool GroupType { get; set; }

        [JsonProperty(PropertyName = "groupMinistry")]
        public bool GroupMinistry { get; set; }

        [JsonProperty(PropertyName = "groupCongregation")]
        public int GroupCongregation { get; set; }

        [JsonProperty(PropertyName = "groupPrimaryContact")]
        public bool GroupPrimaryContact { get; set; }

        [JsonProperty(PropertyName = "groupDescription")]
        public int GroupDescription { get; set; }

        [JsonProperty(PropertyName = "groupStartDate")]
        public int GroupStartDate { get; set; }

        [JsonProperty(PropertyName = "groupEndDate")]
        public int GroupEndDate { get; set; }

        [JsonProperty(PropertyName = "groupTargetSize")]
        public int GroupTargetSize { get; set; }

        [JsonProperty(PropertyName = "groupOffsiteAddress")]
        public int GroupOffsiteAddress { get; set; }

        [JsonProperty(PropertyName = "groupMeetingDay")]
        public int GroupMeetingDay { get; set; }

        [JsonProperty(PropertyName = "groupMeetingTime")]
        public int GroupMeetingTime { get; set; }
    }

   
}