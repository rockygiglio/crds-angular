using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Core;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ChildcareRepositoryTest
    {
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IApiUserRepository> _apiUserRepository;
        private IChildcareRepository _fixture;

        [SetUp]
        public void Setup()
        {
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _apiUserRepository = new Mock<IApiUserRepository>();   
            _fixture = new ChildcareRepository(_configurationWrapper.Object, _ministryPlatformRest.Object,_apiUserRepository.Object);
        }

        [Test]
        public void childShouldBeRsvpd()
        {
            var token = "some silly string";
            var contactId = 12234;
            var groupId = 989423;

            var parms = new Dictionary<string,object>()
            {
                {"@ContactID", contactId},
                {"@EventGroupID", groupId }
            };

            var retVal = new List<List<MPRspvd>>()
            {
                new List<MPRspvd>()
                {
                    new MPRspvd() {Rsvpd = true}
                }
            };

            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.GetFromStoredProc<MPRspvd>("api_crds_childrsvpd", parms)).Returns(retVal);
            var rsvpd = _fixture.IsChildRsvpd(contactId, groupId, token);

            _ministryPlatformRest.VerifyAll();
            Assert.AreEqual(true, rsvpd);
        }

        [Test]
        public void childShouldNotBeRsvpd()
        {
            var token = "some silly string";
            var contactId = 12234;
            var groupId = 989423;

            var parms = new Dictionary<string, object>()
            {
                {"@ContactID", contactId},
                {"@EventGroupID", groupId }
            };

            var retVal = new List<List<MPRspvd>>()
            {
                new List<MPRspvd>()
            };

            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.GetFromStoredProc<MPRspvd>("api_crds_childrsvpd", parms)).Returns(retVal);
            var rsvpd = _fixture.IsChildRsvpd(contactId, groupId, token);

            _ministryPlatformRest.VerifyAll();
            Assert.AreEqual(false, rsvpd);
        }
    }
}
