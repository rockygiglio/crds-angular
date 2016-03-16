using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class GroupConnector
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}