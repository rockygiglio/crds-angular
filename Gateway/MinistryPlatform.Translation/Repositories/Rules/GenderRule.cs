using System;
using System.Collections.Generic;
using System.IO;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces.Rules;

namespace MinistryPlatform.Translation.Repositories.Rules
{
    public class GenderRule : IRule
    {
        private readonly MPGenderRule _genderRule;

        public GenderRule(DateTime startDate, DateTime? endDate, int allowedGenderId)
        {
            _genderRule = new MPGenderRule
            {
                StartDate = startDate,
                EndDate = endDate,
                AllowedGenderId = allowedGenderId
            };
        }

        public bool RuleIsActive()
        {
            var now = DateTime.Now;
            return now >= _genderRule.StartDate && (_genderRule.EndDate == null || now < _genderRule.EndDate);
        }

        public MPRuleResult RulePasses(Dictionary<string, object> data)
        {
            var result = new MPRuleResult();

            if (!RuleIsActive())
            {
                result = new MPRuleResult
                {
                    Message = "Rule Not Active",
                    RulePassed = true
                };
                return result;
            }

            try
            {
                var genderId = data.ToInt("GenderId", true);
                result.RulePassed = (genderId == _genderRule.AllowedGenderId);
                result.Message = result.RulePassed ? "Rule Passed" : $"Gender must be {_genderRule.AllowedGenderId}";
                return result;
            }
            catch (Exception e)
            {
                throw new InvalidDataException("Invalid data provided to Gender Rule.", e);
            }
        }
    }
}