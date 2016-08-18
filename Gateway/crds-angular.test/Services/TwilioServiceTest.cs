using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
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
            _fixture = new TwilioService(_configurationWrapper.Object);
        }

        [Test]
        public void Test()
        {
            throw new NotImplementedException();
        }
    }
}
