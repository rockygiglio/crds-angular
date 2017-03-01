using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class CincinnatiRegistration : Registration
    {
        [JsonProperty(PropertyName = "additionalInformation")]
        [StringLength(500, ErrorMessage = "AdditionalInformation value cannot exceed 500 characters. ")]
        public string AdditionalInformation { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<ChildrenAttending> ChildAgeGroup { get; set; }

        [JsonProperty(PropertyName = "createGroupConnector")]
        public bool CreateGroupConnector { get; set; }

        [JsonProperty(PropertyName = "equipment")]
        public List<Equipment> Equipment { get; set; }

        [JsonProperty(PropertyName = "groupConnector")]
        public GroupConnector GroupConnector { get; set; }

        [JsonProperty(PropertyName = "otherOrganizationName")]
        public string OtherOrganizationName { get; set; }

        [JsonProperty(PropertyName = "prepWork")]
        public List<PrepWork> PrepWork { get; set; }

        [JsonProperty(PropertyName = "privateGroup")]
        public bool PrivateGroup { get; set; }

        [JsonProperty(PropertyName = "projectPreferences")]
        public List<ProjectPreference> ProjectPreferences { get; set; }

        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }

        [JsonProperty(PropertyName = "skills")]
        public List<GoSkills> Skills { get; set; }

        [JsonProperty(PropertyName = "spouse")]
        public Registrant Spouse { get; set; }

        [JsonProperty(PropertyName = "waiverSigned")]
        [Required]
        public bool WaiverSigned { get; set; }
    }
}