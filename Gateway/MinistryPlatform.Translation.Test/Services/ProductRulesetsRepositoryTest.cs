using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Rules;
using MinistryPlatform.Translation.Test.Helpers;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class ProductRulesetsRepositoryTest
    {

        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRestRepository;
        private IProductRulesetsRepository _fixture;

        [SetUp]
        public void Setup()
        {
            Factories.MPProductRuleset();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _ministryPlatformRestRepository = new Mock<IMinistryPlatformRestRepository>();
            _fixture = new ProductRulesetsRepository(_apiUserRepository.Object, _ministryPlatformRestRepository.Object);
        }

        public void Teardown()
        {
            _apiUserRepository.VerifyAll();
            _ministryPlatformRestRepository.VerifyAll();
        }

        [Test]
        public void ShouldGetRulesetsForProduct()
        {
            const string token = "O\'Doyle Rules!";
            const int productId = 351;
            var rulesets = new List<MPProductRuleSet>
            {
               FactoryGirl.NET.FactoryGirl.Build<MPProductRuleSet>( r => { r.ProductId = r.ProductId + 2; r.RulesetId = r.RulesetId + 2; }),
               FactoryGirl.NET.FactoryGirl.Build<MPProductRuleSet>( r => { r.ProductId = r.ProductId + 3; r.RulesetId = r.RulesetId + 3; }),
               FactoryGirl.NET.FactoryGirl.Build<MPProductRuleSet>( r => { r.ProductId = r.ProductId + 4; r.RulesetId = r.RulesetId + 4; })
            };

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _ministryPlatformRestRepository.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRestRepository.Object);
            _ministryPlatformRestRepository.Setup(m => m.Search<MPProductRuleSet>(It.IsAny<string>(), It.IsAny<string>(), null as string, false)).Returns(rulesets);

            var res = _fixture.GetProductRulesets(productId);
            Assert.AreEqual(3, res.Count);            
        }

        [Test]
        public void ShouldGetEmptyListForNoRulesets()
        {
            const string token = "O\'Doyle Rules!";
            const int productId = 351;
            var rulesets = new List<MPProductRuleSet>();

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _ministryPlatformRestRepository.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRestRepository.Object);
            _ministryPlatformRestRepository.Setup(m => m.Search<MPProductRuleSet>(It.IsAny<string>(), It.IsAny<string>(), null as string, false)).Returns(rulesets);

            var res = _fixture.GetProductRulesets(productId);
            Assert.AreEqual(0, res.Count);
        }

        [Test]
        public void ShouldRethrowExceptionWhenExceptionIsThrown()
        {
            const string token = "O\'Doyle Rules!";
            const int productId = 351;
            var rulesets = new List<MPProductRuleSet>();

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _ministryPlatformRestRepository.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRestRepository.Object);
            _ministryPlatformRestRepository.Setup(m => m.Search<MPProductRuleSet>(It.IsAny<string>(), It.IsAny<string>(), null as string, false)).Throws(new Exception());

            Assert.Throws<Exception>(() =>
            {
                _fixture.GetProductRulesets(productId);
            });

        }

    }
}
