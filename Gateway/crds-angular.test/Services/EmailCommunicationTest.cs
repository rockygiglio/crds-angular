﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using crds_angular.Services;
using Moq;
using MinistryPlatform.Translation.Repositories.Interfaces;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using crds_angular.Models.Crossroads;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models;
using Newtonsoft.Json;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class EmailCommunicationTest
    {
        private EmailCommunication fixture;

        private Mock<ICommunicationRepository> _communicationService;
        private Mock<IPersonService> _personService;
        private Mock<IContactRepository> _contactService;
        private Mock<IConfigurationWrapper> _configurationWrapper;

        [SetUp]
        public void Setup()
        {
            _communicationService = new Mock<ICommunicationRepository>();
            _personService = new Mock<IPersonService>();
            _contactService = new Mock<IContactRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _configurationWrapper.Setup(mock => mock.GetConfigIntValue("DefaultContactEmailId")).Returns(5);
            fixture = new EmailCommunication(_communicationService.Object, _personService.Object, _contactService.Object, _configurationWrapper.Object);
        }

        [Test]
        public void TestSendEmailWithContactId()
        {
            EmailCommunicationDTO emailData = new EmailCommunicationDTO
            {
                TemplateId = 264567,
                MergeData = new Dictionary<string, object>(),
                ToContactId = 10,
                StartDate = DateTime.Now
            };

            MpMessageTemplate template = new MpMessageTemplate()
            {
                Body = "body",
                Subject = "subject",
                FromContactId = 5,
                FromEmailAddress = "sender@test.com",
                ReplyToContactId = 5,
                ReplyToEmailAddress = "replyto@test.com."
            };

            MpContact expectedContact = new MpContact()
            {
                ContactId = emailData.ToContactId.Value,
                EmailAddress = "user@test.com"
            };

            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.ReplyToContactId)).Returns(template.ReplyToEmailAddress);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(emailData.ToContactId.Value)).Returns("user@test.com");
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.FromContactId)).Returns(template.FromEmailAddress);
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactIdByEmail(emailData.emailAddress)).Returns(0);

            var spiedComm = new MpCommunication();
            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false))
                .Callback<MpCommunication, bool>((comm, token) => spiedComm = comm);

            
            fixture.SendEmail(emailData, null);
            _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Once);
            Assert.AreEqual(JsonConvert.SerializeObject(expectedContact),JsonConvert.SerializeObject(spiedComm.ToContacts[0]));

        }

        [Test]
        public void TestSendEmailWithEmailAddressOfContact()
        {

            EmailCommunicationDTO emailData = new EmailCommunicationDTO {
                TemplateId = 264567,
                MergeData = new Dictionary<string, object>(),
                emailAddress = "user@test.com",
                StartDate = DateTime.Now
            };

            MpMessageTemplate template = new MpMessageTemplate()
            {
                Body = "body",
                Subject = "subject",
                FromContactId = 5,
                FromEmailAddress = "sender@test.com",
                ReplyToContactId = 5,
                ReplyToEmailAddress = "replyto@test.com."
            };

            MpContact expectedContact = new MpContact()
            {
                ContactId = 10,
                EmailAddress = emailData.emailAddress
            };

            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.ReplyToContactId)).Returns(template.ReplyToEmailAddress);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.FromContactId)).Returns(template.FromEmailAddress);
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactIdByEmail(emailData.emailAddress)).Returns(10);

            var spiedComm = new MpCommunication();
            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false))
                .Callback<MpCommunication, bool>((comm, token) => spiedComm = comm);
            
            fixture.SendEmail(emailData, null);
            _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Once);
            Assert.AreEqual(JsonConvert.SerializeObject(expectedContact), JsonConvert.SerializeObject(spiedComm.ToContacts[0]));

        }

        [Test]
        public void TestSendEmailWithEmailAddressOfNonContact()
        {
            EmailCommunicationDTO emailData = new EmailCommunicationDTO
            {
                TemplateId = 264567,
                MergeData = new Dictionary<string, object>(),
                emailAddress = "user@test.com",
                StartDate = DateTime.Now
            };

            MpMessageTemplate template = new MpMessageTemplate()
            {
                Body = "body",
                Subject = "subject",
                FromContactId = 5,
                FromEmailAddress = "sender@test.com",
                ReplyToContactId = 5,
                ReplyToEmailAddress = "replyto@test.com."
            };

            MpContact expectedContact = new MpContact()
            {
                ContactId = 5,
                EmailAddress = emailData.emailAddress
            };

            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.ReplyToContactId)).Returns(template.ReplyToEmailAddress);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.FromContactId)).Returns(template.FromEmailAddress);
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactIdByEmail(emailData.emailAddress)).Returns(0);

            var spiedComm = new MpCommunication();
            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false))
                .Callback<MpCommunication, bool>((comm, token) => spiedComm = comm);

            fixture.SendEmail(emailData, null);
            _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Once);
            Assert.AreEqual(JsonConvert.SerializeObject(expectedContact), JsonConvert.SerializeObject(spiedComm.ToContacts[0])); ;

        }
    }
}
