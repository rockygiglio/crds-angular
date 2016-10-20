using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class RsvpMembers
    {
        [JsonProperty(PropertyName = "Event_ID")]
        public long EventId { get; set; }

        [JsonProperty(PropertyName = "Participant_ID")]
        public long ParticipantId { get; set; }

        [JsonProperty(PropertyName = "Opportunity_ID")]
        public long Opportunity { get; set; }

        [JsonProperty(PropertyName = "Group_Role_ID")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "NickName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Response_Result_ID")]
        public int ResponseResultId { get; set; } = 0;

        [JsonProperty(PropertyName = "age")]
        public int? Age { get; set; }
    }
}