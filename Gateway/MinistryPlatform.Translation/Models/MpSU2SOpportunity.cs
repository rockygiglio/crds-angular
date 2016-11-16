using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    public class MpSU2SOpportunity
    {
        [JsonProperty(PropertyName = "Opportunity_ID")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "Opportunity_Title")]
        public string OpportunityTitle { get; set; }

        [JsonProperty(PropertyName = "rsvpMembers")]
        public List<MpRsvpMember> RsvpMembers { get; set; }

        [JsonProperty(PropertyName = "Group_Role_ID")]
        public int Group_Role_Id { get; set; }

        [JsonProperty(PropertyName = "Role_Title")]
        public string RoleTitle { get; set; }

        [JsonProperty(PropertyName = "Shift_Start")]
        public string ShiftStartTime { get; set; }

        [JsonProperty(PropertyName = "Shift_End")]
        public string ShiftEndTime { get; set; }

        [JsonProperty(PropertyName = "Room")]
        public string Room { get; set; }

        [JsonProperty(PropertyName = "Minimum_Needed")]
        public int? MinimumNeeded { get; set; }
        
        [JsonProperty(PropertyName = "Maximum_Needed")]
        public int? MaximumNeeded { get; set; }

        public MpSU2SOpportunity()
        {
            RsvpMembers = new List<MpRsvpMember>();
        }
    }
}
