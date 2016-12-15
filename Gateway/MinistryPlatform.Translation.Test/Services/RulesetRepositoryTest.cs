using MinistryPlatform.Translation.Models.Rules;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Rules;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class RulesetRepositoryTest
    {
        private RulesetRepository _fixture;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IMinistryPlatformRestRepository> _mpRestRepository;
        private readonly string _mockToken = "iRule";

        [SetUp]
        public void SetUpTests()
        {
            _apiUserRepository = new Mock<IApiUserRepository>();
            _mpRestRepository = new Mock<IMinistryPlatformRestRepository>();
            _fixture = new RulesetRepository(_apiUserRepository.Object, _mpRestRepository.Object);
            _apiUserRepository.Setup(m => m.GetToken()).Returns(_mockToken);
            _mpRestRepository.Setup(m => m.UsingAuthenticationToken(_mockToken)).Returns(_mpRestRepository.Object);
        }

        [TearDown]
        public void TearItDown()
        {
            _apiUserRepository.VerifyAll();
            _mpRestRepository.VerifyAll();
        }

        [Test]
        public void ShouldGetRuleset()
        {
            var ruleSetId = 1;

            var mockRuleSet = new MPRuleSet {Name = "MockRuleset"};

            _mpRestRepository.Setup(m => m.Get<MPRuleSet>(ruleSetId, null)).Returns(mockRuleSet);

            var result = _fixture.GetRulesetFromMP(ruleSetId);

            Assert.AreEqual("MockRuleset", result.Name);
        }

        [Test]
        public void ShouldDoStuff()
        {
            
        }

    }
}
