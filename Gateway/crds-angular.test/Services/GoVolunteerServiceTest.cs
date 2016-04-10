using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using FsCheck;
using log4net;
using MinistryPlatform.Models;
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
            Prop.ForAll<string, int>((token, sendMailResult) =>
            {
                const int templateId = 123456789;
                const int fromContactId = 0987;                
                var fromContact = TestHelpers.MyContact(fromContactId);
                var registration = TestHelpers.RegistrationNoSpouse();
                var contactFromRegistration = TestHelpers.ContactFromRegistrant(registration.Self);
                var communication = TestHelpers.Communication(fromContact, contactFromRegistration, templateId);

                _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailTemplate")).Returns(templateId);
                _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailFromContactId")).Returns(fromContactId);
                _contactService.Setup(m => m.GetContactById(fromContactId)).Returns(fromContact);
                _commnuicationService.Setup(m => m.GetTemplateAsCommunication(templateId,
                                                                              fromContactId,
                                                                              fromContact.Email_Address,
                                                                              fromContact.Contact_ID,
                                                                              fromContact.Email_Address,
                                                                              registration.Self.ContactId,
                                                                              registration.Self.EmailAddress,
                                                                              It.IsAny<Dictionary<string, object>>())).Returns(communication);
                _commnuicationService.Setup(m => m.SendMessage(communication, false)).Returns(sendMailResult);
                var success = _fixture.SendMail(registration);
                _commnuicationService.Verify();
                if (sendMailResult > 0)
                {
                    Assert.IsTrue(success);
                }
                else
                {
                    Assert.IsFalse(success);
                }                
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldSendEmailToVolunteerAndSpouse()
        {
            Prop.ForAll<string, int>((token, sendMailResult) =>
            {
                const int templateId = 123456789;
                const int spouseTemplateId = 98765432;

                const int fromContactId = 0987;
                var fromContact = TestHelpers.MyContact(fromContactId);
                var registration = TestHelpers.RegistrationWithSpouse();
                var contactFromRegistration = TestHelpers.ContactFromRegistrant(registration.Self);
                var spouseFromRegistration = TestHelpers.ContactFromRegistrant(registration.Spouse);

                var communication = TestHelpers.Communication(fromContact, contactFromRegistration, templateId);
                var spouseCommunication = TestHelpers.Communication(fromContact, spouseFromRegistration, spouseTemplateId);

                _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailTemplate")).Returns(templateId);
                

                _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailFromContactId")).Returns(fromContactId);
                _contactService.Setup(m => m.GetContactById(fromContactId)).Returns(fromContact);

                _commnuicationService.Setup(m => m.GetTemplateAsCommunication(templateId,
                                                                              fromContactId,
                                                                              fromContact.Email_Address,
                                                                              fromContact.Contact_ID,
                                                                              fromContact.Email_Address,
                                                                              registration.Self.ContactId,
                                                                              registration.Self.EmailAddress,
                                                                              It.IsAny<Dictionary<string, object>>())).Returns(communication);

                if (sendMailResult > 0)
                {
                    _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailSpouseTemplate")).Returns(spouseTemplateId);
                    _commnuicationService.Setup(m => m.GetTemplateAsCommunication(spouseTemplateId,
                                                                                  fromContactId,
                                                                                  fromContact.Email_Address,
                                                                                  fromContact.Contact_ID,
                                                                                  fromContact.Email_Address,
                                                                                  registration.Spouse.ContactId,
                                                                                  registration.Spouse.EmailAddress,
                                                                                  It.IsAny<Dictionary<string, object>>())).Returns(spouseCommunication);
                    _commnuicationService.Setup(m => m.SendMessage(spouseCommunication, false)).Returns(1);
                }
                _commnuicationService.Setup(m => m.SendMessage(communication, false)).Returns(sendMailResult);
               
                
                var success = _fixture.SendMail(registration);                
                _configurationWrapper.VerifyAll();
                _commnuicationService.VerifyAll();
                if (sendMailResult > 0)
                {
                    Assert.IsTrue(success);
                }
                else
                {
                    Assert.IsFalse(success);
                }
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldSetupMergeDataCorrectly()
        {
            var registration = TestHelpers.RegistrationNoSpouse();
            var mergeData = _fixture.SetupMergeData(registration);

            var tableAttrs = new Dictionary<string, string>()
            {
                {"width", "100%"},
                {"border", "1"},
                {"cellspacing", "0"},
                {"cellpadding", "5"}
            };

            var cellAttrs = new Dictionary<string, string>()
            {
                {"align", "center"}
            };

            var headers = new List<string>()
            {
                "Question",
                "Answer"
            }.Select(el => new HtmlElement("th", el)).ToList();

            var htmlTable = new HtmlElement("table", tableAttrs)
                .Append(new HtmlElement("tr", headers))
                .Append(new HtmlElement("tr"))
                .Append(new HtmlElement("td", cellAttrs, "Organization"))
                .Append(new HtmlElement("td", cellAttrs, registration.OrganizationId.ToString()))
                .Append(new HtmlElement("tr"))
                .Append(new HtmlElement("td", cellAttrs, "Spouse Participating"))
                .Append(new HtmlElement("td", cellAttrs, registration.SpouseParticipation.ToString()));

            if (registration.SpouseParticipation)
            {
                htmlTable.Append(new HtmlElement("tr"))
                    .Append(new HtmlElement("td", cellAttrs, "Spouse Name"))
                    .Append(new HtmlElement("td", cellAttrs, registration.Spouse.FirstName + " " + registration.Spouse.LastName))
                    ;
            }
               


            Assert.AreEqual(registration.RegistrationId, mergeData["registrationId"]);

        }

    }
}
