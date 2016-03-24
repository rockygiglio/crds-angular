using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models;
using Moq;
using NUnit.Framework;
using Rhino.Mocks;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class OrganizationServiceTest
    {
        private OrganizationService _fixture;
        private Mock<MPInterfaces.IOrganizationService> _organizationService;
        private Mock<MPInterfaces.IApiUserService> _apiUserService;

        private const string apiUserToken = "somerandomstring";
        private const int CONTACTID = 123456789;
        private readonly DateTime STARTDATE = new DateTime(2008, 02, 21);
        private const string NAME = "Ingage";
        private const int ORGID = 987654321;

        [SetUp]
        public void SetUp()
        {
            _organizationService = new Mock<MPInterfaces.IOrganizationService>();
            _apiUserService = new Mock<MPInterfaces.IApiUserService>();
            _fixture = new OrganizationService(_organizationService.Object, _apiUserService.Object);

            _apiUserService.Setup(m => m.GetToken()).Returns(apiUserToken);
        }

        [Test]
        public void ShouldGetOrganizationByName()
        {
            var mp = UniquelyNamedOrganization();
            _organizationService.Setup(m => m.GetOrganization(NAME, apiUserToken))
                .Returns(mp);
            var org = _fixture.GetOrganizationByName(NAME);
            Assert.IsInstanceOf<Organization>(org);
            Assert.AreEqual(mp.OrganizationId,org.OrganizationId);
            Assert.AreEqual(mp.ContactId, org.ContactId);
            Assert.AreEqual(mp.EndDate, org.EndDate);
            Assert.AreEqual(mp.StartDate, org.StartDate);
            Assert.AreEqual(mp.Name, org.Name);
            Assert.AreEqual(mp.OpenSignup, org.OpenSignup);
        }

        [Test]
        public void ShouldHandleNoOrganization()
        {
            _organizationService.Setup(m => m.GetOrganization("Ingage", apiUserToken))
                .Returns((MPOrganization) null);
            var org = _fixture.GetOrganizationByName("Ingage");
            Assert.IsNull(org);
        }

        [Test]
        public void ShouldGetAllOrganizations()
        {
            var mporgs = Organizations();
            _organizationService.Setup(m => m.GetOrganizations(apiUserToken))
                .Returns(mporgs);
            var orgs = _fixture.GetOrganizations();
            Assert.AreEqual(orgs.Count, mporgs.Count);      
        }

        [Test]
        public void ShouldGetEmptyListOfOrganizations()
        {
            var mporgs = new List<MPOrganization>();
            _organizationService.Setup(m => m.GetOrganizations(apiUserToken))
               .Returns(mporgs);
            var orgs = _fixture.GetOrganizations();
            Assert.IsEmpty(orgs);
        }

        private List<MPOrganization> Organizations()
        {
            return new List<MPOrganization>()
            {
                UniquelyNamedOrganization("Crossroads"),
                UniquelyNamedOrganization(),
                UniquelyNamedOrganization("Whateva")
            };
        } 

        private MPOrganization UniquelyNamedOrganization(string name = NAME, bool valid = true)
        {
            if (valid)
            {
                return new MPOrganization()
                {
                    ContactId = CONTACTID,
                    EndDate = new DateTime(),
                    StartDate = STARTDATE,
                    Name = name,
                    OpenSignup = false,
                    OrganizationId = ORGID
                };

            }
            return new MPOrganization()
            {
                ContactId = CONTACTID,
                EndDate = new DateTime(),
                StartDate = STARTDATE,
                Name = name,
                OpenSignup = false
            };
        }

      
    }
}
