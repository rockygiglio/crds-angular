using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces.GoCincinnati;
using Moq;
using NUnit.Framework;
using IAttributeService = crds_angular.Services.Interfaces.IAttributeService;
using IGroupConnectorService = MinistryPlatform.Translation.Services.Interfaces.GoCincinnati.IGroupConnectorService;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class GoVolunteerServiceTest
    {
        private readonly GoVolunteerService _fixture;

        private readonly Mock<IAttributeService> _attributeService;
        private readonly Mock<IConfigurationWrapper> _configurationWrapper;
        private readonly Mock<MinistryPlatform.Translation.Services.Interfaces.IContactRelationshipService> _contactRelationshipService;
        private readonly Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService> _contactService;
        private readonly Mock<IGroupConnectorService> _groupConnectorService;
        private readonly Mock<MinistryPlatform.Translation.Services.Interfaces.IParticipantService> _participantService;
        private readonly Mock<MinistryPlatform.Translation.Services.Interfaces.IProjectTypeService> _projectTypeService;
        private readonly Mock<IRegistrationService> _registrationService;
        private readonly Mock<IGoSkillsService> _skillsService;
        private readonly Mock<ICommunicationService> _commnuicationService;


        public GoVolunteerServiceTest()
        {
            _attributeService = new Mock<IAttributeService>();
            _commnuicationService = new Mock<ICommunicationService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _contactRelationshipService = new Mock<IContactRelationshipService>();
            _contactService = new Mock<IContactService>();
            _groupConnectorService = new Mock<IGroupConnectorService>();
            _participantService = new Mock<IParticipantService>();
            _projectTypeService = new Mock<IProjectTypeService>();
            _registrationService = new Mock<IRegistrationService>();
            _skillsService = new Mock<IGoSkillsService>();
            _fixture = new GoVolunteerService(_participantService.Object, 
                _registrationService.Object, 
                _contactService.Object, 
                _groupConnectorService.Object, 
                _configurationWrapper.Object, 
                _contactRelationshipService.Object, 
                _projectTypeService.Object, 
                _attributeService.Object, 
                _skillsService.Object,
                _commnuicationService.Object);
        }

        [Test]
        public void ShouldSendEmailOnlyToVolunteer()
        {
            Prop.ForAll<string, int>((token,registrationContactId) =>
            {
                const int templateId = 123456789;
                const int fromContactId = 0987;                
                var fromContact = TestHelpers.MyContact(fromContactId);
                var registration = TestHelpers.RegistrationNoSpouse();
                

                _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailTemplate")).Returns(templateId);
                _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailFromContactId")).Returns(fromContactId);
                _contactService.Setup(m => m.GetContactById(fromContactId)).Returns(fromContact);
                _commnuicationService.Setup(m => m.GetTemplateAsCommunication(templateId,
                                                                              fromContactId,
                                                                              fromContact.Email_Address,
                                                                              fromContact.Contact_ID,
                                                                              fromContact.Email_Address,
                                                                              registrationContactId,
                                                                              registration.Self.EmailAddress,
                                                                              It.IsAny<Dictionary<string, object>>())).Returns();

                var success = _fixture.SendMail(registrationContactId, registration);
                Assert.IsTrue(success);
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldSendEmailToVolunteerAndSpouse()
        {
            
        }

    }
}
