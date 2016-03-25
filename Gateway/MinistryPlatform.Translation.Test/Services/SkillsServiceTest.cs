using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using FsCheck;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class SkillsServiceTest
    {
        private SkillsService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IAuthenticationService> _authenticationService;

        private const int CONFIG = 123;

        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _authenticationService = new Mock<IAuthenticationService>();

            _fixture = new SkillsService(_authenticationService.Object, _configurationWrapper.Object, _ministryPlatformService.Object);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerSkills")).Returns(CONFIG);
        }

        [Test]
        public void ShouldGetGoVolunteerSkills()
        {
           Prop.ForAll<List<Dictionary<string,object>>>((list) =>
           {
               var token = "random";
               //var returnVal = new MPGoVolunteerSkill(id, label, name);
               _ministryPlatformService.Setup(m => m.GetRecordsDict(CONFIG, token, "", "")).Returns(list);
               var newList = _fixture.GetGoVolunteerSkills(token);

               Assert.AreEqual(list.Count, newList.Count);
               _ministryPlatformService.VerifyAll();
           }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldNotThrowErrorIfNoLabel()
        {
            
        }        
         
    }
}