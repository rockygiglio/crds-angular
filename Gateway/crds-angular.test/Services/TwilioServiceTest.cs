using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using log4net;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class TwilioServiceTest
    {
        private TwilioService _fixture;
        private Mock<IConfigurationWrapper> _configurationWrapper;

        [SetUp]
        public void Setup()
        {
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _configurationWrapper.Setup(mock => mock.GetConfigValue("TwilioAccountSid")).Returns("AC051651a7abfd7ec5209ad22273a24390");
            _configurationWrapper.Setup(mock => mock.GetConfigValue("TwilioAuthToken")).Returns("4995535b191fe6e7dd3578c10e4c7976");
            _configurationWrapper.Setup(mock => mock.GetConfigValue("TwilioFromPhoneNumber")).Returns("+15005550006");

            _fixture = new TwilioService(_configurationWrapper.Object);
        }

        [Test]
        public void TestTextSucceeds()
        {
            Mock<ILog> mockLogger = new Mock<ILog>();
            _fixture.SetLogger(mockLogger.Object);
            _fixture.SendTextMessage("+15005550006", "Hi");
            mockLogger.Verify(mock => mock.Error(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TestTextFails()
        {
            Mock<ILog> mockLogger = new Mock<ILog>();
            _fixture.SetLogger(mockLogger.Object);
            _fixture.SendTextMessage("+15005550001", "Hi");
            mockLogger.Verify(mock => mock.Error(It.IsAny<string>()), Times.Once);
        }
    }
}
