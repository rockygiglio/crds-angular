using System;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class MpChildcareRequestEmail
    {
        public int RequestId { get; set; }
        public string Requester { get; set; }
        public int RequesterId { get; set; }
        public String RequesterEmail { get; set; }
        public String RequesterNickname { get; set; }
        public String RequesterLastName { get; set; }
        public String MinistryName { get; set; }
        public String GroupName{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Frequency { get; set; }
        public string ChildcareSession { get; set; }
        public int EstimatedChildren { get; set; }
        public string CongregationName { get; set; }
        
        public int ChildcareContactId { get; set; }
        public string ChildcareContactEmail { get; set; }
    }
}
    
