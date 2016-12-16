using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces.Rules;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IProductRulesetsRepository
    {
        List<MPProductRuleSet> GetProductRulesets(int productId);        
    }
}