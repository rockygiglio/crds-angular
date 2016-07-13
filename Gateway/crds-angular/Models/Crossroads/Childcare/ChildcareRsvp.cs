using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareRsvp
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "childName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "eligible")]
        public bool ChildEligible { get; set; }

        [JsonProperty(PropertyName = "rsvpness")]
        public bool ChildHasRsvp { get; set; }
    }
}