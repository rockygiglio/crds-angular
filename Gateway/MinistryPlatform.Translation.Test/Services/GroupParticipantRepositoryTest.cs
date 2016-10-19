using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class GroupParticipantRepositoryTest
    {
        private GroupParticipantRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRestRepository;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IGroupRepository> _groupRepository;

        private const int GroupLeaderRole = 22;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);
            _ministryPlatformRestRepository = new Mock<IMinistryPlatformRestRepository>(MockBehavior.Strict);
            _apiUserRepository = new Mock<IApiUserRepository>(MockBehavior.Strict);
            _groupRepository = new Mock<IGroupRepository>(MockBehavior.Strict);

            var config = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            var auth = new Mock<IAuthenticationRepository>(MockBehavior.Strict);

            config.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("api_user");
            config.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("password");

            auth.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            config.Setup(mocked => mocked.GetConfigIntValue("GroupRoleLeader")).Returns(GroupLeaderRole);

            _fixture = new GroupParticipantRepository(config.Object,
                                                      _ministryPlatformService.Object,
                                                      _apiUserRepository.Object,
                                                      _ministryPlatformRestRepository.Object,
                                                      _groupRepository.Object);
        }

        [Test]
        public void GetAllGroupNamesLeadByParticipantShouldTransfomGroupParticipantIntoGroup()
        {
            var participantId = 3;
            var groupType = 9;

            List<MpGroupParticipant> groupParticipantReturn = new List<MpGroupParticipant>()
            {
                new MpGroupParticipant()
                {
                    ContactId = 30,
                    Email = "a@b.com",
                    GroupRoleId = 22,
                    GroupName = "group one",
                    NickName = "name one",
                    LastName = "name one",
                    GroupId = 1,
                    ParticipantId = participantId,
                    GroupParticipantId = 1,
                },
                new MpGroupParticipant()
                {
                    ContactId = 31,
                    Email = "a1@b.com",
                    GroupRoleId = 22,
                    GroupName = "group two",
                    NickName = "name two",
                    LastName = "name two",
                    GroupId = 2,
                    ParticipantId = participantId,
                    GroupParticipantId = 2
                },
                new MpGroupParticipant()
                {
                    ContactId = 32,
                    Email = "a3@b.com",
                    GroupRoleId = 22,
                    GroupName = "group three",
                    NickName = "name three",
                    LastName = "name three",
                    GroupId = 3,
                    ParticipantId = participantId,
                    GroupParticipantId = 3
                }
            };

            List<MpGroup> groups = new List<MpGroup>()
            {
                new MpGroup()
                {
                    GroupId = 1,
                    Name = "group one"
                },
                new MpGroup()
                {
                    GroupId = 2,
                    Name = "group two"
                },
                new MpGroup()
                {
                    GroupId = 3,
                    Name = "group three"
                }
            };

            _apiUserRepository.Setup(mocked => mocked.GetToken()).Returns("yeah!");
            _ministryPlatformRestRepository.Setup(mocked => mocked.UsingAuthenticationToken("yeah!")).Returns(_ministryPlatformRestRepository.Object);
            _ministryPlatformRestRepository.Setup(mocked => mocked.Search<MpGroupParticipant>(It.IsAny<string>(), It.IsAny<string>())).Returns(groupParticipantReturn);

            var result = _fixture.GetAllGroupNamesLeadByParticipant(participantId, groupType);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3, "Result count should be three groups");
            Assert.AreEqual(result[0].GroupId, groups[0].GroupId);
            Assert.AreEqual(result[0].Name, groups[0].Name);
            Assert.AreEqual(result[1].GroupId, groups[1].GroupId);
            Assert.AreEqual(result[1].Name, groups[1].Name);
            Assert.AreEqual(result[2].GroupId, groups[2].GroupId);
            Assert.AreEqual(result[2].Name, groups[2].Name);
        }
    }
}