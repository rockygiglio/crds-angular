using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampWaiverResponseDTO
    {
        [JsonProperty(PropertyName = "waiverId")]
        public int WaiverId { get; set; }

        [JsonProperty(PropertyName = "accepted")]
        public bool WaiverAccepted { get; set; }
    }
}