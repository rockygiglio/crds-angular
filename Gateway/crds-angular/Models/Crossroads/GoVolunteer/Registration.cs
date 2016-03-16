using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using crds_angular.Models.Crossroads.Profile;
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

        [JsonProperty(PropertyName = "congregationId")]
        [Required]
        public int OrganizationId { get; set; }

        [JsonProperty(PropertyName = "preferredLaunchSiteId")]
        public int PreferredLaunchSiteId { get; set; }

        [JsonProperty(PropertyName = "prepWork")]
        public List<PrepWork> PrepWork { get; set; }

        [JsonProperty(PropertyName = "projectPreferences")]
        public List<ProjectPreference> ProjectPreferences { get; set; }

        [JsonProperty(PropertyName = "registrationId")]
        public int RegistrationId { get; set; }

        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }

        [JsonProperty(PropertyName = "self")]
        public Person Self { get; set; }

        [JsonProperty(PropertyName = "spouse")]
        public Person Spouse { get; set; }

        [JsonProperty(PropertyName = "spouseParticipation")]
        [Required]
        public bool SpouseParticipation { get; set; }

        [JsonProperty(PropertyName = "waiverSigned")]
        [Required]
        public bool WaiverSigned { get; set; }
    }

    public class GroupConnector
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    public class ChildrenAttending
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    public class PrepWork
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "spouse")]
        public bool Spouse { get; set; }
    }

    public class Equipment
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }

    public class ProjectPreference
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }
    }
}