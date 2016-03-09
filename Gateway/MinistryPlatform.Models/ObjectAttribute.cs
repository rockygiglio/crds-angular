using System;

namespace MinistryPlatform.Models
{
    public class ObjectAttribute
    {
        public int ObjectAttributeId { get; set; }
        public int AttributeId { get; set; }
        public int AttributeTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public string AttributeTypeName { get; set; }
    }
}