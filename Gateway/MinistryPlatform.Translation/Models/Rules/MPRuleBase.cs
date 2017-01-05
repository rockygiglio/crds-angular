using System;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Rules
{
    public class MPRuleBase
    {
        [JsonProperty(PropertyName = "Rule_Start_Date")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "Rule_End_Date")]
        public DateTime? EndDate { get; set; }
    }
}
