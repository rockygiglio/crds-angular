using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Profile
{
    public class ObjectAttributeTypeDTO
    {
        [JsonProperty(PropertyName = "attributeTypeId")]
        public int AttributeTypeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<ObjectAttributeDTO> Attributes { get; set; }


        public ObjectAttributeTypeDTO()
        {
            Attributes = new List<ObjectAttributeDTO>();
        }
    }
}