using System;
using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Attribute_Categories")]
    public class MpObjectAttribute
    {
        public int ObjectAttributeId { get; set; }

        [JsonProperty(PropertyName = "Attribute_ID")]
        public int AttributeId { get; set; }

        [JsonProperty(PropertyName = "Attribute_Type_ID")]
        public int AttributeTypeId { get; set; }

        [JsonProperty(PropertyName = "Attribute_Type")]
        public string AttributeTypeName { get; set; }

        [JsonProperty(PropertyName = "Attribute_Name")]
        public string Name { get; set; }
        

        [JsonProperty(PropertyName = "Attribute_Category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "Attribute_Category_ID")]
        public int CategoryId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public bool Selected { get; set; }
        public string Description { get; set; }
    }
}