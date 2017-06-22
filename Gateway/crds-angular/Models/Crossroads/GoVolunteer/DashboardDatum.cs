using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class DashboardDatum
    {
        public static readonly string[] Headers =
        {
            "Name", "Email Address", "Mobile Phone",
            "Adults", "Children"
        };

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string MobilePhone { get; set; }

        [JsonProperty(PropertyName = "adults")]
        public int Adults { get; set; }

        [JsonProperty(PropertyName = "children")]
        public int Children { get; set; }

        public bool Equals(DashboardDatum other)
        {
            return this.Name == other.Name
                   && this.EmailAddress == other.EmailAddress
                   && this.MobilePhone == other.MobilePhone
                   && this.Adults == other.Adults
                   && this.Children == other.Children;
        }
    }
}