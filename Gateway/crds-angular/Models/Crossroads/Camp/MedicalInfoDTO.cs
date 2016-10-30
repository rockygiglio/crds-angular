using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class MedicalInfoDTO
    {
        [JsonProperty(PropertyName = "insuranceCompany")]
        public string InsuranceCompany { get; set; }

        [JsonProperty(PropertyName = "policyHolder")]
        public string PolicyHolder { get; set; }

        [JsonProperty(PropertyName = "physicianName")]
        public string PhysicianName { get; set; }

        [JsonProperty(PropertyName = "physicianPhone")]
        public string PhysicianPhone { get; set; }
    }
}