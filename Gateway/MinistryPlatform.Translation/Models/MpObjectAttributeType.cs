using System.Collections.Generic;

namespace MinistryPlatform.Translation.Models
{
    public class MpObjectAttributeType
    {
        public int AttributeTypeId { get; set; }
        public string Name { get; set; }
        public List<MpObjectAttribute> Attributes { get; set; }

        public MpObjectAttributeType()
        {
            Attributes = new List<MpObjectAttribute>();
        }
    }
}