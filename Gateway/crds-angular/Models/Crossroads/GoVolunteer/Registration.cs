using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class Registration
    {
        // how are we doing skills? - should we reuse attributes on Person object?

        [JsonProperty(PropertyName = "additionalInformation")]
        public string AdditionalInformation { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<ChildrenAttending> ChildAgeGroup { get; set; }

        [JsonProperty(PropertyName = "createGroupConnector")]
        public bool CreateGroupConnector { get; set; }

        [JsonProperty(PropertyName = "equipment")]
        public List<Equipment> Equipment { get; set; }

        [JsonProperty(PropertyName = "groupConnectorId")]
        public int GroupConnectorId { get; set; }

        [JsonProperty(PropertyName = "initiativeId")]
        [Required]
        public int InitiativeId { get; set; }

        [JsonProperty(PropertyName = "organizationId")]
        [Required]
        public int OrganizationId { get; set; }

        [JsonProperty(PropertyName = "preferredLaunchSiteId")]
        public int PreferredLaunchSiteId { get; set; }

        [JsonProperty(PropertyName = "prepWork")]
        public List<PrepWork> PrepWork { get; set; }

        [JsonProperty(PropertyName = "privateGroup")]
        public bool PrivateGroup { get; set; }

        [JsonProperty(PropertyName = "projectPreferences")]
        public List<ProjectPreference> ProjectPreferences { get; set; }

        [JsonProperty(PropertyName = "registrationId")]
        public int RegistrationId { get; set; }

        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }

        // had the existing Person object, but it felt way too heavy
        // and Ministry-Platformy, rolling my own specific person class
        // revisit this decision once intergration starts
        // also, haven't figured out skills yet
        [JsonProperty(PropertyName = "self")]
        public Registrant Self { get; set; }

        [JsonProperty(PropertyName = "spouse")]
        public Registrant Spouse { get; set; }

        [JsonProperty(PropertyName = "spouseParticipation")]
        [Required]
        public bool SpouseParticipation { get; set; }

        [JsonProperty(PropertyName = "waiverSigned")]
        [Required]
        public bool WaiverSigned { get; set; }
    }
}