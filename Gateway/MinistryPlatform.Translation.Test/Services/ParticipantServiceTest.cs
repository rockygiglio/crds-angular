using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ParticipantServiceTest
    {
        private ParticipantRepository _fixture;
        private Mock<IMinistryPlatformService> _mpServiceMock;
        private Mock<IAuthenticationRepository> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        [SetUp]
        public void SetUp()
        {
            _mpServiceMock = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationRepository>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _configWrapper.Setup(m => m.GetConfigIntValue("Participants")).Returns(355);
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new AuthToken
            {
                AccessToken = "ABC",
                ExpiresIn = 123
            });

            _fixture = new ParticipantRepository(_mpServiceMock.Object, _authService.Object, _configWrapper.Object);
        }

        [Test]
        public void GetParticipantByParticipantId()
        {
            const int contactId = 99999;

            const string viewKey = "ParticipantByContactId";
            var searchString = contactId.ToString() + ",";
            DateTime backToTheFuture = new DateTime(2015, 10, 21);
            var mockDictionaryList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Contact_ID", 99999},
                    {"Participant_ID", 100},
                    {"Email_Address", "email-address"},
                    {"Nickname", "nick-name"},
                    {"Display_Name", "display-name"},
                    {"__Age", 99},
                    {"Approved_Small_Group_Leader", true },
                    {"Attendance_Start_Date", backToTheFuture}
                }
            };

            _mpServiceMock.Setup(m => m.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, "", 0)).Returns(mockDictionaryList);

            var participant = _fixture.GetParticipant(contactId);

            _mpServiceMock.VerifyAll();

            Assert.IsNotNull(participant);
            Assert.AreEqual("nick-name", participant.PreferredName);
            Assert.AreEqual("email-address", participant.EmailAddress);
            Assert.AreEqual(100, participant.ParticipantId);
            Assert.IsTrue(participant.ApprovedSmallGroupLeader);
            Assert.AreEqual(backToTheFuture, participant.AttendanceStart);
        }

        [Test]
        public void TestGetParticipantRecord()
        {
            const string token = "tok123";

            const string viewKey = "MyParticipantRecords";
            var mockDictionaryList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Contact_ID", 99999},
                    {"dp_RecordID", 100},
                    {"Email_Address", "email-address"},
                    {"Nickname", "nick-name"},
                    {"Display_Name", "display-name"},
                    {"Age", 99},
                    {"Approved_Small_Group_Leader", true }
                }
            };

            _mpServiceMock.Setup(m => m.GetRecordsDict(viewKey, token, string.Empty, string.Empty)).Returns(mockDictionaryList);

            var participant = _fixture.GetParticipantRecord(token);

            _mpServiceMock.VerifyAll();

            Assert.IsNotNull(participant);
            Assert.AreEqual("nick-name", participant.PreferredName);
            Assert.AreEqual("email-address", participant.EmailAddress);
            Assert.AreEqual(100, participant.ParticipantId);
            Assert.IsTrue(participant.ApprovedSmallGroupLeader);
        }

        [Test]

        public void TestCreateParticipantRecord()
        {
            const int contactId = 9999;
            var date = DateTime.Now; 

            var mockDictionary = new Dictionary<string, object>
            {
                    {"Participant_Type_Default_ID", 2},
                    {"Participant_Start_Date", date},
                    {"Contact ID", 99999},
            };

            _mpServiceMock.Setup(
                mocked => mocked.CreateRecord(355, It.IsAny<Dictionary<string,object>>(), "ABC", false))
                .Returns(123);

            var participant = _fixture.CreateParticipantRecord(contactId);
            Assert.AreEqual(123, participant);           
        }

    }

}