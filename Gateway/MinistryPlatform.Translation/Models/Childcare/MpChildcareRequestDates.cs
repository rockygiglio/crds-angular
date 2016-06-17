using System;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class MpChildcareRequestDate
    {
        public int ChildcareRequestDateId { get; set; }
        public int ChildcareRequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public bool Approved { get; set; }
    }
}
