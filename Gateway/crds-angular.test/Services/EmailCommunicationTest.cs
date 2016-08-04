using System;
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
using MinistryPlatform.Translation.Models;

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
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.ReplyToContactId)).Returns(template.ReplyToEmailAddress);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(emailData.ToContactId.Value)).Returns("user@test.com");
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.FromContactId)).Returns(template.FromEmailAddress);
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactIdByEmail(emailData.emailAddress)).Returns(0);

            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Verifiable();
            try
            {
                fixture.SendEmail(emailData, null);
                _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Once);
            }
            catch (Exception)
            {
                Assert.Fail("Unexpected exception was thrown");
            }
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
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.ReplyToContactId)).Returns(template.ReplyToEmailAddress);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.FromContactId)).Returns(template.FromEmailAddress);
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactIdByEmail(emailData.emailAddress)).Returns(10);

            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Verifiable();
            try
            {
                fixture.SendEmail(emailData, null);
                _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Once);
            }
            catch (Exception)
            {
                Assert.Fail("Unexpected exception was thrown");
            }
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
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.ReplyToContactId)).Returns(template.ReplyToEmailAddress);
            _communicationService.Setup(mocked => mocked.GetEmailFromContactId(template.FromContactId)).Returns(template.FromEmailAddress);
            _communicationService.Setup(mocked => mocked.GetTemplate(emailData.TemplateId)).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactIdByEmail(emailData.emailAddress)).Returns(0);

            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Verifiable();
            try
            {
                fixture.SendEmail(emailData, null);
                _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Once);
            }
            catch (Exception)
            {
                Assert.Fail("Unexpected exception was thrown");
            }
        }
    }
}
