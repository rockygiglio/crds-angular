using System;
using MinistryPlatform.Translation.Models;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class Organization
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "openSignup")]
        public bool OpenSignup { get; set; }

        [JsonProperty(PropertyName = "organizationId")]
        public int OrganizationId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        public Organization FromMpOrganization(MPOrganization mpOrg)
        {
            return new Organization()
            {
                OrganizationId = mpOrg.OrganizationId,
                ContactId = mpOrg.ContactId,
                EndDate = mpOrg.EndDate,
                StartDate = mpOrg.StartDate,
                Name = mpOrg.Name,
                OpenSignup = mpOrg.OpenSignup
            };
        }

        public MPOrganization ToMpOrganization(Organization org)
        {
            return new MPOrganization()
            {
                OrganizationId = org.OrganizationId,
                ContactId = org.ContactId,
                EndDate = org.EndDate,
                StartDate = org.StartDate,
                Name = org.Name,
                OpenSignup = org.OpenSignup
            };
        }
    }
}