using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Finder
{
    [MpRestApiTable(Name = "cr_Connect_Communications")]
    public class MpConnectCommunication
    {
        [JsonProperty(PropertyName = "Connect_Communication_ID")]
        public int ConnectCommunicationId { get; set; }

        [JsonProperty(PropertyName = "FromUser_Contact_ID")]
        public int FromUserContactId { get; set; }

        [JsonProperty(PropertyName = "ToUser_Contact_ID")]
        public int ToUserContactId { get; set; }

        [JsonProperty(PropertyName = "Communication_ID")]
        public int CommunicationId { get; set; }

        [JsonProperty(PropertyName = "Communication_Type")]
        public string CommunicationType { get; set; }

        [JsonProperty(PropertyName = "Communication_Status")]
        public string CommunicationStatus { get; set; }
    }
}
