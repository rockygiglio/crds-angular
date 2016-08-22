using System;
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
        private Mock<ILog> _mockLogger;

        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("TWILIO_ACCOUNT_SID",Environment.GetEnvironmentVariable("TWILIO_TEST_ACCOUNT_SID"));
            Environment.SetEnvironmentVariable("TWILIO_AUTH_TOKEN", Environment.GetEnvironmentVariable("TWILIO_TEST_AUTH_TOKEN"));
            Environment.SetEnvironmentVariable("TWILIO_FROM_PHONE_NUMBER", "+15005550006");
            _fixture = new TwilioService();
            _mockLogger = new Mock<ILog>();
            _fixture.SetLogger(_mockLogger.Object);
        }

        [Test]
        public void TestTextSucceeds()
        {
            _mockLogger.Setup(mock => mock.Error(It.IsAny<string>())).Verifiable();
            _fixture.SendTextMessage("+15005550006", "Hi");
            _mockLogger.Verify(mock => mock.Error(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TestTextFails()
        {
            _mockLogger.Setup(mock => mock.Error(It.IsAny<string>())).Verifiable();
            _fixture.SendTextMessage("+15005550001", "Hi");
            _mockLogger.Verify(mock => mock.Error(It.IsAny<string>()), Times.Once);
        }
    }
}
