using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.App_Start;
using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using MpEvent = MinistryPlatform.Translation.Models.MpEvent;
using IEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;
using Moq;
using MvcContrib.TestHelper.Ui;
using NUnit.Framework;
using MinistryPlatform.Translation.Models.EventReservations;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class EventServiceTest 
    {
        private Mock<IContactRelationshipService> _contactRelationshipService;
        private Mock<IContactService> _contactService;
        private Mock<IContentBlockService> _contentBlockService;
        private Mock<IEventService> _eventService;
        private Mock<IParticipantService> _participantService;
        private Mock<IGroupParticipantService> _groupParticipantService;
        private Mock<IGroupService> _groupService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IApiUserService> _apiUserService;
        private Mock<IRoomService> _roomService;
        private Mock<IEquipmentService> _equipmentService;
        private Mock<IEventParticipantService> _eventParticipantService;

        private EventService _fixture;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();
          
            _contactRelationshipService = new Mock<IContactRelationshipService>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _apiUserService = new Mock<IApiUserService>(MockBehavior.Strict);
            _contentBlockService = new Mock<IContentBlockService>(MockBehavior.Strict);
            _contactService = new Mock<IContactService>(MockBehavior.Strict);
            _groupService = new Mock<IGroupService>(MockBehavior.Strict);
            _communicationService = new Mock<ICommunicationService>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _apiUserService = new Mock<IApiUserService>(MockBehavior.Strict);
            _groupParticipantService = new Mock<IGroupParticipantService>(MockBehavior.Strict);
            _participantService = new Mock<IParticipantService>(MockBehavior.Strict);
            _eventService = new Mock<IEventService>();
            _roomService = new Mock<IRoomService>();
            _equipmentService= new Mock<IEquipmentService>();
            _eventParticipantService = new Mock<IEventParticipantService>(MockBehavior.Strict);


            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("EventsReadyForPrimaryContactReminder")).Returns(2205);
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("EventPrimaryContactReminderTemplateId")).Returns(14909);
     
            _fixture = new EventService(_eventService.Object,
                                        _groupService.Object,
                                        _communicationService.Object,
                                        _contactService.Object, 
                                        _contentBlockService.Object, 
                                        _configurationWrapper.Object,  
                                        _apiUserService.Object,
                                        _contactRelationshipService.Object,
                                        _groupParticipantService.Object,
                                        _participantService.Object,
                                        _roomService.Object,
                                        _equipmentService.Object,
                                        _eventParticipantService.Object);
        }
        
        [Test]
        public void ShouldSendPrimaryContactReminderEmails()
        {
            const string search = "";
            const string apiToken = "qwerty1234";
            var defaultContact = new MyContact()
            {
                Contact_ID = 321,
                Email_Address = "default@email.com"
            };

            var testEvent = new MpEvent ()
            {
                EventId = 32,
                EventStartDate = new DateTime(),
                EventEndDate = new DateTime().AddHours(2),
                PrimaryContact = new MpContact()
                {
                    EmailAddress = "test@test.com",
                    ContactId = 4321
                }
            };

            var testEventList = new List<MpEvent>()
            {
               testEvent
            };
       
            _apiUserService.Setup(m => m.GetToken()).Returns(apiToken);
            _eventService.Setup(m => m.EventsByPageViewId(apiToken, 2205, search)).Returns(testEventList);
            var eventList = testEventList.Select(evt => new crds_angular.Models.Crossroads.Events.Event() 
            {
                name = evt.EventTitle,
                EventId = evt.EventId,
                EndDate = evt.EventEndDate,
                StartDate = evt.EventStartDate,
                EventType = evt.EventType,
                location = evt.Congregation,
                PrimaryContactEmailAddress = evt.PrimaryContact.EmailAddress,
                PrimaryContactId = evt.PrimaryContact.ContactId
            });
            
            eventList.ForEach(evt =>
            {
                var mergeData = new Dictionary<string, object>
                {
                    {"Event_ID", evt.EventId},
                    {"Event_Title", evt.name},
                    {"Event_Start_Date", evt.StartDate.ToShortDateString()},
                    {"Event_Start_Time", evt.StartDate.ToShortTimeString()}               
                };

                var contact = new MpContact() { ContactId = defaultContact.Contact_ID, EmailAddress = defaultContact.Email_Address };
                var fakeCommunication = new MpCommunication()
                {
                    AuthorUserId = defaultContact.Contact_ID,
                    DomainId = 1,
                    EmailBody = "Test event email stuff",
                    EmailSubject = "Test Event Reminder",
                    FromContact = contact,
                    MergeData = mergeData,
                    ReplyToContact = contact,
                    TemplateId = 14909,
                    ToContacts = new List<MpContact>() { contact }
                };

                var testContact = new MyContact()
                {
                    Contact_ID = 9876,
                    Email_Address = "ghj@cr.net"

                };
              
                _contactService.Setup(m => m.GetContactById(9876)).Returns(testContact);
                _communicationService.Setup(m => m.GetTemplateAsCommunication(14909,
                                                                              testContact.Contact_ID,
                                                                              testContact.Email_Address,
                                                                              evt.PrimaryContactId,
                                                                              evt.PrimaryContactEmailAddress,
                                                                              evt.PrimaryContactId,
                                                                              evt.PrimaryContactEmailAddress,
                                                                              mergeData)).Returns(fakeCommunication);
                _communicationService.Setup(m => m.SendMessage(fakeCommunication, false));
                _communicationService.Verify();

            });
            _fixture.EventsReadyForPrimaryContactReminder(apiToken);
            _eventService.Verify();
            
        }

        [Test]
        public void TestGetEventRoomDetails()
        {
            var e = new MpEvent
            {
                EventTitle = "title",
                CongregationId = 12,
                EventEndDate = DateTime.Today.AddDays(2),
                EventStartDate = DateTime.Today.AddDays(1),
                ReminderDaysPriorId = 34
            };
            _eventService.Setup(mocked => mocked.GetEvent(123)).Returns(e);

            var r = new List<MpRoomReservationDto>
            {
                new MpRoomReservationDto
                {
                    Cancelled = false,
                    Hidden = false,
                    RoomLayoutId = 1,
                    Notes = "notes 1",
                    RoomId = 11,
                    EventRoomId = 111,
                    Capacity = 1111,
                    CheckinAllowed = false,
                    Name = "name 1",
                    Label = "label 1"
                },
                new MpRoomReservationDto
                {
                    Cancelled = true,
                    Hidden = true,
                    RoomLayoutId = 2,
                    Notes = "notes 2",
                    RoomId = 22,
                    EventRoomId = 222,
                    Capacity = 2222,
                    CheckinAllowed = true,
                    Name = "name 2",
                    Label = "label 2"
                }
            };
            _roomService.Setup(mocked => mocked.GetRoomReservations(123)).Returns(r);

            var p = new List<List<EventParticipant>>
            {
                new List<EventParticipant>
                {
                    new EventParticipant()
                },
                new List<EventParticipant>
                {
                    new EventParticipant(),
                    new EventParticipant()
                }
            };
            _eventParticipantService.Setup(mocked => mocked.GetEventParticipants(123, 11)).Returns(p[0]);
            _eventParticipantService.Setup(mocked => mocked.GetEventParticipants(123, 22)).Returns(p[1]);

            var response = _fixture.GetEventRoomDetails(123);
            _eventService.VerifyAll();
            _roomService.VerifyAll();
            _equipmentService.VerifyAll();
            _eventParticipantService.VerifyAll();

            Assert.NotNull(response);
            Assert.AreEqual(e.EventTitle, response.Title);
            Assert.AreEqual(e.CongregationId, response.CongregationId);
            Assert.AreEqual(e.EventStartDate, response.StartDateTime);
            Assert.AreEqual(e.EventEndDate, response.EndDateTime);
            Assert.AreEqual(e.ReminderDaysPriorId, response.ReminderDaysId);
            Assert.IsNotNull(response.Rooms);
            Assert.AreEqual(r.Count, response.Rooms.Count);
            for (var i = 0; i < r.Count; i++)
            {
                Assert.AreEqual(r[i].Cancelled, response.Rooms[i].Cancelled);
                Assert.AreEqual(r[i].Hidden, response.Rooms[i].Hidden);
                Assert.AreEqual(r[i].RoomLayoutId, response.Rooms[i].LayoutId);
                Assert.AreEqual(r[i].Notes, response.Rooms[i].Notes);
                Assert.AreEqual(r[i].RoomId, response.Rooms[i].RoomId);
                Assert.AreEqual(r[i].EventRoomId, response.Rooms[i].RoomReservationId);
                Assert.AreEqual(r[i].Capacity, response.Rooms[i].Capacity);
                Assert.AreEqual(r[i].CheckinAllowed, response.Rooms[i].CheckinAllowed);
                Assert.AreEqual(r[i].Name, response.Rooms[i].Name);
                Assert.AreEqual(r[i].Label, response.Rooms[i].Label);
                Assert.AreEqual(p[i].Count, response.Rooms[i].ParticipantsAssigned);
            }
        }

        [Test]
        public void TestGetEventReservation()
        {
            var e = new MpEvent
            {
                EventTitle = "title",
                CongregationId = 12,
                EventEndDate = DateTime.Today.AddDays(2),
                EventStartDate = DateTime.Today.AddDays(1),
                ReminderDaysPriorId = 34
            };
            _eventService.Setup(mocked => mocked.GetEvent(123)).Returns(e);

            var r = new List<MpRoomReservationDto>
            {
                new MpRoomReservationDto
                {
                    Cancelled = false,
                    Hidden = false,
                    RoomLayoutId = 1,
                    Notes = "notes 1",
                    RoomId = 11,
                    EventRoomId = 111,
                    Capacity = 1111,
                    CheckinAllowed = false,
                    Name = "name 1",
                    Label = "label 1"
                },
                new MpRoomReservationDto
                {
                    Cancelled = true,
                    Hidden = true,
                    RoomLayoutId = 2,
                    Notes = "notes 2",
                    RoomId = 22,
                    EventRoomId = 222,
                    Capacity = 2222,
                    CheckinAllowed = true,
                    Name = "name 2",
                    Label = "label 2"
                }
            };
            _roomService.Setup(mocked => mocked.GetRoomReservations(123)).Returns(r);

            var q = new List<List<MpEquipmentReservationDto>>()
            {
                 new List<MpEquipmentReservationDto>{
                    new MpEquipmentReservationDto
                    {
                        Cancelled = false,
                        EquipmentId = 1,
                        EventRoomId = 11,
                        QuantityRequested = 111
                    },
                    new MpEquipmentReservationDto
                    {
                        Cancelled = true,
                        EquipmentId = 2,
                        EventRoomId = 22,
                        QuantityRequested = 222
                    }

                },
                new List<MpEquipmentReservationDto>
                {
                    new MpEquipmentReservationDto
                    {
                        Cancelled = false,
                        EquipmentId = 3,
                        EventRoomId = 33,
                        QuantityRequested = 333
                    },
                    new MpEquipmentReservationDto
                    {
                        Cancelled = true,
                        EquipmentId = 4,
                        EventRoomId = 44,
                        QuantityRequested = 444
                    }
                }
            };
            _equipmentService.Setup(mocked => mocked.GetEquipmentReservations(123, r[0].RoomId)).Returns(q[0]);
            _equipmentService.Setup(mocked => mocked.GetEquipmentReservations(123, r[1].RoomId)).Returns(q[1]);

            var response = _fixture.GetEventReservation(123);
            _eventService.VerifyAll();
            _roomService.VerifyAll();
            _equipmentService.VerifyAll();

            Assert.NotNull(response);
            Assert.AreEqual(e.EventTitle, response.Title);
            Assert.AreEqual(e.CongregationId, response.CongregationId);
            Assert.AreEqual(e.EventStartDate, response.StartDateTime);
            Assert.AreEqual(e.EventEndDate, response.EndDateTime);
            Assert.AreEqual(e.ReminderDaysPriorId, response.ReminderDaysId);
            Assert.IsNotNull(response.Rooms);
            Assert.AreEqual(r.Count, response.Rooms.Count);
            for (var i = 0; i < r.Count; i++)
            {
                Assert.AreEqual(r[i].Cancelled, response.Rooms[i].Cancelled);
                Assert.AreEqual(r[i].Hidden, response.Rooms[i].Hidden);
                Assert.AreEqual(r[i].RoomLayoutId, response.Rooms[i].LayoutId);
                Assert.AreEqual(r[i].Notes, response.Rooms[i].Notes);
                Assert.AreEqual(r[i].RoomId, response.Rooms[i].RoomId);
                Assert.AreEqual(r[i].EventRoomId, response.Rooms[i].RoomReservationId);
                Assert.AreEqual(r[i].Capacity, response.Rooms[i].Capacity);
                Assert.AreEqual(r[i].CheckinAllowed, response.Rooms[i].CheckinAllowed);
                Assert.AreEqual(r[i].Name, response.Rooms[i].Name);
                Assert.AreEqual(r[i].Label, response.Rooms[i].Label);
                Assert.IsNotNull(response.Rooms[i].Equipment);
                Assert.AreEqual(q[i].Count, response.Rooms[i].Equipment.Count);
                for (var j = 0; j < q[i].Count; j++)
                {
                    Assert.AreEqual(q[i][j].Cancelled, response.Rooms[i].Equipment[j].Cancelled);
                    Assert.AreEqual(q[i][j].EquipmentId, response.Rooms[i].Equipment[j].EquipmentId);
                    Assert.AreEqual(q[i][j].EventEquipmentId, response.Rooms[i].Equipment[j].EquipmentReservationId);
                    Assert.AreEqual(q[i][j].QuantityRequested, response.Rooms[i].Equipment[j].QuantityRequested);
                }
            }
        }

    }
}
      
