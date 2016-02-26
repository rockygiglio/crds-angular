using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.test.Models.Crossroads.Events;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using Moq;
using NUnit.Framework;
using Event = MinistryPlatform.Models.Event;
using MPServices = MinistryPlatform.Translation.Services.Interfaces;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.test.Services
{
    public class GroupServiceTest 
    {
        private GroupService fixture;
        private Mock<MPServices.IAuthenticationService> authenticationService;
        private Mock<MPServices.IGroupService> groupService;
        private Mock<MPServices.IEventService> eventService;
        private Mock<MPServices.IContactRelationshipService> contactRelationshipService;     
        private Mock<IServeService> serveService;
        private Mock<IGroupService> _groupService;
        private Mock<MPServices.IParticipantService> participantService;
        private Mock<MPServices.ICommunicationService> _communicationService;
        private Mock<MPServices.IContactService> _contactService;
        private Mock<IConfigurationWrapper> config;

        private readonly List<ParticipantSignup> mockParticipantSignup = new List<ParticipantSignup>
        {
            new ParticipantSignup()
            {
                particpantId = 999,
                childCareNeeded = false,
                SendConfirmationEmail = true
            },
            new ParticipantSignup()
            {
                particpantId = 888,
                childCareNeeded = false,
                SendConfirmationEmail = true
            }
        };

        private const int GROUP_ROLE_DEFAULT_ID = 123;
        private const int JourneyGroupInvitationTemplateId = 123;         

        [SetUp]
        public void SetUp()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<EventProfile>());
            AutoMapperConfig.RegisterMappings();

            authenticationService = new Mock<MPServices.IAuthenticationService>();
            groupService = new Mock<IGroupService>();
            eventService = new Mock<MPServices.IEventService>(MockBehavior.Strict);
            contactRelationshipService = new Mock<MPServices.IContactRelationshipService>();           
            serveService = new Mock<IServeService>();
            participantService = new Mock<MPServices.IParticipantService>();
            _groupService = new Mock<IGroupService>();
            _communicationService = new Mock<MPServices.ICommunicationService>();
            _contactService = new Mock<MPServices.IContactService>();
            config = new Mock<IConfigurationWrapper>();

            config.Setup(mocked => mocked.GetConfigIntValue("Group_Role_Default_ID")).Returns(GROUP_ROLE_DEFAULT_ID);

            fixture = new GroupService(groupService.Object, config.Object, eventService.Object, contactRelationshipService.Object,
                        serveService.Object, participantService.Object, _communicationService.Object, _contactService.Object);
        }

        [Test]
        public void shouldThrowExceptionWhenAddingToCommunityGroupIfGetGroupDetailsFails()
        {
            Exception exception = new Exception("Oh no, Mr. Bill!");
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Throws(exception);

            try
            {
                fixture.addParticipantsToGroup(456, mockParticipantSignup);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (ApplicationException), e);
                Assert.AreSame(exception, e.InnerException);
            }

            groupService.VerifyAll();
        }

        [Test]
        public void shouldThrowCommunityGroupIsFullExceptionWhenGroupFullIndicatorIsSet()
        {
            var g = new Group
            {
                TargetSize = 3,
                Full = true,
                Participants = new List<GroupParticipant>
                {
                    new GroupParticipant()
                }
            };
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            try
            {
                fixture.addParticipantsToGroup(456, mockParticipantSignup);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (GroupFullException), e);
            }

            groupService.VerifyAll();
        }

        [Test]
        public void shouldThrowCommunityGroupIsFullExceptionWhenNotEnoughSpaceRemaining()
        {
            var g = new Group
            {
                TargetSize = 2,
                Full = false,
                Participants = new List<GroupParticipant>
                {
                    new GroupParticipant()
                }
            };
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            try
            {
                fixture.addParticipantsToGroup(456, mockParticipantSignup);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (GroupFullException), e);
            }

            groupService.VerifyAll();
        }

        [Test]
        public void shouldAddParticipantsToCommunityGroupAndEvents()
        {
            var g = new Group
            {
                TargetSize = 0,
                Full = false,
                Participants = new List<GroupParticipant>()
            };
            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            groupService.Setup(mocked => mocked.addParticipantToGroup(999, 456, GROUP_ROLE_DEFAULT_ID, false, It.IsAny<DateTime>(), null, false)).Returns(999456);
            groupService.Setup(mocked => mocked.addParticipantToGroup(888, 456, GROUP_ROLE_DEFAULT_ID, false, It.IsAny<DateTime>(), null, false)).Returns(888456);
            groupService.Setup(mocked => mocked.SendCommunityGroupConfirmationEmail(It.IsAny<int>(), 456, true, false));

            var events = new List<Event>
            {
                new Event {EventId = 777},
                new Event {EventId = 555},
                new Event {EventId = 444}
            };
            groupService.Setup(mocked => mocked.getAllEventsForGroup(456)).Returns(events);

            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(999, 777, 456, 999456)).Returns(999777);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(999, 555, 456, 999456)).Returns(999555);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(999, 444, 456, 999456)).Returns(999444);

            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(888, 777, 456, 888456)).Returns(888777);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(888, 555, 456, 888456)).Returns(888555);
            eventService.Setup(mocked => mocked.RegisterParticipantForEvent(888, 444, 456, 888456)).Returns(888444);

            fixture.addParticipantsToGroup(456, mockParticipantSignup);

            groupService.VerifyAll();
            eventService.VerifyAll();
        }

        [Test]
        public void testGetGroupDetails()
        {
            var g = new Group
            {
                TargetSize = 0,
                Full = true,
                Participants = new List<GroupParticipant>(),
                GroupType = 90210,
                WaitList = true,
                WaitListGroupId = 10101,
                GroupId = 98765
            };

            var eventList = new List<Event>()
            {
                EventHelpers.TranslationEvent()
            };

            groupService.Setup(mocked => mocked.getGroupDetails(456)).Returns(g);

            groupService.Setup(mocked => mocked.getAllEventsForGroup(456)).Returns(eventList);

            var relations = new List<GroupSignupRelationships>
            {
                new GroupSignupRelationships {RelationshipId = 111}
            };
            groupService.Setup(mocked => mocked.GetGroupSignupRelations(90210)).Returns(relations);

            var contactRelations = new List<ContactRelationship>
            {
                new ContactRelationship
                {
                    Contact_Id = 333,
                    Relationship_Id = 111,
                    Participant_Id = 222
                }
            };
            contactRelationshipService.Setup(mocked => mocked.GetMyCurrentRelationships(777, "auth token")).Returns(contactRelations);

            var participant = new Participant
            {
                ParticipantId = 555,
            };
            groupService.Setup(mocked => mocked.checkIfUserInGroup(555, It.IsAny<List<GroupParticipant>>())).Returns(false);
            groupService.Setup(mocked => mocked.checkIfUserInGroup(222, It.IsAny<List<GroupParticipant>>())).Returns(false);

            var response = fixture.getGroupDetails(456, 777, participant, "auth token");

            groupService.VerifyAll();
            contactRelationshipService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.GroupFullInd);
            Assert.AreEqual(g.GroupId, response.GroupId);
            Assert.AreEqual(2, response.SignUpFamilyMembers.Count);
            Assert.AreEqual(g.WaitListGroupId, response.WaitListGroupId);
            Assert.AreEqual(g.WaitList, response.WaitListInd);
        }

        [Test]
        public void GetGroupsByTypeForParticipant()
        {
            const string token = "1234frd32";
            const int participantId = 54;
            const int groupTypeId = 19;

            var groups = new List<Group>()
            {
                new Group
                {
                    GroupId = 321,
                    CongregationId = 5,
                    Name = "Test Journey Group 2016",
                    GroupRoleId = 16,
                    GroupDescription = "The group will test some new code",
                    MinistryId = 8,
                    ContactId = 4321,
                    GroupType = 19,
                    StartDate = Convert.ToDateTime("2016-02-12"),
                    EndDate = Convert.ToDateTime("2018-02-12"),
                    MeetingDayId = 3,
                    MeetingTime = "10 AM",
                    AvailableOnline = false,
                    Address = new Address()
                    {
                        Address_Line_1 = "123 Sesame St",
                        Address_Line_2 = "",
                        City = "South Side",
                        State = "OH",
                        Postal_Code = "12312"
                    }
                }
            };
            
            groupService.Setup(mocked => mocked.GetGroupsByTypeForParticipant(token, participantId, groupTypeId)).Returns(groups);

            var grps = fixture.GetGroupsByTypeForParticipant(token, participantId, groupTypeId);
           
            groupService.VerifyAll();
            Assert.IsNotNull(grps);
        }

        [Test]
        public void TestCreateGroup()
        {
            var start = DateTime.Now;
            var end = DateTime.Now.AddYears(2);

            var newGroup = new Group()
            {
                Name = "New Testing Group",
                GroupDescription = "The best group ever created for testing stuff and things",
                GroupId = 145,
                GroupType = 19,
                MinistryId = 8,
                CongregationId = 1,
                StartDate = start,
                EndDate = end,
                Full = false,
                AvailableOnline = true,
                RemainingCapacity = 8,
                WaitList = false,
                ChildCareAvailable = false,
                MeetingDayId = 2,
                MeetingTime = "18000",
                GroupRoleId = 16
            };
            
            var group = new GroupDTO()
            {
                GroupName = "New Testing Group",
                GroupId = 145,
                GroupDescription = "The best group ever created for testing stuff and things",                
                GroupTypeId = 19,
                MinistryId = 8,
                CongregationId = 1,
                StartDate = start,
                EndDate = end,
                GroupFullInd = false,
                AvailableOnline = true,
                RemainingCapacity = 8,
                WaitListInd = false,
                MeetingDayId = 2,
                MeetingTime = "18000",
                GroupRoleId = 16
            };

            groupService.Setup(mocked => mocked.CreateGroup(newGroup)).Returns(14);
            var groupResp =  fixture.CreateGroup(group);
     
            _groupService.VerifyAll();
            Assert.IsNotNull(groupResp);
        }

        [Test]
        public void WhenLookupParticipantIsCalledWithAllParticipantIdSpecified_ShouldNotLookupParticipant()
        {
            fixture.LookupParticipantIfEmpty("123", mockParticipantSignup);

            participantService.Verify(x => x.GetParticipantRecord(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void WhenLookupParticipantIsCalledWithoutParticipantIdSpecified_ShouldLookupParticipantAndSetParticipantId()
        {
            var token = "123";
            var participant = new Participant() {ParticipantId = 100};            

            participantService.Setup(x => x.GetParticipantRecord(token)).Returns(participant);
            var participants = new List<ParticipantSignup>
            {
                new ParticipantSignup()
                {                
                    childCareNeeded = false,
                    SendConfirmationEmail = true
                },
            };
            fixture.LookupParticipantIfEmpty(token, participants);

            participantService.Verify(x => x.GetParticipantRecord(It.IsAny<string>()), Times.Once);

            Assert.AreEqual(100, participants[0].particpantId);           
        }

        [Test]
        public void SendJourneyEmailInviteNoGroupsFound()
        {
            var groupId = 98765;
            const string token = "doit";
            var participant = new Participant() { ParticipantId = 100 };

            var communication = new EmailCommunicationDTO()
            {
                emailAddress = "BlackWidow@marvel.com",
                groupId = 98765
            };

            participantService.Setup(x => x.GetParticipantRecord(token)).Returns(participant);

            Assert.Throws<InvalidOperationException>(() => fixture.SendJourneyEmailInvite(communication, token));
            _communicationService.Verify(x => x.SendMessage(It.IsAny<Communication>(), false), Times.Never);
        }

        [Test]
        public void SendJourneyEmailInviteNoGroupMembershipFound()
        {
            var groupId = 98765;
            const string token = "doit";
            var participant = new Participant() { ParticipantId = 100 };
            var communication = new EmailCommunicationDTO()
            {
                emailAddress = "BlackWidow@marvel.com",
                groupId = 98765
            };

            var groups = new List<Group>()
            {
               new Group(){}
            };

            participantService.Setup(x => x.GetParticipantRecord(token)).Returns(participant);
            var membership = groups.Where(group => group.GroupId == groupId).ToList();
            Assert.AreEqual(membership.Count, 0);
            Assert.Throws<InvalidOperationException>(() => fixture.SendJourneyEmailInvite(communication, token));
            _communicationService.Verify(x => x.SendMessage(It.IsAny<Communication>(), false), Times.Never);
        }

        [Test]
        public void SendJourneyEmailInviteGroupMembershipIsFound()
        {
            const string token = "doit";
            const int groupId = 98765;
            var participant = new Participant() { ParticipantId = 100 };

            var groups = new List<Group>()
            {
               new Group()
               {
                   GroupId = 98765
               }
            };

            var communication = new EmailCommunicationDTO()
            {
                emailAddress = "BlackWidow@marvel.com",
                groupId = 98765
            };

            var template = new MessageTemplate()
            {
                Subject = "You Can Join My Group",
                Body = "This is a journey group."
            };
            var contact = new MyContact()
            {
                Contact_ID = 7689
            };

            participantService.Setup(x => x.GetParticipantRecord(token)).Returns(participant);
            groupService.Setup(x => x.GetGroupsByTypeForParticipant(token, participant.ParticipantId, 19)).Returns(groups);
            _communicationService.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(template);
            _contactService.Setup(mocked => mocked.GetContactById(It.IsAny<int>())).Returns(contact);
            _communicationService.Setup(m => m.SendMessage(It.IsAny<Communication>(), false)).Verifiable();

            var membership = groups.Where(group => group.GroupId == groupId).ToList();
            fixture.SendJourneyEmailInvite(communication, token);
            Assert.AreEqual(membership.Count, 1);
            _communicationService.Verify(m => m.SendMessage(It.IsAny<Communication>(), false), Times.Once);
        }
    }
}
