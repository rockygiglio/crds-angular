namespace MinistryPlatform.Translation.Models
{
    public class MpGoVolunteerSkill
    {
        public MpGoVolunteerSkill(int id, string label, string name, int attributeId)
        {
            this.GoVolunteerSkillId = id;
            this.AttributeName = name;
            this.Label = label;
            this.AttributeId = attributeId;
        }

        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public int GoVolunteerSkillId { get; set; }
        public string Label { get; set; }
        public bool Checked { get; set; }
    }
}