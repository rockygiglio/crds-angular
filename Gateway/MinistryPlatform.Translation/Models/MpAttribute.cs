namespace MinistryPlatform.Translation.Models
{
    public class MpAttribute
    {
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public bool? Selected { get; set; }

        public int AttributeTypeId { get; set; }
        public string AttributeTypeName { get; set; }
        public bool PreventMultipleSelection { get; set; }
        public int SortOrder { get; set; }
    }
}