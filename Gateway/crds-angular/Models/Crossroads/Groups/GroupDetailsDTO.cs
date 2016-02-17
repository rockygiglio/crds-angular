using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Groups
{
    
    public class GroupDetailsDto
    {
        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "groupTypeId")]
        public int GroupTypeId { get; set; }

        [JsonProperty(PropertyName = "ministryId")]
        public int GroupMinistryId { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int GroupCongregationId { get; set; }

        [JsonProperty(PropertyName = "primaryContactId")]
        public int GroupPrimaryContactId { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string GroupDescription { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime GroupStartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime GroupEndDate { get; set; }

        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }

        [JsonProperty(PropertyName = "meetingTime")]
        public DateTime GroupMeetingTime { get; set; }

        [JsonProperty(PropertyName = "meetingDayId")]
        public int GroupMeetingDayId { get; set; }

        [JsonProperty(PropertyName = "groupAvailableOnline")]
        public bool GroupAvailableOnline { get; set; }

    }

   
}