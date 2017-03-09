using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    class GroupConnectorServiceTest
    {
        private Mock<IGroupConnectorRepository> _groupRepository;
        private Mock<IConfigurationWrapper> _config;
        private IGroupConnectorService _fixture;

        [SetUp]
        public void Setup()
        {
            _groupRepository = new Mock<IGroupConnectorRepository>();
            _config = new Mock<IConfigurationWrapper>();
            _fixture = new GroupConnectorService(_groupRepository.Object, _config.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _groupRepository.VerifyAll();
            _config.VerifyAll();
        }

        [Test]
        public void ShouldGetOpenOrgGroupConnectorsAndFilterOutAnywhere()
        {
            const int initiativeId = 4;
            const int anywhereId = 4;
            var fakeGroupConnectors = new List<MpGroupConnector>
            {
                new MpGroupConnector
                {
                    Name = "groupConnector1",
                    PreferredLaunchSiteId = 2,
                    PreferredLaunchSite = "Oakley"
                },
                new MpGroupConnector
                {
                    Name = "groupConnector2",
                    PreferredLaunchSiteId = 4,
                    PreferredLaunchSite = "Anywhere"
                },
            };

            _groupRepository.Setup(m => m.GetGroupConnectorsForOpenOrganizations(initiativeId))
                .Returns(fakeGroupConnectors);
            _config.Setup(m => m.GetConfigIntValue("AnywhereCongregation")).Returns(anywhereId);

            var res = _fixture.GetGroupConnectorsForOpenOrganizations(initiativeId);           

            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("groupConnector1", res.First().Name);

        }

    }
}
