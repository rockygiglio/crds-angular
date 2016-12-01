using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    public class MpStoredProcBool
    {
        [JsonProperty(PropertyName = "Status")]
        public bool isTrue { get; set; }
    }
}
