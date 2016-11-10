using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampEmergencyContactDTO
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "relationship")]
        public string Relationship { get; set; }

        [JsonProperty(PropertyName = "primaryContact")]
        public bool PrimaryEmergencyContact { get; set; }
    }
}
