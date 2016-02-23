using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Groups
{

    public class GroupParticipantDTO
    {
        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "nickName")]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        [JsonProperty(PropertyName = "groupRoleTitle")]
        public string GroupRoleTitle { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<crds_angular.Models.Crossroads.Profile.ContactAttributeDTO> Attributes { get; set; }
    }
}