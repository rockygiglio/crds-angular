using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Models
{
    public class MpCommunication
    {
        public MpCommunication()
        {
            ToContacts = new List<MpContact>();
        }

        public int AuthorUserId { get; set; }
        public int DomainId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public MpContact FromContact { get; set; }
        public MpContact ReplyToContact { get; set; }
        public List<MpContact> ToContacts { get; set; } 
        public int TemplateId { get; set; }
        public Dictionary<string, object> MergeData { get; set; }
        public DateTime StartDate { get; set; }
    }
}