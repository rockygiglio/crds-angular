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
        
        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _authenticationService = new Mock<IAuthenticationService>();

            _fixture = new SkillsService(_authenticationService.Object, _configurationWrapper.Object, _ministryPlatformService.Object);
            
        }

        [Test]
        public void ShouldGetGoVolunteerSkills()
        {
           Prop.ForAll<int, string>((config, token) =>
           {
               //var returnVal = new MPGoVolunteerSkill(id, label, name);
               _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerSkills")).Returns(config);
               _ministryPlatformService.Setup(m => m.GetRecordsDict(config, token, "", "")).Returns(ListOfSkills());
               var newList = _fixture.GetGoVolunteerSkills(token);

               Assert.AreEqual(ListOfSkills().Count, newList.Count);
               _ministryPlatformService.VerifyAll();
           }).QuickCheckThrowOnFailure();
        }

        private List<Dictionary<string, object>> ListOfSkills()
        {
            return new List<Dictionary<string, object>>()
            {
                
                new Dictionary<string, object>()
                {
                    {"Go_Volunteer_Skills_ID", 2},
                    {"Label", "some label" },
                    {"Attribute_Name", "Skill Attribute" },
                    {"Attribute_ID", 2}
                },
                new Dictionary<string, object>()
                {
                    {"Go_Volunteer_Skills_ID", 3},
                    {"Label", "some label 2" },
                    {"Attribute_Name", "Skill Attribute two" },
                    {"Attribute_ID", 3}
                },
                new Dictionary<string, object>()
                {
                    {"Go_Volunteer_Skills_ID", 4},
                    {"Label", null },
                    {"Attribute_Name", "Skill Attribute four" },
                    {"Attribute_ID", 4}
                }
            };
        }
           
         
    }
}