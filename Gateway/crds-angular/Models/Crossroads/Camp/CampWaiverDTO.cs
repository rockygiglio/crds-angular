using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampWaiverDTO
    {
        [JsonProperty(PropertyName = "waiverId")]
        public int WaiverId { get; set; }

        [JsonProperty(PropertyName = "waiverName")]
        public string WaiverName { get; set; }

        [JsonProperty(PropertyName = "waiverText")]
        public string WaiverText { get; set; }

        [JsonProperty(PropertyName = "waiverRequired")]
        public bool Required { get; set; }
    }
}