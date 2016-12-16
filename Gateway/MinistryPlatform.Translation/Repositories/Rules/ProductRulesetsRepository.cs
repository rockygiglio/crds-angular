using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories.Rules
{
    public class ProductRulesetsRepository : IProductRulesetsRepository
    {
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRestRepository;

        public ProductRulesetsRepository(IApiUserRepository apiUserRepository, IMinistryPlatformRestRepository ministryPlatformRestRepository)
        {
            _apiUserRepository = apiUserRepository;
            _ministryPlatformRestRepository = ministryPlatformRestRepository;
        }

        public List<MPProductRuleSet> GetProductRulesets(int productId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var searchString = $"Product_ID_Table.[Product_ID] = {productId}";
            const string columnList = "Product_ID_Table.[Product_ID],Ruleset_ID_Table.[Ruleset_ID],cr_Product_Ruleset.[Start_Date],cr_Product_Ruleset.[End_Date]";
            return _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Search<MPProductRuleSet>(searchString, columnList);
        }
    }
}