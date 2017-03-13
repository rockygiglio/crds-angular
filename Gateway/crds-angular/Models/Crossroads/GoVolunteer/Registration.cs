using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class Registration
    {
        [JsonProperty(PropertyName = "initiativeId")]
        [Required]
        public int InitiativeId { get; set; }

        [JsonProperty(PropertyName = "registrationId")]
        public int RegistrationId { get; set; }

        [JsonProperty(PropertyName = "groupConnectorId")]
        public int GroupConnectorId { get; set; }

        [JsonProperty(PropertyName = "groupConnector")]
        public GroupConnector GroupConnector { get; set; }

        [JsonProperty(PropertyName = "preferredLaunchSite")]
        public PreferredLaunchSite PreferredLaunchSite { get; set; }

        [JsonProperty(PropertyName = "spouseParticipation")]
        [Required]
        public bool SpouseParticipation { get; set; }

        [JsonProperty(PropertyName = "self")]
        public Registrant Self { get; set; }
    }
}