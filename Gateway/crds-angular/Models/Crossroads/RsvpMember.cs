using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class RsvpMember
    {
        [JsonProperty(PropertyName = "contactId")]
        public long ContactId { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public long EventId { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public long ParticipantId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        public long Opportunity { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "nickName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "responseResultId")]
        public int ResponseResultId { get; set; } = 0;

        [JsonProperty(PropertyName = "age")]
        public int? Age { get; set; }
    }
}