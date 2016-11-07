using crds_angular.Models.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.Attribute;

namespace crds_angular.Models.Crossroads
{
    public class AttributeCategoryDTO
    {
        [JsonProperty(PropertyName = "categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty(PropertyName = "attribute")]
        public ObjectAttributeDTO Attribute { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "exampleText")]
        public string ExampleText { get; set; }

        [JsonProperty(PropertyName = "requiresActiveAttribute")]
        public bool RequiresActiveAttribute { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string AttributeCategory { get; set; }
    }
}