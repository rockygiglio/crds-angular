using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models.Lookups;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    class LookupServiceTest
    {
        private LookupRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IAuthenticationRepository> _authenticationService;


        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authenticationService = new Mock<IAuthenticationRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _fixture = new LookupRepository(_authenticationService.Object, _configurationWrapper.Object, _ministryPlatformService.Object);

        }

        [Test]
        public void ShouldReturnWorkTeamList()
        {

            var wt = WorkTeams();

            string token = "ABC";

            _ministryPlatformService.Setup(m => m.GetLookupRecords(It.IsAny<int>(), token)).Returns(wt);
            var returnVal = _fixture.GetList<MpWorkTeams>(token);

            Assert.IsInstanceOf<IEnumerable<MpWorkTeams>>(returnVal);
            Assert.AreEqual(wt.Count, returnVal.Count());

        }

        [Test]
        public void ShouldReturnOtherOrgList()
        {

            var oo = OtherOrgs();

            _ministryPlatformService.Setup(m => m.GetLookupRecords(It.IsAny<int>(), It.IsAny<string>())).Returns(oo);
            var returnVal = _fixture.GetList<MpOtherOrganization>(It.IsAny<string>());

            Assert.IsInstanceOf<IEnumerable<MpOtherOrganization>>(returnVal);
            Assert.AreEqual(oo.Count, returnVal.Count());

        }

        [Test]
        public void ShouldReturnSites()
        {
            string _tokenValue = "ABC";
            var authenticateResults =
                new Dictionary<string, object>()
                {
                                {"token", _tokenValue},
                                {"exp", "123"}
                };
            _authenticationService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(authenticateResults);
            _fixture = new LookupRepository(_authenticationService.Object, _configurationWrapper.Object, _ministryPlatformService.Object);
            var sites = CrossroadsSites();
            _ministryPlatformService.Setup(m => m.GetLookupRecords(It.IsAny<int>(), It.IsAny<String>())).Returns(sites);
            var returnVal = _fixture.CrossroadsLocations();
            Assert.IsInstanceOf<List<Dictionary<string, object>>>(returnVal);
            Assert.AreEqual(sites.Count, returnVal.Count());

        }

        private List<Dictionary<string, object>> OtherOrgs()
        {
            return new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 15},
                    {"dp_RecordName", "namworkteam"}
                },
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 12},
                    {"dp_RecordName", "name or workteam"}
                }
            };
        }

        private List<Dictionary<string, object>> CrossroadsSites()
        {
            return new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 15},
                    {"dp_RecordName", "Anywhere"}
                },
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 7},
                    {"dp_RecordName", "Florence"}
                }
            };
        }

        private List<Dictionary<string, object>> WorkTeams()
        {
            return new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 15},
                    {"dp_RecordName", "namworkteam"}
                },
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 12},
                    {"dp_RecordName", "name or workteam"}
                }
            };
        }

    }
}
