using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Rules;

namespace MinistryPlatform.Translation.Repositories.Interfaces.Rules
{
    public interface IRule
    {
        bool RuleIsActive();
        MPRuleResult RulePasses(Dictionary<string, object> data);
    }
}
