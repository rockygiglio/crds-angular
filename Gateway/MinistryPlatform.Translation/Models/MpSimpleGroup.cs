using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Groups")]
    public class MpSimpleGroup
    {
        [JsonProperty(PropertyName = "Group_ID")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "Group_Name")]
        public string GroupName { get; set; }
    }
}
