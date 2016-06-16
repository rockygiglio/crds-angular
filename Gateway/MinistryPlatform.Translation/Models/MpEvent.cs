using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Models
{
    public class MpEvent
    {
        private IList<int> participants = new List<int>();

        public int EventId { get; set; }
        public string Congregation { get; set; }
        public string Site { get; set; }
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public int? ParentEventId { get; set; }
        public MpContact PrimaryContact { get; set; }

        public IList<int> Participants
        {
            get { return (participants); }
        }

        public int CongregationId { get; set; }
        public int ReminderDaysPriorId { get; set; }
        public bool Template { get; set; }
    }
}