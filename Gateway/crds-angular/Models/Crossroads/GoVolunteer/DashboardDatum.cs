using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class DashboardDatum
    {
        public static readonly string[] Headers =
        {
            "Registrant Name", "Email Address", "Phone Number",
            "Adults Participating", "Children Participating"
        };

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

        public bool Equals(DashboardDatum other)
        {
            return this.RegistrantName == other.RegistrantName
                   && this.EmailAddress == other.EmailAddress
                   && this.PhoneNumber == other.PhoneNumber
                   && this.AdultsParticipating == other.AdultsParticipating
                   && this.ChildrenParticipating == other.ChildrenParticipating;
        }
    }
}