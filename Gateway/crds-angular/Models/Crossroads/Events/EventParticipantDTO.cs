using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventParticipantDTO
    {
        [JsonProperty(PropertyName = "eventParticipantId")]
        public int EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "participationStatus")]
        public int ParticipationStatus { get; set; }        
    }
}