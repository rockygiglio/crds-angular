using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Models.Crossroads.Profile;
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

        [JsonProperty(PropertyName = "primaryContactEmail")]
        public string PrimaryContactEmail { get; set; }

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
        public int? MeetingDayId { get; set; }

        [JsonProperty(PropertyName = "meetingTime")]
        public string MeetingTime { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "address")]
        public AddressDTO Address { get; set; }

        [JsonProperty(PropertyName = "attributeTypes")]
        public Dictionary<int, ObjectAttributeTypeDTO> AttributeTypes { get; set; }

        [JsonProperty(PropertyName = "singleAttributes")]
        public Dictionary<int, ObjectSingleAttributeDTO> SingleAttributes { get; set; }
    }
}