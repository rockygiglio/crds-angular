using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class GoEquipment
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        public List<GoEquipment> ToGoEquipment(List<MinistryPlatform.Models.Attribute> attributes)
        {
            var sorted = attributes.OrderBy(o => o.SortOrder).ThenBy(o => o.Name);
            return sorted.Select(attribute => new GoEquipment
            {
                Id = attribute.AttributeId,
                Label = attribute.Name
            }).ToList();
        }
    }
}