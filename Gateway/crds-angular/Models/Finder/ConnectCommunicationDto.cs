using crds_angular.Models.Crossroads;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class ConnectCommunicationDto
    {
        [JsonProperty("connectCommunicationsId")]
        public int ConnectCommunicationsId { get; set; }

        [JsonProperty("fromUserContactId")]
        public int FromUserContactId { get; set; }

        [JsonProperty("toUserContactId")]
        public int ToUserContactId { get; set; }

        [JsonProperty("communicationId")]
        public int CommunicationId { get; set; }

        [JsonProperty("communicationType")]
        public string CommunicationType { get; set; }

        [JsonProperty("communicationStatus")]
        public string CommunicationStatus { get; set; }
    }
}