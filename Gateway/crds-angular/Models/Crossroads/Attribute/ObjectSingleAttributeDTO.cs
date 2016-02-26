using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class ObjectSingleAttributeDTO
    {
        [JsonProperty(PropertyName = "attribute")]
        public AttributeDTO Value { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}