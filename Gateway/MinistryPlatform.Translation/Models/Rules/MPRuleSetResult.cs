using System.Collections.Generic;

namespace MinistryPlatform.Translation.Models.Rules
{
    public class MPRuleSetResult
    {
        public bool AllRulesPass { get; set; }
        public List<MPRuleResult> RuleResults { get; set; }

        public MPRuleSetResult()
        {
            RuleResults = new List<MPRuleResult>();
        }
    }
}
