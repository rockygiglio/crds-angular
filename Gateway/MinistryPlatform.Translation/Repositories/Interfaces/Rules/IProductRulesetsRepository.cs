using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Rules;

namespace MinistryPlatform.Translation.Repositories.Interfaces.Rules
{
    public interface IProductRulesetsRepository
    {
        List<MPProductRuleSet> GetProductRulesets(int productId);
        bool VerifyRulesets(List<MPProductRuleSet> productRulesets, Dictionary<string, object> testData);
    }
}