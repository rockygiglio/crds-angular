using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class ParticipantSignup
    {
        [JsonProperty(PropertyName = "participantId")]
        public int particpantId { get; set; }

        [JsonProperty(PropertyName = "childCareNeeded")]
        public bool childCareNeeded { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int groupRoleId { get; set; }

    }

}