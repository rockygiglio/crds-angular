using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class ChildcareRequest
    {
        public int RequesterId { get; set; }
        public int LocationId { get; set; }
        public int MinistryId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Frequency { get; set; }
        public string PreferredTime { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public List<DateTime> DatesList { get; set; }
        public string DecisionNotes { get; set; }
    }
}
