namespace MinistryPlatform.Translation.Models
{
    public class MpAttributeCategory
    {
        public int CategoryID { get; set; }
        public MpAttribute Attribute { get; set; }
        public string Description { get; set; }
        public string Example_Text { get; set; }
        public bool Requires_Active_Attribute { get; set; }
        public string Attribute_Category { get; set; }
    }
}