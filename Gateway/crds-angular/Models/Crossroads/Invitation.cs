using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class Invitation
    {
        
        [JsonProperty(PropertyName = "sourceId")]
        public int SourceId { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "recipientName")]
        public string RecipientName { get; set; }

        [JsonProperty(PropertyName = "requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "invitationType")]
        public int InvitationType { get; set; }
    }

}