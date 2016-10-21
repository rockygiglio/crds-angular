using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampFamilyMember
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "preferredName")]
        public string PreferredName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "isEligible")]
        public bool IsEligible { get; set; }

        [JsonProperty(PropertyName = "signedUpDate")]
        public DateTime? SignedUpDate { get; set; }
    }
}