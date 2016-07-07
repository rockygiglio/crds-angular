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
using Participant = MinistryPlatform.Translation.Models.Participant;

namespace crds_angular.test.Services
{
    public class GroupToolServiceTest
    {
        private GroupToolService groupToolService;
        private Mock<MPServices.IAuthenticationRepository> authenticationService;
        private Mock<MPServices.IGroupToolRepository> groupToolRepository;
        private Mock<MPServices.ICommunicationRepository> communicationService;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            communicationService = new Mock<MPServices.ICommunicationRepository>();
            groupToolRepository = new Mock<MPServices.IGroupToolRepository>();
            groupToolService = new GroupToolService(groupToolRepository.Object);

        }


        [Test]
        public void CanGetInvitationsForGroups()
        {
            groupToolRepository.Setup(m => m.GetInvitations(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(getMpInvations());
            var sourceId = 1;
            var invitationTypeId = 1;
            var token = "dude";

            var invitations =  groupToolService.GetInvitations(sourceId, invitationTypeId, token);

            Assert.AreEqual(4, invitations.Count);


        }

        private List<MpInvitation> getMpInvations()
        {
            var invitations = new List<MpInvitation>();
            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "dudley@doright.com",
                    GroupRoleId = 16, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Dudley Doright",
                    RequestDate = new DateTime(2016, 7, 6)
                }

                );

            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "jker@gmail.com",
                    GroupRoleId = 16, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Joe",
                    RequestDate = new DateTime(2016, 7, 5)
                }

            );

            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "doubleDown@joker.com",
                    GroupRoleId = 16, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Joe from Chicago",
                    RequestDate = new DateTime(2016, 7, 4)
                }

            );

            invitations.Add(
                new MpInvitation
                {
                    EmailAddress = "ratso@rizzo.com",
                    GroupRoleId = 22, // 16 = Group member
                    InvitationType = 1, // 1 = Group invitation type
                    RecipientName = "Ratso Rizzo",
                    RequestDate = new DateTime(2016, 7, 3)
                }

            );


            return invitations;
        }
    }
}
