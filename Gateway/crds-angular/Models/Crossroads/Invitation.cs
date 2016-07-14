using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class Invitation
    {
        
        [JsonProperty(PropertyName = "sourceId")]
        [Required]
        public int SourceId { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        [Required]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "recipientName")]
        [Required]
        public string RecipientName { get; set; }

        [JsonProperty(PropertyName = "requestDate")]
        [Required]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "invitationType")]
        [Required]
        public int InvitationType { get; set; }
    }

}