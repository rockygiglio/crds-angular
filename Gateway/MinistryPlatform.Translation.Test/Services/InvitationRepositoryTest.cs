using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class InvitationRepositoryTest
    {
        private InvitationRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;

        private const int InvitationPageId = 123;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            var config = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            var auth = new Mock<IAuthenticationRepository>(MockBehavior.Strict);

            config.Setup(mocked => mocked.GetConfigIntValue("InvitationPageID")).Returns(InvitationPageId);
            config.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("api_user");
            config.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("password");

            auth.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });

            _fixture = new InvitationRepository(_ministryPlatformService.Object, config.Object, auth.Object);
        }

        [Test]
        public void TestCreateInvitation()
        {
            var dto = new MpInvitation
            {
                EmailAddress = "wolverine@x-men.org",
                GroupRoleId = 11,
                RecipientName = "James Howlett",
                InvitationType = 33,
                RequestDate = DateTime.Now,
                SourceId = 44
            };

            const string token = "adamantium";

            const int invitationId = 987;
            const string invitationGuid = "1020304050";

            var created = new Dictionary<string, object>
            {
                {"Invitation_GUID", invitationGuid}
            };

            _ministryPlatformService.Setup(
                mocked =>
                    mocked.CreateRecord(InvitationPageId,
                                        It.Is<Dictionary<string, object>>(
                                            d =>
                                                d.ToInt("Source_ID", false) == dto.SourceId && d.ToString("Email_Address").Equals(dto.EmailAddress) &&
                                                d.ToString("Recipient_Name").Equals(dto.RecipientName) && d.ToInt("Group_Role_ID", false) == dto.GroupRoleId &&
                                                d.ToInt("Invitation_Type_ID", false) == dto.InvitationType /*&& d["Invitation_Date"].Equals(dto.RequestDate)*/),
                                        It.IsAny<string>(),
                                        true)).Returns(invitationId);
            _ministryPlatformService.Setup(mocked => mocked.GetRecordDict(InvitationPageId, invitationId, It.IsAny<string>(), false)).Returns(created);

            var result = _fixture.CreateInvitation(dto);
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreSame(dto, result);
            Assert.AreEqual(invitationId, result.InvitationId);
            Assert.AreEqual(invitationGuid, result.InvitationGuid);
        }
    }
}
