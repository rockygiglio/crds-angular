using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class GoSkillsServiceTest
    {
        private GoSkillsService _fixture;
        private Mock<IApiUserService> _apiUserService;
        private Mock<ISkillsService> _skillsService;
        private Mock<crds_angular.Services.Interfaces.IObjectAttributeService> _objectAttributeService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IContactService> _contactService;

        [SetUp]
        public void Setup()
        {
            _apiUserService = new Mock<IApiUserService>();
            _skillsService = new Mock<ISkillsService>();
            _objectAttributeService = new Mock<crds_angular.Services.Interfaces.IObjectAttributeService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _contactService = new Mock<IContactService>();
            _fixture = new GoSkillsService(_apiUserService.Object, _skillsService.Object, _objectAttributeService.Object, _contactService.Object, _configurationWrapper.Object);
            
        }

        [Test]
        public void ShouldRetrieveGoSkills()
        {

            Prop.ForAll<string>( token =>
            {
                var skills = TestHelpers.MPSkills();
                _apiUserService.Setup(m => m.GetToken()).Returns(token);
                _skillsService.Setup(m => m.GetGoVolunteerSkills(token)).Returns(skills);
                var returned = _fixture.RetrieveGoSkills(string.Empty);
                Assert.IsInstanceOf<List<GoSkills>>(returned);
                Assert.AreEqual(skills.Count, returned.Count);
                _skillsService.VerifyAll();
            }).QuickCheckThrowOnFailure();           
        }
       
    }
}
