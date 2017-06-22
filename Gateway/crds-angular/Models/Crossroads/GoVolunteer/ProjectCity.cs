using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class ProjectCity
    {
        [JsonProperty(PropertyName = "projectId")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}