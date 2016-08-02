using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class GroupToolRepositoryTest
    {
        private GroupToolRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRestRepository;
        private Mock<IApiUserRepository> _apiUserRepository;

        private const int InvitationPageID = 55;
        private const int GroupInquiriesSubPageId = 304;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);
            _ministryPlatformRestRepository = new Mock<IMinistryPlatformRestRepository>(MockBehavior.Strict);
            _apiUserRepository = new Mock<IApiUserRepository>(MockBehavior.Strict);

            var config = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            var auth = new Mock<IAuthenticationRepository>(MockBehavior.Strict);

            config.Setup(mocked => mocked.GetConfigIntValue("InvitationPageID")).Returns(InvitationPageID);
            config.Setup(mocked => mocked.GetConfigIntValue("GroupInquiresSubPage")).Returns(GroupInquiriesSubPageId);

            config.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("api_user");
            config.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("password");

            auth.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });

            _fixture = new GroupToolRepository(_ministryPlatformService.Object, config.Object, auth.Object, _ministryPlatformRestRepository.Object, _apiUserRepository.Object);
        }

        [Test]
        public void GetInquiriesTest()
        {
            var dto = new List<MpInquiry>();

            dto.Add(new MpInquiry
            {
                InquiryId = 178,
                GroupId = 199846,
                EmailAddress = "test@jk.com",
                PhoneNumber = "444-111-2111",
                FirstName = "Joe",
                LastName = "Smith",
                RequestDate = new DateTime(2004, 3, 12),
                Placed = true,
                ContactId = 1,
            });

            const string token = "adamantium";
            const int groupId = 199846;

            var returned = new List<Dictionary<string, object>>();
                
            returned.Add(
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 178},
                    {"Email", "test@jk.com"},
                    {"Phone", "444-111-2111"},
                    {"First_Name", "Joe"},
                    {"Last_Name", "Smith"},
                    {"Inquiry_Date", "3/12/2004"},
                    {"Placed", "true"},
                    {"Contact_ID", 1}
                }
            );

            _ministryPlatformService.Setup(mocked => mocked.GetSubPageRecords(GroupInquiriesSubPageId, groupId, It.IsAny<string>())).Returns(returned);

            var result = _fixture.GetInquiries(groupId);
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(dto[0].InquiryId, result[0].InquiryId);
            Assert.AreEqual(dto[0].GroupId, result[0].GroupId);
            Assert.AreEqual(dto[0].EmailAddress, result[0].EmailAddress);
            Assert.AreEqual(dto[0].PhoneNumber, result[0].PhoneNumber);
            Assert.AreEqual(dto[0].FirstName, result[0].FirstName);
            Assert.AreEqual(dto[0].LastName, result[0].LastName);
            Assert.AreEqual(dto[0].RequestDate, result[0].RequestDate);
            Assert.AreEqual(dto[0].RequestDate, result[0].RequestDate);
            Assert.AreEqual(dto[0].ContactId, result[0].ContactId);
        }
    }
}
