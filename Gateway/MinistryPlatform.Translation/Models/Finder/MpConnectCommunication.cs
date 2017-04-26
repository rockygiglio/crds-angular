using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Finder
{
    [MpRestApiTable(Name = "cr_Connect_Communications")]
    public class MpConnectCommunication
    {
        [JsonProperty(PropertyName = "Connect_Communication_ID")]
        public int ConnectCommunicationId { get; set; }

        [JsonProperty(PropertyName = "From_Contact_ID")]
        public int FromContactId { get; set; }

        [JsonProperty(PropertyName = "To_Contact_ID")]
        public int ToContactId { get; set; }

        [JsonProperty(PropertyName = "Communication_Type_ID")]
        public int CommunicationTypeId { get; set; }

        [JsonProperty(PropertyName = "Communication_Status_ID")]
        public int CommunicationStatusId { get; set; }

        [JsonProperty(PropertyName = "Group_ID")]
        public int GroupId { get; set; }
    }
}
