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
    [TestFixture]
    public class GroupParticipantRepositoryTest
    {
        private GroupParticipantRepository _fixture;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRestService;
        private Mock<IGroupRepository> _groupRepository;
        //private readonly int _groupsParticipantsPageId = 298;
        //private readonly int _groupsParticipantsSubPage = 88;
        //private readonly int _groupsPageId = 322;
        //private readonly int _groupsSubGroupsPageId = 299;
        //private readonly int _myGroupParticipationPageId = 563;
        //private const string ApiToken = "ABC";
        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _ministryPlatformRestService = new Mock<IMinistryPlatformRestRepository>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _fixture = new GroupParticipantRepository(_configWrapper.Object, _ministryPlatformService.Object, _apiUserRepository.Object, _ministryPlatformRestService.Object, _groupRepository.Object);

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
        }

        [Test]
        public void TestGetAllGroupsLeadByParticipantNotFound()
        {
        }

        [Test]
        public void TestGetAllGroupsLeadByParticipantNoGroupsFound()
        {
        }

        [Test]
        public void TestGetAllGroupsLeadByParticipantNoGroupsLead()
        {
        }

        [Test]
        public void TestGetAllGroupsLeadByParticipantNoGroups()
        {
        }
    }
}