using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GroupLeader
{
    public class GroupLeaderProfileDTO
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "oldEmail")]
        public string OldEmail { get; set; }

        [JsonProperty(PropertyName = "site")]
        public int Site { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string MobilePhone { get; set; }

        [JsonProperty(PropertyName = "householdId")]
        public int HouseholdId { get; set; }
    }
}