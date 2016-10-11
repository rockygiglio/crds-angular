using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models
{
    public class RsvpYesMembers
    {
        [JsonProperty(PropertyName = "Event_ID")]
        public long EventId { get; set; }

        [JsonProperty(PropertyName = "Participant_ID")]
        public long ParticipantId { get; set; }

        [JsonProperty(PropertyName = "Opportunity_ID")]
        public long Opportunity { get; set; }

        [JsonProperty(PropertyName = "Group_Role_Id")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "NickName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Opportunity_Title")]
        public string OpportunityTitle { get; set; }
    }
}