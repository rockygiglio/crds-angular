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

        public GoSkills(int id, int attributeId, string label, string name, bool selected)
        {
            SkillId = id;
            Label = label;
            Name = name;
            Checked = selected;
            AttributeId = attributeId;
        }

        [JsonProperty(PropertyName = "attributeId")]
        public int AttributeId { get; set; }

        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        [JsonProperty(PropertyName = "currentlySet")]
        public bool CurrentlySet { get; set; }

        public List<GoSkills> ToGoSkills(List<MpGoVolunteerSkill> skills)
        {
            return skills.Select(skill => new GoSkills(skill.GoVolunteerSkillId,skill.AttributeId, skill.Label, skill.AttributeName, skill.Checked)).ToList();
        }
    }
}