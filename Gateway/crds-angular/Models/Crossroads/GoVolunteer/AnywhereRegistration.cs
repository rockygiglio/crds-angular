using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class AnywhereRegistration : Registration
    {
        [JsonProperty(PropertyName = "organizationId")]
        public int OrganizationId { get; set; }
    }
}