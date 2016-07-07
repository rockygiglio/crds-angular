using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.test.Models.Crossroads.Events;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using Moq;
using NUnit.Framework;
using MpAttribute = MinistryPlatform.Translation.Models.MpAttribute;
using MpEvent = MinistryPlatform.Translation.Models.MpEvent;
using GroupService = crds_angular.Services.GroupService;
using MPServices = MinistryPlatform.Translation.Repositories.Interfaces;
using IGroupRepository = MinistryPlatform.Translation.Repositories.Interfaces.IGroupRepository;
using InvitationType = crds_angular.Models.Crossroads.Invitation;
using Participant = MinistryPlatform.Translation.Models.Participant;

namespace crds_angular.test.Services
{
    public class InvitationServiceTest
    {
        private InvitationService invitationService;
        private Mock<MPServices.IAuthenticationRepository> authenticationService;
        private Mock<MPServices.IInvitationRepository> invitationRepository;
        private Mock<MPServices.ICommunicationRepository> communicationService;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            communicationService = new Mock<MPServices.ICommunicationRepository>();
            invitationRepository = new Mock<MPServices.IInvitationRepository>();
            invitationService = new InvitationService(invitationRepository.Object, communicationService.Object);

        }


        [Test]
        public void CanCreateInvitationsForGroups()
        {
            var token = "dude";
            var invitation = new Invitation()
            {
                EmailAddress = "dudley@doright.com",
                GroupRoleId = 16,
                InvitationType = crds_angular.Models.Crossroads.InvitationType.Group,
                RecipientName = "Dudley Doright",
                RequestDate = new DateTime(2016, 7, 6)
            };
            invitationRepository.Setup(m => m.CreateInvitation(It.IsAny<MpInvitation>(), token));

            var invitations = invitationService.CreateInvitation(invitation, token);
            invitationRepository.Verify(x => x.CreateInvitation(It.IsAny<MpInvitation>(), token), Times.Once);
        }
    }
}
