using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using MinistryPlatform.Translation.Models;
using Moq;
using NUnit.Framework;
using MPServices = MinistryPlatform.Translation.Repositories.Interfaces;

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

        [Test]
        public void CanGetInquiriesForGroups()
        {
            var mpResults = new List<MpInquiry>();

            mpResults.Add(new MpInquiry
            {
                InquiryId = 178,
                GroupId = 199846,
                EmailAddress = "test@jk.com",
                PhoneNumber = "444-111-2111",
                FirstName = "Joe",
                LastName = "Smith",
                RequestDate = new DateTime(2004, 3, 12),
                Placed = true,
            });

            var dto = new List<Inquiry>();

            dto.Add(new Inquiry
                {
                    InquiryId = 178,
                    GroupId = 199846,
                    EmailAddress = "test@jk.com",
                    PhoneNumber = "444-111-2111",
                    FirstName = "Joe",
                    LastName = "Smith",
                    RequestDate = new DateTime(2004, 3, 12),
                    Placed = true,
                });

            var groupId = 1;
            var token = "dude";

            groupToolRepository.Setup(m => m.GetInquiries(It.IsAny<int>(), It.IsAny<string>())).Returns(mpResults);

            var inquiries = groupToolService.GetInquiries(groupId, token);
            
            Assert.AreEqual(1, inquiries.Count);
            Assert.AreEqual(dto[0].InquiryId, inquiries[0].InquiryId);
            Assert.AreEqual(dto[0].GroupId, inquiries[0].GroupId);
            Assert.AreEqual(dto[0].EmailAddress, inquiries[0].EmailAddress);
            Assert.AreEqual(dto[0].PhoneNumber, inquiries[0].PhoneNumber);
            Assert.AreEqual(dto[0].FirstName, inquiries[0].FirstName);
            Assert.AreEqual(dto[0].LastName, inquiries[0].LastName);
            Assert.AreEqual(dto[0].RequestDate, inquiries[0].RequestDate);
            Assert.AreEqual(dto[0].Placed, inquiries[0].Placed);


        }
    }
}
