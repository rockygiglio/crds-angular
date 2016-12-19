using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.Rules;

namespace crds_angular.Services
{
    public class CampRules : ICampRules
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IProductRulesetsRepository _productRulesetsRepository;

        public CampRules(IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository, IProductRulesetsRepository productRulesetsRepository)
        {
            _eventRepository = eventRepository;
            _eventParticipantRepository = eventParticipantRepository;
            _productRulesetsRepository = productRulesetsRepository;
        }


        public bool VerifyCampRules(int eventId, int gender)
        {
            var currentCampers = _eventParticipantRepository.GetEventParticipantCountByGender(eventId, gender);
            var currentCampersPlusMe = currentCampers + 1;
            var ruleData = new Dictionary<string, object>
            {
                {"GenderId", gender},
                {"registrantCount", currentCampersPlusMe}
            };
            var mpEvent = _eventRepository.GetEvent(eventId);
            var productRulesets = _productRulesetsRepository.GetProductRulesets(mpEvent.OnlineProductId ?? 0);
            productRulesets = productRulesets.Where(p => (p.StartDate <= DateTime.Now && (p.EndDate == null || p.EndDate >= DateTime.Now))).ToList();
            var rulesPass = _productRulesetsRepository.VerifyRulesets(productRulesets, ruleData);
            return rulesPass;            
        }
    }
}