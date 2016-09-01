using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    public class MpContact
    {
        [JsonProperty(PropertyName = "Contact_ID")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Email_Address")]
        public string EmailAddress { get; set; }
    }
}