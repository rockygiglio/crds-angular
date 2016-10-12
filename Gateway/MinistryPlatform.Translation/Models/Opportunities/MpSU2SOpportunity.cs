using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Opportunities
{
    public class MpSU2SOpportunity
    {
        [JsonProperty(PropertyName = "Opportunity_ID")]
        public int OpportunityId;
        [JsonProperty(PropertyName = "Opportunity_Title")]
        public string OpportunityTitle;

        [JsonProperty(PropertyName = "rsvpMembers")]
        public List<MpRsvpMember> RsvpMembers { get; set; }

        public MpSU2SOpportunity()
        {
            RsvpMembers = new List<MpRsvpMember>();
        }
    }
}
