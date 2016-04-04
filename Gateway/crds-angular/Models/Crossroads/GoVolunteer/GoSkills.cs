using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class GoSkills
    {
        public GoSkills()
        {
        }

        public GoSkills(int id, string label, string name, bool selected)
        {
            SkillId = id;
            Label = label;
            Name = name;
            Checked = selected;
        }

        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        public List<GoSkills> ToGoSkills(List<MpGoVolunteerSkill> skills)
        {
            return skills.Select(skill => new GoSkills(skill.GoVolunteerSkillId, skill.Label, skill.AttributeName, skill.Checked)).ToList();
        }
    }
}