using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class DashboardDatum
    {
        [JsonProperty(PropertyName = "name")]
        public string RegistrantName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "adults")]
        public int AdultsParticipating { get; set; }

        [JsonProperty(PropertyName = "children")]
        public int ChildrenParticipating { get; set; }
    }
}