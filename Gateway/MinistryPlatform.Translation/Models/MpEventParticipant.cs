using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpEventParticipant
    {
        public bool ChildcareRequired { get; set; }
        public int ContactId { get; set; }
        public int EventId { get; set; }
        public int EventParticipantId { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public string EventTitle { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupParticipantId { get; set; }
        public int ParticipantId { get; set; }
        public string ParticipantEmail { get; set; }
        public int ParticipantStatus { get; set; }
        public int RoomId { get; set; }
    }
}