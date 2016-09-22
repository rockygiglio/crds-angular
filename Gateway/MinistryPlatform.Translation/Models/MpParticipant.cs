using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpParticipant
    {
        public int ContactId { get; set; }
        public int ParticipantId { get; set;}
        public string EmailAddress { get; set; }
        public string PreferredName { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public DateTime? AttendanceStart { get; set; }
        public bool ApprovedSmallGroupLeader { get; set; }
        public string Role { get; set; }
        public string GroupName { get; set; }
        public string Nickname { get; set; }
    }
}
