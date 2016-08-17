using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class TripRepositoryTest
    {
        private readonly Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;
        private readonly Mock<IConfigurationWrapper> _configurationWrapper;
        private readonly ITripRepository _fixture;

        private const string token = "some garbage token";
        private const string storedProc = "api_crds_Add_As_TripParticipant";

        public TripRepositoryTest()
        {
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _fixture = new TripRepository(_ministryPlatformRest.Object, _configurationWrapper.Object);
        }

        [SetUp]
        public void Setup()
        {
            _configurationWrapper.Setup(m => m.GetConfigValue("TripParticipantStoredProc")).Returns(storedProc);
        }

        [Test]
        public void ShouldAddAsTripParticipant()
        {
            var values = new Dictionary<string, object>
            {
                {"@PledgeCampaignID", 12345},
                {"@ContactID", 433334}
            };
            _ministryPlatformRest.Setup(m => m.PostStoredProc(storedProc, values)).Returns(200);
            var returnVal = _fixture.AddAsTripParticipant(433334, 12345, token);

            Assert.AreEqual(true, returnVal);
            _ministryPlatformRest.VerifyAll();
        }

        [Test]
        public void ShouldNotAddAsParticipant()
        {
            var values = new Dictionary<string, object>
            {
                {"@PledgeCampaignID", 12345},
                {"@ContactID", 433334}
            };
            _ministryPlatformRest.Setup(m => m.PostStoredProc(storedProc, values)).Returns(400);
            var returnVal = _fixture.AddAsTripParticipant(433334, 12345, token);

            Assert.AreEqual(false, returnVal);
            _ministryPlatformRest.VerifyAll();
        }

    }
}
