using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    public class MpSU2SOpportunity
    {
        [JsonProperty(PropertyName = "Opportunity_ID")]
        public int OpportunityId;

        [JsonProperty(PropertyName = "Opportunity_Title")]
        public string OpportunityTitle;

        [JsonProperty(PropertyName = "rsvpMembers")]
        public List<MpRsvpMember> RsvpMembers { get; set; }

        [JsonProperty(PropertyName = "Group_Role_ID")]
        public int Group_Role_Id { get; set; }

        [JsonProperty(PropertyName = "Shift_Start_Time")]
        public string ShiftStartTime { get; set; }

        [JsonProperty(PropertyName = "Shift_End_Time")]
        public string ShiftEndTime { get; set; }


        public MpSU2SOpportunity()
        {
            RsvpMembers = new List<MpRsvpMember>();
        }
    }
}
