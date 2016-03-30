using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Services;
using FsCheck;
using MinistryPlatform.Translation.Models.Lookups;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class GatewayLookupServiceTest
    {
        private GatewayLookupService _fixture;
        private Mock<IApiUserService> _apiUserService;
        private Mock<ILookupService> _lookupService;

        [SetUp]
        public void Setup()
        {
            _apiUserService = new Mock<IApiUserService>();
            _lookupService = new Mock<ILookupService>();

            _fixture = new GatewayLookupService(_lookupService.Object, _apiUserService.Object);
        }

        [Test]
        public void ShouldGetOtherOrgs()
        {
            Prop.ForAll<string>((token) =>
            {
                var otherOrgs = MPOtherOrgs();
                const string tk = "somerandomstring";
                if (token == null)
                {
                    _apiUserService.Setup(m => m.GetToken()).Returns(tk);
                    token = tk;
                }
                _lookupService.Setup(m => m.GetList<MPOtherOrganization>(token)).Returns(otherOrgs);
                var result = _fixture.GetOtherOrgs(token);
                Assert.IsInstanceOf<List<OtherOrganization>>(result);
                Assert.AreEqual(otherOrgs.Count(), result.Count);
            }).QuickCheckThrowOnFailure();
        }

        private IEnumerable<MPOtherOrganization> MPOtherOrgs()
        {
            return new List<MPOtherOrganization>()
            {
                new MPOtherOrganization(12, "sadfsadf"),
                new MPOtherOrganization(345, "asdfadfdfasdf"),
                new MPOtherOrganization(90909, "asfdgahlskjdfsadf")   
            };
        } 
    }
}
