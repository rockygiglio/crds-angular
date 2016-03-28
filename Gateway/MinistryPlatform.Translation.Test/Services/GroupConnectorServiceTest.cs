using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces.GoCincinnati;
using Moq;
using NUnit.Framework;


namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    class GroupConnectorServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private GroupConnectorService _fixture;
        private Mock<IConfigurationWrapper> _configuration;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configuration = new Mock<IConfigurationWrapper>();

            _configuration.Setup(mocked => mocked.GetConfigIntValue("GroupConnectorPageId")).Returns(467);
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });


            _fixture = new GroupConnectorService(_ministryPlatformService.Object, _authService.Object, _configuration.Object);

        }

        [Test]
        public void ShouldCreateGroupConnector()
        {
            const int registrationId = 9999;

            _ministryPlatformService.Setup(
              mocked => mocked.CreateRecord(467, It.IsAny<Dictionary<string, object>>(), "ABC", true))
              .Returns(123);

            var groupConnector = _fixture.CreateGroupConnector(registrationId);
            Assert.AreEqual(123, groupConnector);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(groupConnector);
            Assert.AreEqual(9999, registrationId);
        }

        [Test]
        public void ShouldCreateGroupConnectorRegistration()
        {
            const int registrationId = 9999;
            const int groupConnectorId = 8888;
            _configuration.Setup(mocked => mocked.GetConfigValue("GroupConnectorRegistrationPageId")).Returns("GroupConnectorRegistrationPageId");

            _ministryPlatformService.Setup(
              mocked => mocked.CreateSubRecord("GroupConnectorRegistrationPageId", groupConnectorId,It.IsAny<Dictionary<string, object>>(), "ABC", true))
              .Returns(123);

            var groupConnectorRegistration = _fixture.CreateGroupConnectorRegistration(groupConnectorId, registrationId);
            Assert.AreEqual(123, groupConnectorRegistration);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(groupConnectorRegistration);
            Assert.AreEqual(9999, registrationId);
        }

    }
}
