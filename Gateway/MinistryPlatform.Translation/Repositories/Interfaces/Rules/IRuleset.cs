using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Rules;

namespace MinistryPlatform.Translation.Repositories.Interfaces.Rules
{
    public interface IRuleset
    {
        MPRuleSet GetRulesetFromMP(int ruleSetId);
        List<IRule> GetRulesInRuleset(int ruleSetId);
        MPRuleSetResult AllRulesPass(List<IRule> rules, Dictionary<string, object> testData);
    }
}