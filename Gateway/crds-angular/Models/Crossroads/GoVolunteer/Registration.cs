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

        [JsonProperty(PropertyName = "organizationId")]
        [Required]
        public int OrganizationId { get; set; }

        [JsonProperty(PropertyName = "registrationId")]
        public int RegistrationId { get; set; }

        [JsonProperty(PropertyName = "self")]
        public Registrant Self { get; set; }
    }
}