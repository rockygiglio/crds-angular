using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpInquiry
    {
        public int ContactId { get; set; }

        public int InquiryId { get; set; }

        public int GroupId { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RequestDate { get; set; }

        public bool Placed { get; set; }
    }

}