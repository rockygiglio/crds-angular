using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Groups
{

    public class GroupDTO
    {
        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "groupDescription")]
        public string GroupDescription { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupTypeId")]
        public int GroupTypeId { get; set; }

        [JsonProperty(PropertyName = "ministryId")]
        public int MinistryId { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int CongregationId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "contactName")]
        public string PrimaryContactName { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty(PropertyName = "availableOnline")]
        public bool AvailableOnline { get; set; }

        [JsonProperty(PropertyName = "remainingCapacity")]
        public int RemainingCapacity { get; set; }

        [JsonProperty(PropertyName = "groupFullInd")]
        public bool GroupFullInd { get; set; }

        [JsonProperty(PropertyName = "waitListInd")]
        public bool WaitListInd { get; set; }

        [JsonProperty(PropertyName = "waitListGroupId")]
        public int WaitListGroupId { get; set; }

        [JsonProperty(PropertyName = "childCareInd")]
        public bool ChildCareAvailable { get; set; }

        [JsonProperty(PropertyName = "minAge")]
        public int OnlineRsvpMinimumAge { get; set; }

        public List<SignUpFamilyMembers> SignUpFamilyMembers { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<Event> Events { get; set; }

        [JsonProperty(PropertyName = "meetingDayId")]
        public int MeetingDayId { get; set; }

        [JsonProperty(PropertyName = "meetingTime")]
        public TimeSpan? MeetingTime { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "addresss")]
        public AddressDTO Address { get; set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<crds_angular.Models.Crossroads.Profile.ContactAttributeDTO> Attributes { get; set; }
    }


}