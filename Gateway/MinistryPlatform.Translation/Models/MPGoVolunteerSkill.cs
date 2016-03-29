namespace MinistryPlatform.Translation.Models
{
    public class MPGoVolunteerSkill
    {
        public MPGoVolunteerSkill(int id, string label, string name)
        {
            this.GoVolunteerSkillId = id;
            this.AttributeName = name;
            this.Label = label;
        }

        public int GoVolunteerSkillId { get; set; }        
        public string Label { get; set; }
        public string AttributeName { get; set; }
    }
}
