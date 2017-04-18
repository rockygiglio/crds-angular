using crds_angular.Models.Crossroads;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class HostRequestDto
    {
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty("groupName")]
        public int GroupName { get; set; }

        [JsonProperty("address")]
        public AddressDTO Address { get; set; }
    }
}