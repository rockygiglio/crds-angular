using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Attributes")]
    public class MpAttribute
    {
        [JsonProperty(PropertyName = "Attribute_ID")]
        public int AttributeId { get; set; }
        [JsonProperty(PropertyName = "Attribute_Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Attribute_Category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "Attribute_Category_ID")]
        public int? CategoryId { get; set; }
        [JsonProperty(PropertyName = "Attribute_Category_Description")]
        public string CategoryDescription { get; set; }
        public bool? Selected { get; set; }
        public int AttributeTypeId { get; set; }
        public string AttributeTypeName { get; set; }
        [JsonProperty(PropertyName = "Prevent_Multiple_Selection")]
        public bool PreventMultipleSelection { get; set; }
        [JsonProperty(PropertyName = "Sort_Order")]
        public int SortOrder { get; set; }
    }
}