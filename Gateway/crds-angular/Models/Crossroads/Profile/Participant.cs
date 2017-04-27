using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Profile
{
   
   public class Participant
    {
        [JsonProperty(PropertyName = "ContactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "ParticipantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "EmailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "PreferredName")]
        public string PreferredName { get; set; }

        [JsonProperty(PropertyName = "DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "Age")]
        public int? Age { get; set; }

        [JsonProperty(PropertyName = "AttendanceStart")]
        public DateTime? AttendanceStart { get; set; }

        [JsonProperty(PropertyName = "ApprovedSmallGroupLeader")]
        public bool ApprovedSmallGroupLeader { get; set; }

        [JsonProperty(PropertyName = "Role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "GroupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "GroupLeaderStatus")]
        public int GroupLeaderStatus { get; set; }
    }

}