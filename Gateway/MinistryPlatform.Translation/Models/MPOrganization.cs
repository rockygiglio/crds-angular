using System;

namespace MinistryPlatform.Translation.Models
{
    public class MPOrganization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public bool OpenSignup { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ContactId { get; set; }
    }
}
