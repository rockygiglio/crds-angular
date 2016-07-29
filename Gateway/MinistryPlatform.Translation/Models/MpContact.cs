using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MinistryPlatform.Translation.Models
{
    public class MpContact
    {
        [JsonProperty(PropertyName = "Contact_ID")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "Email_Address")]
        public string EmailAddress { get; set; }
    }
}