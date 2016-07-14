using System;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.test.Services
{
    public class InvitationServiceTest
    {
        private InvitationService _fixture;
        private Mock<IInvitationRepository> _invitationRepository;
        private Mock<ICommunicationRepository> _communicationService;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IParticipantRepository> _participantRepository;

        private const int GroupInvitationType = 11;
        private const int GroupInvitationEmailTemplate = 22;
        private const int TripInvitationType = 33;
        private const int TripInvitationEmailTemplate = 44;
        private const int DefaultInvitationEmailTemplate = 55;
        private const int DomainId = 66;
        private const int GroupRoleLeader = 77;


        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _communicationService = new Mock<ICommunicationRepository>();
            _invitationRepository = new Mock<IInvitationRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _participantRepository = new Mock<IParticipantRepository>();
            var config = new Mock<IConfigurationWrapper>();

            config.Setup(mocked => mocked.GetConfigIntValue("GroupInvitationType")).Returns(GroupInvitationType);
            config.Setup(mocked => mocked.GetConfigIntValue("GroupInvitationEmailTemplate")).Returns(GroupInvitationEmailTemplate);
            config.Setup(mocked => mocked.GetConfigIntValue("TripInvitationType")).Returns(TripInvitationType);
            config.Setup(mocked => mocked.GetConfigIntValue("PrivateInviteTemplate")).Returns(TripInvitationEmailTemplate);
            config.Setup(mocked => mocked.GetConfigIntValue("DefaultInvitationEmailTemplate")).Returns(DefaultInvitationEmailTemplate);
            config.Setup(mocked => mocked.GetConfigIntValue("DomainId")).Returns(DomainId);
            config.Setup(mocked => mocked.GetConfigIntValue("GroupRoleLeader")).Returns(GroupRoleLeader);

            _fixture = new InvitationService(_invitationRepository.Object, _communicationService.Object, config.Object, _groupRepository.Object, _participantRepository.Object);
        }


        [Test]
        public void CanCreateInvitationsForGroups()
        {
            var token = "dude";
            var invitation = new Invitation()
            {
                EmailAddress = "dudley@doright.com",
                GroupRoleId = 16, // 16 = Co-leader invitation
                InvitationType = 1, // 1 = group invitation
                RecipientName = "Dudley Doright",
                RequestDate = new DateTime(2016, 7, 6)
            };
            _invitationRepository.Setup(m => m.CreateInvitation(It.IsAny<MpInvitation>(), token));

            var invitations = _fixture.CreateInvitation(invitation, token);
            _invitationRepository.Verify(x => x.CreateInvitation(It.IsAny<MpInvitation>(), token), Times.Once);
        }
    }
}
