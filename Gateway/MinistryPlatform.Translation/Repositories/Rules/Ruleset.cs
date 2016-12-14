using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.Rules;

namespace MinistryPlatform.Translation.Repositories.Rules
{
    public class Ruleset
    {
        public IApiUserRepository _apiUserRepository;
        public IMinistryPlatformRestRepository _ministryPlatformRestRepository;

        public Ruleset(ApiUserRepository apiUserRepository, MinistryPlatformRestRepository ministryPlatformRestRepository)
        {
            _apiUserRepository = apiUserRepository;
            _ministryPlatformRestRepository = ministryPlatformRestRepository;
        }

        public MPRuleSet GetRulesetFromMP(int ruleSetId)
        {
            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Get<MPRuleSet>(ruleSetId);
        }

        public List<IRule> GetRulesInRuleset(int ruleSetId)
        {
            var rules = new List<IRule>();

            rules.AddRange(GetGenderRules(ruleSetId));


            return rules;
        }

        public bool AllRulesPass(List<IRule> rules, Dictionary<string, object> testData)
        {
            return rules.Where(rule => rule.RuleIsActive()).All(rule => rule.RulePasses(testData).RulePassed);
        }

        private IEnumerable<GenderRule> GetGenderRules(int ruleSetId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var searchString = $"Ruleset_ID = {ruleSetId}";
            var genderRules = _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Search<MPGenderRule>(searchString);
            var rules = genderRules.Select(g => new GenderRule(g.StartDate, g.EndDate, g.AllowedGenderId));
            return rules;
        }
    }
}
