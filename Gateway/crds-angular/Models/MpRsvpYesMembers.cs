using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Responses")]
    public class MpRsvpYesMember
    {
        [JsonProperty(PropertyName = "Contact_ID")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Role_Id" )]
        public long RoleId { get; set; }

        [JsonProperty(PropertyName = "Leader")]
        public bool IsLeader { get; set; }

        [JsonProperty(PropertyName = "Opportunity_Name")]
        public string OpportunityName { get; set; }
    }
}