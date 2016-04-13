using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class ChildrenAttending
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    public class ChildrenOptions
    {
        [JsonProperty(PropertyName = "attributeId")]
        public int AttributeId { get; set; }

        [JsonProperty(PropertyName = "singularLabel")]
        public string SingularLabel { get; set; }

        [JsonProperty(PropertyName = "pluralLabel")]
        public string PluralLabel { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }
}