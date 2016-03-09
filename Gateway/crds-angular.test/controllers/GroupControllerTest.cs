using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using Rhino.Mocks;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        private GroupController fixture;
        private Mock<crds_angular.Services.Interfaces.IGroupService> groupServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IParticipantService> participantServiceMock;
        private Mock<crds_angular.Services.Interfaces.IAddressService> addressServiceMock;        
        private Mock<IGroupSearchService> groupSearchServiceMock;
        private string authType;
        private string authToken;

        [SetUp]
        public void SetUp()
        {
            groupServiceMock = new Mock<crds_angular.Services.Interfaces.IGroupService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            participantServiceMock = new Mock<IParticipantService>();
            addressServiceMock = new Mock<crds_angular.Services.Interfaces.IAddressService>();            
            groupSearchServiceMock = new Mock<IGroupSearchService>();

            fixture = new GroupController(groupServiceMock.Object, authenticationServiceMock.Object, participantServiceMock.Object, addressServiceMock.Object, groupSearchServiceMock.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void testPostParticipantToCommunityGroupIsSuccessful()
        {
            const int groupId = 456;

            List<ParticipantSignup> particpantIdToAdd = new List<ParticipantSignup>
            {
                new ParticipantSignup()
                {
                    particpantId = 90210,
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                },
                new ParticipantSignup()
                {
                    particpantId = 41001,
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                }
            };

            List<Event> events = new List<Event>();
            Event e1 = new Event();
            e1.EventId = 101;
            Event e2 = new Event();
            e2.EventId = 202;
            events.Add(e1);
            events.Add(e2);

            var participantsAdded = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"123", "456"}
                },
                new Dictionary<string, object>
                {
                    {"abc", "def"}
                },
            };
            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd));

            IHttpActionResult result = fixture.Post(groupId, particpantIdToAdd);

            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (OkResult), result);
        }

        [Test]
        public void testPostParticipantToJourneyGroupIsSuccessful()
        {
            const int groupId = 456;

            List<ParticipantSignup> particpantIdToAdd = new List<ParticipantSignup>
            {
                new ParticipantSignup(){
                    particpantId = 90210,
                    childCareNeeded = false,
                    groupRoleId = 22,
                    SendConfirmationEmail = false,
                    capacityNeeded = 0
                },
                new ParticipantSignup()
                {
                    particpantId = 41001,
                    childCareNeeded = false,
                    groupRoleId = 16,
                    SendConfirmationEmail = false,
                    capacityNeeded = 1
                }
            };

            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd));

            IHttpActionResult result = fixture.Post(groupId, particpantIdToAdd);

            authenticationServiceMock.VerifyAll();
            groupServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
        }

        [Test]
        public void testPostParticipantToCommunityGroupFails()
        {
            Exception ex = new Exception();
            int groupId = 456;
            List<ParticipantSignup> particpantIdToAdd = new List<ParticipantSignup>
            {
                new ParticipantSignup()
                {
                    particpantId = 90210,
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                },
                new ParticipantSignup()
                {
                    particpantId = 41001,
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                }
            };

            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd)).Throws(ex);

            IHttpActionResult result = fixture.Post(groupId, particpantIdToAdd);
            authenticationServiceMock.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (BadRequestResult), result);
        }

        [Test]
        public void testGetGroupDetails()
        {
            int groupId = 333;
            int contactId = 777;

            Group g = new Group();
            g.GroupId = 333;
            g.GroupType = 8;
            g.GroupRole = "Member";
            g.Name = "Test Me";
            g.GroupId = 123456;
            g.TargetSize = 5;
            g.WaitList = true;
            g.WaitListGroupId = 888;
            g.RemainingCapacity = 10;

            Participant participant = new Participant();
            participant.ParticipantId = 90210;
            participantServiceMock.Setup(
                mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString()))
                .Returns(participant);

            authenticationServiceMock.Setup(mocked => mocked.GetContactId(fixture.Request.Headers.Authorization.ToString())).Returns(contactId);

            var relationRecord = new GroupSignupRelationships
            {
                RelationshipId = 1,
                RelationshipMinAge = 00,
                RelationshipMaxAge = 100
            };

            var groupDto = new GroupDTO
            {
            };

            groupServiceMock.Setup(mocked => mocked.getGroupDetails(groupId, contactId, participant, fixture.Request.Headers.Authorization.ToString())).Returns(groupDto);


            IHttpActionResult result = fixture.Get(groupId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (OkNegotiatedContentResult<GroupDTO>), result);
            groupServiceMock.VerifyAll();

            var groupDtoResponse = ((OkNegotiatedContentResult<GroupDTO>) result).Content;

            Assert.NotNull(result);
            Assert.AreSame(groupDto, groupDtoResponse);
        }

        [Test]
        public void testCallGroupServiceFailsUnauthorized()
        {
            fixture.Request.Headers.Authorization = null;
            IHttpActionResult result = fixture.Post(3, new List<ParticipantSignup>());
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (UnauthorizedResult), result);
            groupServiceMock.VerifyAll();
        }

        [Test]
        public void testAddParticipantToCommunityGroupWhenGroupFull()
        {
            int groupId = 333;
            Group g = new Group();
            g.GroupId = 333;
            g.GroupType = 8;
            g.GroupRole = "Member";
            g.Name = "Test Me";
            g.TargetSize = 5;
            g.WaitList = false;
            g.Full = true;

            var particpantIdToAdd = new List<ParticipantSignup>
            {
                new ParticipantSignup()
                {
                    particpantId = 90210,
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                },
                new ParticipantSignup()
                {
                    particpantId = 41001,
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                }
            };
            var groupFull = new GroupFullException(g);
            groupServiceMock.Setup(mocked => mocked.addParticipantsToGroup(groupId, particpantIdToAdd)).Throws(groupFull);

            try
            {
                fixture.Post(333, particpantIdToAdd);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof (HttpResponseException), e.GetType());
                var ex = (HttpResponseException) e;
                Assert.IsNotNull(ex.Response);
                Assert.AreEqual((HttpStatusCode) 422, ex.Response.StatusCode);
            }
        }

        [Test]
        public void GetGroupsByTypeForParticipantNoGroups()
        {
            const string token = "1234frd32";
            const int groupTypeId = 19;
          
            Participant participant = new Participant() 
            { 
                ParticipantId = 90210
            };

            var groups = new List<GroupDTO>();

            groupServiceMock.Setup(
                mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString()))
                .Returns(participant);

            groupServiceMock.Setup(mocked => mocked.GetGroupsByTypeForParticipant(token, participant.ParticipantId, groupTypeId)).Returns(groups);
          
            IHttpActionResult result = fixture.GetGroups(groupTypeId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<GroupDTO>>), result);
        }

        [Test]
        public void GetGroupsByTypeForParticipanGroupsFound()
        {
            const int groupTypeId = 19;

            Participant participant = new Participant()
            {
                ParticipantId = 90210
            };
            
            var groups = new List<GroupDTO>()
            {
                new GroupDTO()
                {
                    GroupName = "This will work"
                }
            };

           groupServiceMock.Setup(
               mocked => mocked.GetParticipantRecord(fixture.Request.Headers.Authorization.ToString()))
               .Returns(participant);

            groupServiceMock.Setup(mocked => mocked.GetGroupsByTypeForParticipant(fixture.Request.Headers.Authorization.ToString(), participant.ParticipantId, groupTypeId)).Returns(groups);

            IHttpActionResult result = fixture.GetGroups(groupTypeId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<GroupDTO>>), result);
      }

        [Test]
        public void PostGroupSuccessfully()
        {
            var group = new GroupDTO()
            {
                GroupName = "This will work"
            };

            var returnGroup = new GroupDTO()
            {
                GroupName = "This will work"
            };

            groupServiceMock.Setup(mocked => mocked.CreateGroup(group)).Returns(returnGroup);

            IHttpActionResult result = fixture.PostGroup(group);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (CreatedNegotiatedContentResult<GroupDTO>), result);
        }

        [Test]
        public void PostGroupFailed()
        {
            Exception ex = new Exception();

            var group = new GroupDTO()
            {
                GroupName = "This will work"
            };

            groupServiceMock.Setup(mocked => mocked.CreateGroup(group)).Throws(ex);

            IHttpActionResult result = fixture.PostGroup(group);
            groupServiceMock.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof (BadRequestResult), result);
        }


        [Test]
        public void PostGroupWithAddressSuccessfully()
        {
            var group = new GroupDTO()
            {
                GroupName = "This will work",
                Address = new AddressDTO()
                {
                    AddressLine1 = "123 Abc St.",
                    AddressLine2 = "Apt. 2",
                    City = "Cincinnati",
                    State = "OH",
                    County = "Hamilton",
                    ForeignCountry = "United States",
                    PostalCode = "45213"
                }
            };

            var returnGroup = new GroupDTO()
            {
                GroupName = "This will work"
            };
            
            groupServiceMock.Setup(mocked => mocked.CreateGroup(group)).Returns(returnGroup);            

            IHttpActionResult result = fixture.PostGroup(group);
            addressServiceMock.Verify(x=> x.FindOrCreateAddress(group.Address), Times.Once);
            groupServiceMock.VerifyAll();            
        }

        [Test]
        public void PostGroupWithNullAddressLine1_WillNotAddAddressToGroup_Successfully()
        {
            var group = new GroupDTO()
            {
                GroupName = "This will work",
                Address = new AddressDTO()
                {
                    AddressLine1 = null,
                    AddressLine2 = "Apt. 2",
                    City = "Cincinnati",
                    State = "OH",
                    County = "Hamilton",
                    ForeignCountry = "United States",
                    PostalCode = "45213"
                }
            };

            var returnGroup = new GroupDTO()
            {
                GroupName = "This will work"
            };

            groupServiceMock.Setup(mocked => mocked.CreateGroup(group)).Returns(returnGroup);

            IHttpActionResult result = fixture.PostGroup(group);
            addressServiceMock.Verify(x => x.FindOrCreateAddress(group.Address), Times.Never);
            groupServiceMock.VerifyAll();
        }

        [Test]
        public void PostGroupWithWithEmptyAddressLine1_WillNotAddAddressToGroup_Successfully()
        {
            var group = new GroupDTO()
            {
                GroupName = "This will work",
                Address = new AddressDTO()
                {
                    AddressLine1 = string.Empty,
                    AddressLine2 = "Apt. 2",
                    City = "Cincinnati",
                    State = "OH",
                    County = "Hamilton",
                    ForeignCountry = "United States",
                    PostalCode = "45213"
                }
            };

            var returnGroup = new GroupDTO()
            {
                GroupName = "This will work"
            };

            groupServiceMock.Setup(mocked => mocked.CreateGroup(group)).Returns(returnGroup);

            IHttpActionResult result = fixture.PostGroup(group);
            addressServiceMock.Verify(x => x.FindOrCreateAddress(group.Address), Times.Never);
            groupServiceMock.VerifyAll();
        }

        [Test]
        public void PostGroupWithoutAddressSuccessfully()
        {
            var group = new GroupDTO()
            {
                GroupName = "This will work",
                Address = null
            };

            var returnGroup = new GroupDTO()
            {
                GroupName = "This will work"
            };

            groupServiceMock.Setup(mocked => mocked.CreateGroup(group)).Returns(returnGroup);

            IHttpActionResult result = fixture.PostGroup(group);
            addressServiceMock.Verify(x => x.FindOrCreateAddress(group.Address), Times.Never);            
            groupServiceMock.VerifyAll();
        }

        [Test]
        public void PostInvitationWhenRequesterIsMemberOfGoup()
        {
           var communication = new EmailCommunicationDTO()
            {
                emailAddress  = "wonderwoman@marvel.com"
            };

           groupServiceMock.Setup(mocked => mocked.SendJourneyEmailInvite(communication, fixture.Request.Headers.Authorization.ToString()));

            IHttpActionResult result = fixture.PostInvitation(communication);
            groupServiceMock.Verify(x => x.SendJourneyEmailInvite(communication, fixture.Request.Headers.Authorization.ToString()), Times.Once);
            groupServiceMock.VerifyAll();
        }

        [Test]
        public void DoNotPostInvitationWhenRequesterIsNotMemberOfGoup()
        {
            var communication = new EmailCommunicationDTO()
            {
                emailAddress = "wonderwoman@marvel.com"
            };
            
            groupServiceMock.Setup(mocked => mocked.SendJourneyEmailInvite(communication, fixture.Request.Headers.Authorization.ToString())).Throws<InvalidOperationException>();

            IHttpActionResult result = fixture.PostInvitation(communication);
            groupServiceMock.VerifyAll();
            Assert.IsNotNull(result);

            Assert.IsInstanceOf(typeof (NotFoundResult), result);
        }

        [Test]
        public void testGetGroupParticipantsFound()
        {
            const string token = "1234frd32";
            const int groupId = 170656;

            var participant = new List<GroupParticipantDTO>();

            groupServiceMock.Setup(mocked => mocked.GetGroupParticipants(groupId)).Returns(participant);

            IHttpActionResult result = fixture.GetGroupParticipants(groupId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<GroupParticipantDTO>>), result);
        }

        [Test]
        public void testGetGroupParticipantsEmptyGroup()
        {
            const string token = "1234frd32";
            const int groupId = 1234;

            var participant = new List<GroupParticipantDTO>();

            groupServiceMock.Setup(mocked => mocked.GetGroupParticipants(groupId)).Returns(participant);

            IHttpActionResult result = fixture.GetGroupParticipants(groupId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<List<GroupParticipantDTO>>), result);
            Assert.AreEqual(0, participant.Count);
        }
    }
}
