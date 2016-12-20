using System;
using System.Collections.Generic;
using crds_angular.Services;
using crds_angular.test.Helpers;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.Rules;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class CampRulesTest
    {
        private Mock<IEventRepository> _eventRepository;
        private Mock<IEventParticipantRepository> _eventParticipantRepository;
        private Mock<IProductRulesetsRepository> _productRulesetsRepository;
        private CampRules _fixture;

        [SetUp]
        public void Setup()
        {
            Factories.MpEvent();
            Factories.MPProductRuleSet();
            _eventParticipantRepository = new Mock<IEventParticipantRepository>();
            _eventRepository = new Mock<IEventRepository>();
            _productRulesetsRepository = new Mock<IProductRulesetsRepository>();
            _fixture = new CampRules(_eventRepository.Object, _eventParticipantRepository.Object, _productRulesetsRepository.Object);
        }

        public void Teardown()
        {
            _eventParticipantRepository.VerifyAll();
            _eventRepository.VerifyAll();
            _productRulesetsRepository.VerifyAll();
        }

        [Test]
        public void ShouldVerifyCampRules()
        {
            const int gender = 1;
            const int eventId = 987;
            const int productId = 777;

            const int currentParts = 12;
            const int currentPlusMe = 13;

            var mpEvent = FactoryGirl.NET.FactoryGirl.Build<MpEvent>((ev) => 
            {
                ev.EventId = eventId;
                ev.OnlineProductId = productId; 
            });

            var mpProductRuleSet = FactoryGirl.NET.FactoryGirl.Build<MPProductRuleSet>((p) => p.ProductId = productId);
            var productRuleSetList = new List<MPProductRuleSet> {mpProductRuleSet};
            var ruleData = new Dictionary<string, object>
            {
                {"GenderId", gender},
                {"registrantCount", currentPlusMe}
            };

            _eventParticipantRepository.Setup(m => m.GetEventParticipantCountByGender(eventId, gender)).Returns(currentParts);
            _eventRepository.Setup(m => m.GetEvent(eventId)).Returns(mpEvent);
            _productRulesetsRepository.Setup(m => m.GetProductRulesets(productId)).Returns(productRuleSetList);
            _productRulesetsRepository.Setup(m => m.VerifyRulesets(productRuleSetList, ruleData)).Returns(true);
            var res = _fixture.VerifyCampRules(eventId, gender);

            Assert.IsTrue(res);
        }

        [Test]
        public void ShouldPassRulesIfNotValidDates()
        {
            const int gender = 1;
            const int eventId = 987;
            const int productId = 777;

            const int currentParts = 12;
            const int currentPlusMe = 13;

            var mpEvent = FactoryGirl.NET.FactoryGirl.Build<MpEvent>((ev) =>
            {
                ev.EventId = eventId;
                ev.OnlineProductId = productId;
            });

            var mpProductRuleSet = FactoryGirl.NET.FactoryGirl.Build<MPProductRuleSet>((p) =>
            {
                p.ProductId = productId;
                p.StartDate = DateTime.Now.AddDays(30);
                p.EndDate = DateTime.Now.AddDays(40);
            });
            var productRuleSetList = new List<MPProductRuleSet> { mpProductRuleSet };
            var ruleData = new Dictionary<string, object>
            {
                {"GenderId", gender},
                {"registrantCount", currentPlusMe}
            };

            _eventParticipantRepository.Setup(m => m.GetEventParticipantCountByGender(eventId, gender)).Returns(currentParts);
            _eventRepository.Setup(m => m.GetEvent(eventId)).Returns(mpEvent);
            _productRulesetsRepository.Setup(m => m.GetProductRulesets(productId)).Returns(productRuleSetList);
            _productRulesetsRepository.Setup(m => m.VerifyRulesets(new List<MPProductRuleSet>(), ruleData)).Returns(true);
            var res = _fixture.VerifyCampRules(eventId, gender);
            Assert.IsTrue(res);
        }

    }
}