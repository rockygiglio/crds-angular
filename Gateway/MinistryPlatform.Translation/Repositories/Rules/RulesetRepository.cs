using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.Rules;

namespace MinistryPlatform.Translation.Repositories.Rules
{
    public class RulesetRepository : IRuleset
    {
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRestRepository;

        public RulesetRepository(IApiUserRepository apiUserRepository, IMinistryPlatformRestRepository ministryPlatformRestRepository)
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
            rules.AddRange(GetRegistrantRules(ruleSetId));

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
            return genderRules.Select(g => new GenderRule(g.StartDate, g.EndDate, g.AllowedGenderId));
        }

        private IEnumerable<RegistrationRule> GetRegistrantRules(int ruleSetId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var searchString = $"Ruleset_ID = {ruleSetId}";
            var registrantRules = _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Search<MPRegistrationRule>(searchString);
            return registrantRules.Select(g => new RegistrationRule(g.StartDate, g.EndDate, g.MinimumRegistrants, g.MaximumRegistrants));
        }
    }
}
