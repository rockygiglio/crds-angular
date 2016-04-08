using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;
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
            _configuration.Setup(mocked => mocked.GetConfigValue("GroupConnectorRegistrationPageId")).Returns("GroupConnectorRegistrationPageId");
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });
            _fixture = new GroupConnectorService(_ministryPlatformService.Object, _authService.Object, _configuration.Object);
            
        }

        [Test]
        public void ShouldCreateGroupConnector()
        {
            const int registrationId = 9999;
            const int expGroupConnectorId = 8888;

            _ministryPlatformService.Setup(
              mocked => mocked.CreateRecord(467, It.IsAny<Dictionary<string, object>>(), "ABC", true))
              .Returns(expGroupConnectorId);
            _ministryPlatformService.Setup(
             mocked => mocked.CreateSubRecord("GroupConnectorRegistrationPageId", expGroupConnectorId, It.IsAny<Dictionary<string, object>>(), "ABC", true))
             .Returns(123);

            var groupConnector = _fixture.CreateGroupConnector(registrationId, false);
            Assert.AreEqual(expGroupConnectorId, groupConnector);

            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void ShouldThrowApplicationExceptionWhenGroupConnectorCreationFails()
        {
            Exception ex = new Exception("GroupConnecotr creation failed");
            _ministryPlatformService.Setup(
                mocked => mocked.CreateRecord(467, It.IsAny<Dictionary<string, object>>(), "ABC", true))
                .Throws(ex);

            try
            {
                _fixture.CreateGroupConnector(9999, false);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof(ApplicationException), e);
                Assert.AreSame(ex, e.InnerException);
            }
        }

        [Test]
        public void ShouldCreateGroupConnectorRegistration()
        {
            const int registrationId = 9999;
            const int groupConnectorId = 8888;

            _ministryPlatformService.Setup(
              mocked => mocked.CreateSubRecord("GroupConnectorRegistrationPageId", groupConnectorId, It.IsAny<Dictionary<string, object>>(), "ABC", true))
              .Returns(123);



            var groupConnectorRegistration = _fixture.CreateGroupConnectorRegistration(groupConnectorId, registrationId);
            Assert.AreEqual(123, groupConnectorRegistration);

            _ministryPlatformService.VerifyAll();
            
        }

        [Test]
        public void ShouldThrowApplicationExceptionWhenGroupConnectorRegistrationCreationFails()
        {
            Exception ex = new Exception("GroupConnecotrRegistration creation failed");
            _ministryPlatformService.Setup(
                mocked => mocked.CreateSubRecord("GroupConnectorRegistrationPageId",8888, It.IsAny<Dictionary<string, object>>(), "ABC", true))
                .Throws(ex);

            try
            {
                _fixture.CreateGroupConnectorRegistration(8888, 9999);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof(ApplicationException), e);
                Assert.AreSame(ex, e.InnerException);
            }
        }

    }
}
