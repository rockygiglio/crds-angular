using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class MPRspvd
    {
        [JsonProperty(PropertyName = "Rsvpd")]
        public bool Rsvpd { get; set; }

    }
}
