
using System;

namespace MinistryPlatform.Translation.Models.Groups
{
    public class GroupDetails
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupTypeId { get; set; }
        public int GroupRoleId { get; set; }
        public int GroupMinistryId { get; set; }
        public int GroupCongregationId { get; set; }
        public int GroupPrimaryContactId { get; set; }
        public string GroupDescription { get; set; }
        public DateTime GroupStartDate { get; set; }
        public DateTime GroupEndDate { get; set; }
        public int GroupMeetingDayId { get; set; }
        public DateTime GroupMeetingTime { get; set; }
        public bool GroupAvailableOnline { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
