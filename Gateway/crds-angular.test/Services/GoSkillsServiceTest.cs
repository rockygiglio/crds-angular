using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Models;
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
        private const int SKILLSATTRIBUTETYPEID = 123;

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

        [Test]
        public void ShouldGetSkillsToEndDate()
        {
            var skills = TestHelpers.ListOfGoSkills(3);
            var currentSkills = CurrentSkills(skills);

            _configurationWrapper.Setup(m => m.GetConfigIntValue("AttributeTypeIdSkills")).Returns(SKILLSATTRIBUTETYPEID);
            //var contact = TestHelpers.MyContact();
            //var token = "token";
            //var participantId = TestHelpers.RandomInt();
            //_contactService.Setup(m => m.GetContactByParticipantId(participantId)).Returns(contact);
            //_objectAttributeService.Setup(m => m.GetObjectAttributes(token, contact.Contact_ID, It.IsAny<ObjectAttributeConfiguration>())).Returns(currentSkills);
            var skillsToEndDate = _fixture.SkillsToEndDate(skills, currentSkills.MultiSelect[1].Attributes);
            Assert.IsInstanceOf<List<ObjectAttributeDTO>>(skillsToEndDate);
            Assert.AreEqual(2, skillsToEndDate.Count);            
        }

        [Test]
        public void ShouldGetSkillsToAdd()
        {
            var skills = TestHelpers.ListOfGoSkills(3);
            var currentSkills = CurrentSkills(skills);

            _configurationWrapper.Setup(m => m.GetConfigIntValue("AttributeTypeIdSkills")).Returns(SKILLSATTRIBUTETYPEID);
            var skillsToAdd = _fixture.SkillsToAdd(skills, currentSkills.MultiSelect[1].Attributes);
            Assert.AreEqual(2, skillsToAdd.Count);
        }

        public ObjectAllAttributesDTO CurrentSkills(List<GoSkills> skills)
        {
            return new ObjectAllAttributesDTO()
            {
                MultiSelect = new Dictionary<int, ObjectAttributeTypeDTO>()
                {
                    {1, new ObjectAttributeTypeDTO()
                        {
                            AttributeTypeId = SKILLSATTRIBUTETYPEID,
                            Attributes = new List<ObjectAttributeDTO>()
                            {
                                new ObjectAttributeDTO()
                                {
                                    AttributeId = skills.First().AttributeId,
                                    Name = skills.First().Name,
                                    EndDate = null
                                },
                                new ObjectAttributeDTO()
                                {
                                    AttributeId = 1290848210,
                                    Name = "not a duplicate",
                                    EndDate = null
                                },
                                new ObjectAttributeDTO()
                                {
                                    AttributeId = 323284821,
                                    Name = "not a duplicate",
                                    EndDate = null
                                },

                            }
                        }
                    },
                    {2, new ObjectAttributeTypeDTO() }
                }
            };
        }

    }
}
