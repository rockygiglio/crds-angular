using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.Attribute;
using Newtonsoft.Json;

namespace crds_angular.Models.Connect
{
    public class GroupDto
    {
        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "groupDescription")]
        public string GroupDescription { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupTypeId")]
        public int GroupTypeId { get; set; }

        [JsonProperty(PropertyName = "groupTypeName")]
        public string GroupTypeName { get; set; }

        [JsonProperty(PropertyName = "ministryId")]
        public int MinistryId { get; set; }

        [JsonProperty(PropertyName = "congregationId")]
        public int CongregationId { get; set; }

        [JsonProperty(PropertyName = "congregationName")]
        public string Congregation { get; set; }

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

        [JsonProperty(PropertyName = "reasonEndedId")]
        public int? ReasonEndedId { get; set; }

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

        [JsonProperty(PropertyName = "meetingDayId")]
        public int? MeetingDayId { get; set; }

        [JsonProperty(PropertyName = "meetingDay")]
        public string MeetingDay { get; set; }

        [JsonProperty(PropertyName = "meetingTime")]
        public string MeetingTime { get; set; }

        [JsonProperty(PropertyName = "meetingFrequency")]
        public string MeetingFrequency { get; set; }

        [JsonProperty(PropertyName = "meetingFrequencyID")]
        public int? MeetingFrequencyID { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "address")]
        public AddressDto Address { get; set; }

        [JsonProperty(PropertyName = "attributeTypes")]
        public Dictionary<int, ObjectAttributeTypeDTO> AttributeTypes { get; set; }

        [JsonProperty(PropertyName = "singleAttributes")]
        public Dictionary<int, ObjectSingleAttributeDTO> SingleAttributes { get; set; }

        [JsonProperty(PropertyName = "maximumAge")]
        public int MaximumAge { get; set; }

        [JsonProperty(PropertyName = "minimumParticipants")]
        public int MinimumParticipants { get; set; }

        [JsonProperty(PropertyName = "tartgetSize")]
        public int TargetSize { get; set; }

        [JsonProperty(PropertyName = "kidsWelcome")]
        public bool? KidsWelcome { get; set; }

        [JsonProperty(PropertyName = "minorAgeGroupsAdded")]
        public bool MinorAgeGroupsAdded { get; set; } = false;

        [JsonProperty(PropertyName = "proximity")]
        public decimal? Proximity { get; set; }

        public GroupDto()
        {
            AttributeTypes = new Dictionary<int, ObjectAttributeTypeDTO>();
            SingleAttributes = new Dictionary<int, ObjectSingleAttributeDTO>();
        }
    }
}