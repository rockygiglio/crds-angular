using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareRsvpDto
    {
        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "childId")]
        public int ChildContactId { get; set; }

        [JsonProperty(PropertyName = "registered")]
        public bool Registered { get; set; }
    }
}