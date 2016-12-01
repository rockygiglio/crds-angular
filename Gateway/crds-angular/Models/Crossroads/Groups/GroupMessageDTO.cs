using Newtonsoft.Json;
using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Groups
{
    public class GroupMessageDTO
    {
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "participants")]
        public List<GroupParticipantDTO> Participants { get; set; }
    }
}