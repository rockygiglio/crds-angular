using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class Project
    {
        [JsonProperty(PropertyName = "projectId")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

        [JsonProperty(PropertyName = "projectStatusId")]
        public int ProjectStatusId { get; set; }

        [JsonProperty(PropertyName = "locationId")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "projectTypeId")]
        public int ProjectTypeId { get; set; }

        [JsonProperty(PropertyName = "organizationId")]
        public int OrganizationId { get; set; }

        [JsonProperty(PropertyName = "initiativeId")]
        public int InitiativeId { get; set; }

        [JsonProperty(PropertyName = "addressId")]
        public int AddressId { get; set; }
    }
}