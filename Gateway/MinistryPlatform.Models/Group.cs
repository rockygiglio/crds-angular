using System;
using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupRole { get; set; }
        public int GroupType { get; set; }
        public int TargetSize { get; set; }
        public string Name { get; set; }
        public IList<GroupParticipant> Participants { get; set; }
        public Boolean Full { get; set; }
        public Boolean WaitList { get; set; }
        public int WaitListGroupId { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public int EventTypeId { get; set; }
        public bool ChildCareAvailable { get; set; }
        public string Congregation { get; set; }
        public int MinimumAge { get; set; }
        public string GroupDescription { get; set; }
        public int MinistryId { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public int MeetingDayId { get; set; }
        public int CongregationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AvailableOnline { get; set; }
        public int RemainingCapacity { get; set; }
        public int ContactId { get; set; }
        public int GroupRoleId { get; set; }
        public Address Address { get; set; }


        public Group()
        {
            Participants = new List<GroupParticipant>();
            ChildCareAvailable = false;
        }
    }
}
