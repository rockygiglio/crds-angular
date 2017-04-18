using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GroupLeader
{
    public class GroupLeaderProfileDTO
    {
        [JsonProperty(PropertyName = "contactId", Required = Required.Always)]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "firstName", Required = Required.Always)]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "lastName", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "birthDate", Required = Required.Always)]
        public DateTime BirthDate { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "oldEmail", Required = Required.Always)]
        public string OldEmail { get; set; }

        [JsonProperty(PropertyName = "site", Required = Required.Always)]
        public int Site { get; set; }

        [JsonProperty(PropertyName = "mobile", Required = Required.Always)]        
        public string MobilePhone { get; set; }

        [JsonProperty(PropertyName = "householdId", Required = Required.Always)]
        public int HouseholdId { get; set; }

        [JsonProperty(PropertyName = "students", Required = Required.Always)]
        public string LeadStudents { get; set; }

        [JsonProperty(PropertyName = "member", Required = Required.Always)]
        public string ReferenceContactId { get; set; }

        [JsonProperty(PropertyName = "huddle", Required = Required.Always)]
        public string HuddleResponse { get; set; }
    }
}