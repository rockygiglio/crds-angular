using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "dp_Users")]
    public class MpUser
    {
        [JsonProperty(PropertyName = "User_Name")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "User_GUID")]
        public string Guid { get; set; }

        [JsonProperty(PropertyName = "Can_Impersonate")]
        public bool CanImpersonate { get; set; }

        [JsonProperty(PropertyName = "User_Email")]
        public string UserEmail { get; set; }

        [JsonProperty(PropertyName = "User_ID")]
        public int UserRecordId { get; set; }

        public string DisplayName { get; set; }
    }
}
