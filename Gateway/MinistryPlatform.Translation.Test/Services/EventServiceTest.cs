using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Test.Helpers;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class EventServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);
            _ministryPlatformRestService = new Mock<IMinistryPlatformRestRepository>(MockBehavior.Strict);
            _authService = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            _configWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _groupService = new Mock<IGroupRepository>(MockBehavior.Strict);

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupsByEventId")).Returns(2221);
            _configWrapper.Setup(m => m.GetConfigIntValue("EventsBySite")).Returns(2222);
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            _fixture = new EventRepository(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object, _groupService.Object, _ministryPlatformRestService.Object);
        }

        private EventRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRestService;
        private Mock<IAuthenticationRepository> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<IGroupRepository> _groupService;
        private const int EventParticipantPageId = 281;
        private const int EventParticipantStatusDefaultId = 2;
        private const int EventsPageId = 308;
        private const string EventsWithEventTypeId = "EventsWithEventTypeId";

        private List<Dictionary<string, object>> MockEventsDictionaryByEventTypeId()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Event Title", "event-title-100"},
                    {"Event Type", "event-type-100"},
                    {"Event Start Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Congregation_ID", 1}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Event Title", "event-title-200"},
                    {"Event Type", "event-type-200"},
                    {"Event Start Date", new DateTime(2015, 4, 1, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 1, 8, 30, 0)},
                    {"Congregation_ID", 1}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Event Title", "event-title-300"},
                    {"Event Type", "event-type-300"},
                    {"Event Start Date", new DateTime(2015, 4, 2, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 2, 8, 30, 0)},
                    {"Congregation_ID", 1}
                }
                ,
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 400},
                    {"Event Title", "event-title-400"},
                    {"Event Type", "event-type-400"},
                    {"Event Start Date", new DateTime(2015, 4, 30, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 30, 8, 30, 0)},
                    {"Congregation_ID", 1}
                }
                ,
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 500},
                    {"Event Title", "event-title-500"},
                    {"Event Type", "event-type-500"},
                    {"Event Start Date", new DateTime(2015, 5, 1, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 5, 1, 8, 30, 0)},
                    {"Congregation_ID", 1}
                }
            };
        }

        private List<Dictionary<string, object>> MockEventsDictionary()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Event_Title", "event-title-100"},
                    {"Event_Type", "event-type-100"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Event_Title", "event-title-200"},
                    {"Event_Type", "event-type-200"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Event_Title", "event-title-300"},
                    {"Event_Type", "event-type-300"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                }
            };
        }

        private List<Dictionary<string, object>> MockEventParticipantsByEventIdAndParticipantId()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_Participant_ID", 8634},
                    {"Event_ID", 93},
                    {"Participant_ID", 134}
                }
            };
        }

        [Test]
        public void GetEvent()
        {
            //Arrange
            const int eventId = 999;
            const string pageKey = "EventsWithDetail";
            var mockEventDictionary = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_ID", 999},
                    {"Event_Title", "event-title-100"},
                    {"Event_Type", "event-type-100"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Contact_ID", 12345},
                    {"Email_Address", "thecinnamonbagel@react.js"},
                    {"Parent_Event_ID", 6543219},
                    {"Congregation_ID", It.IsAny<int>()},
                    {"Reminder_Days_Prior_ID", 2},
                    {"Cancelled", false}
                }
            };
            var searchString = eventId + ",";
            _ministryPlatformService.Setup(m => m.GetPageViewRecords(pageKey, It.IsAny<string>(), searchString, string.Empty, 0)).Returns(mockEventDictionary);

            //Act
            var theEvent = _fixture.GetEvent(eventId);

            //Assert
            _ministryPlatformService.VerifyAll();

            Assert.AreEqual(eventId, theEvent.EventId);
            Assert.AreEqual("event-title-100", theEvent.EventTitle);
            Assert.AreEqual(12345, theEvent.PrimaryContact.ContactId);
            Assert.AreEqual("thecinnamonbagel@react.js", theEvent.PrimaryContact.EmailAddress);
        }

        [Test]
        public void GetEventByParentId()
        {
            const int expectedEventId = 999;
            const int parentEventId = 888;
            var searchString = string.Format(",,,{0}", parentEventId);
            const string pageKey = "EventsByParentEventID";

            var mockEventDictionary = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_ID", 999},
                    {"Event_Title", "event-title-100"},
                    {"Event_Type", "event-type-100"},
                    {"Event_Start_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Event_End_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Contact_ID", 12345},
                    {"Email_Address", "thecinnamonbagel@react.js"}
                }
            };

            _ministryPlatformService.Setup(m => m.GetPageViewRecords(pageKey, It.IsAny<string>(), searchString, string.Empty, 0)).Returns(mockEventDictionary);

            var events = _fixture.GetEventsByParentEventId(parentEventId);

            _ministryPlatformService.VerifyAll();

            Assert.AreEqual(1, events.Count);
            var theEvent = events[0];
            Assert.AreEqual(expectedEventId, theEvent.EventId);
            Assert.AreEqual("event-title-100", theEvent.EventTitle);
            Assert.AreEqual(12345, theEvent.PrimaryContact.ContactId);
            Assert.AreEqual("thecinnamonbagel@react.js", theEvent.PrimaryContact.EmailAddress);
        }

        [Test]
        public void GetEventParticipant()
        {
            const int eventId = 1234;
            const int participantId = 5678;
            const string pageKey = "EventParticipantByEventIdAndParticipantId";
            var mockEventParticipants = MockEventParticipantsByEventIdAndParticipantId();

            _ministryPlatformService.Setup(m => m.GetPageViewRecords(pageKey, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(mockEventParticipants);

            var participant = _fixture.GetEventParticipantRecordId(eventId, participantId);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(participant);
            Assert.AreEqual(8634, participant);
        }

        [Test]
        public void GetEventsByType()
        {
            const string eventType = "Oakley: Saturday at 4:30";

            var search = ",," + eventType;
            _ministryPlatformService.Setup(mock => mock.GetRecordsDict(EventsPageId, It.IsAny<string>(), search, ""))
                .Returns(MockEventsDictionary());

            var events = _fixture.GetEvents(eventType, It.IsAny<string>());
            Assert.IsNotNull(events);
        }

        [Test]
        public void GetEventsByTypeAndRange()
        {
            var eventTypeId = 1;
            var search = ",," + eventTypeId;
            _ministryPlatformService.Setup(mock => mock.GetPageViewRecords(EventsWithEventTypeId, It.IsAny<string>(), search, "", 0))
                .Returns(MockEventsDictionaryByEventTypeId());

            var startDate = new DateTime(2015, 4, 1);
            var endDate = new DateTime(2015, 4, 30);
            var events = _fixture.GetEventsByTypeForRange(eventTypeId, startDate, endDate, It.IsAny<string>());
            Assert.IsNotNull(events);
            Assert.AreEqual(3, events.Count);
            Assert.AreEqual("event-title-200", events[0].EventTitle);
        }

        [Test]
        public void TestRegisterParticipantForEvent()
        {
            _ministryPlatformService.Setup(mocked => mocked.CreateSubRecord(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Dictionary<string, object>>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).Returns(987);

            var expectedValues = new Dictionary<string, object>
            {
                {"Participant_ID", 123},
                {"Event_ID", 456},
                {"Participation_Status_ID", EventParticipantStatusDefaultId}
            };

            var eventParticipantId = _fixture.RegisterParticipantForEvent(123, 456);

            _ministryPlatformService.Verify(mocked => mocked.CreateSubRecord(
                EventParticipantPageId,
                456,
                expectedValues,
                It.IsAny<string>(),
                true));

            Assert.AreEqual(987, eventParticipantId);
        }

        [Test]
        public void ShouldGetEventGroupsForEvent()
        {
            var eventId = 983274;
            var token = "wesadf";

            var eventGroupPageViewId = _configWrapper.Object.GetConfigIntValue("GroupsByEventId");
            var searchString = string.Format("\"{0}\",", eventId);
            var eventGroups = GetMockedEventGroups(new System.Random(DateTime.Now.Millisecond).Next(10));

            _ministryPlatformService.Setup(m => m.GetPageViewRecords(eventGroupPageViewId, token, searchString, "", 0)).Returns(eventGroups);
            var result = _fixture.GetEventGroupsForEvent(eventId, token);
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(eventGroups.Count, result.Count);
            for (var i = 0; i < eventGroups.Count; i++)
            {
                Assert.AreEqual(eventGroups[i]["Event_Group_ID"], result[i].EventGroupId);
                Assert.AreEqual(eventGroups[i]["Event_ID"], result[i].EventId);
                Assert.AreEqual(eventGroups[i]["Group_ID"], result[i].GroupId);
                Assert.AreEqual(eventGroups[i]["Room_ID"], result[i].RoomId);
                Assert.AreEqual(eventGroups[i]["Closed"], result[i].Closed);
                Assert.AreEqual(eventGroups[i]["Event_Room_ID"], result[i].EventRoomId);
            }
        }

        [Test]
        public void ShouldGetEventsBySite()
        {
            //var pageViewId = _configurationWrapper.GetConfigIntValue("EventsBySite");
            //var records = _ministryPlatformService.GetPageViewRecords(pageViewId, token, searchString);

            //var eventsBySitePageViewId = _configWrapper.Object.GetConfigIntValue("EventsBySite");
            var currentDateTime = DateTime.Now;
            var site = "Oakley";
            var token = "123";

            var searchString = ",,\"" + site + "\",,False," + currentDateTime.ToShortDateString() + "," + currentDateTime.ToShortDateString(); // search string needs to match

            _ministryPlatformService.Setup(m => m.GetPageViewRecords(2222, token, searchString, "", 0)).Returns(GetMockedEvents(3));
            _fixture.GetEventsBySite(site, token, currentDateTime, currentDateTime);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void ShouldGetEventTemplatesBySite()
        {
            var site = "Oakley";
            var token = "123";

            var searchString = ",,\"" + site + "\",,True,"; // search string needs to match

            _ministryPlatformService.Setup(m => m.GetPageViewRecords(2222, token, searchString, "", 0)).Returns(GetMockedEvents(3));
            _fixture.GetEventTemplatesBySite(site, token);
            _ministryPlatformService.VerifyAll();
        }


        [Test]
        public void ShouldDeleteEventGroupsForEvent()
        {
            Prop.ForAll<string, int, int>((token, selectionId, eventId) =>
            {
                var searchString = string.Format("\"{0}\",", eventId);
                var eventGroups = GetMockedEventGroups(3);

                _ministryPlatformService.Setup(m => m.GetPageViewRecords(2221, token, searchString, "", 0)).Returns(eventGroups);

                var eventGroupIntIds = Conversions.BuildIntArrayFromKeyValue(eventGroups, "Event_Group_ID").ToArray();

                _ministryPlatformService.Setup(m => m.CreateSelection(It.IsAny<SelectionDescription>(), token)).Returns(selectionId);
                _ministryPlatformService.Setup(m => m.AddToSelection(selectionId, eventGroupIntIds, token));
                _ministryPlatformService.Setup(m => m.DeleteSelectionRecords(selectionId, token));
                _ministryPlatformService.Setup(m => m.DeleteSelection(selectionId, token));

                _fixture.DeleteEventGroupsForEvent(eventId, token);
                _ministryPlatformService.VerifyAll();
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldGetWaiversForEvent()
        {
            var eventId = 12345;
            var contactId = 67890;
            const string columnList = "Waiver_ID_Table.[Waiver_ID], Waiver_ID_Table.[Waiver_Name], Waiver_ID_Table.[Waiver_Text], cr_Event_Waivers.[Required]";
            const string columns = "cr_Event_Participant_Waivers.Waiver_ID, cr_Event_Participant_Waivers.Event_Participant_ID, Accepted, Signee_Contact_ID";

            var mockWaiver = mockWaivers();

            var mockWaiverResponse = new List<MpWaiverResponse>
            {
                new MpWaiverResponse
                {
                    Accepted = true,
                    SigneeContactId = 09876
                }
            };

            var mockWaiverResponse2 = new List<MpWaiverResponse>
            {
                new MpWaiverResponse
                {
                    Accepted = false,
                    SigneeContactId = contactId
                }
            };

            _ministryPlatformRestService.Setup(m => m.Search<MpWaivers>($"Event_ID = {eventId}", columnList, null, false)).Returns(mockWaiver);
            _ministryPlatformRestService.Setup(m => m.Search<MpWaiverResponse>($"Waiver_ID_Table.Waiver_ID = 123 AND Event_Participant_ID_Table_Event_ID_Table.Event_ID = {eventId}", columns, null, false)).Returns(mockWaiverResponse);
            _ministryPlatformRestService.Setup(m => m.Search<MpWaiverResponse>($"Waiver_ID_Table.Waiver_ID = 456 AND Event_Participant_ID_Table_Event_ID_Table.Event_ID = {eventId}", columns, null, false)).Returns(mockWaiverResponse2);
            _ministryPlatformRestService.Setup(m => m.UsingAuthenticationToken("ABC")).Returns(_ministryPlatformRestService.Object);

            var result = _fixture.GetWaivers(eventId, contactId);

            _ministryPlatformRestService.VerifyAll();
            Assert.AreEqual(2,result.Count);
            Assert.IsTrue(result[0].Accepted);
            Assert.IsFalse(result[1].Accepted);
            Assert.AreEqual(09876, result[0].SigneeContactId);
            Assert.AreEqual(contactId, result[1].SigneeContactId);
        }

        [Ignore]
        [Test]
        public void ShouldSaveWaiversForEventParticipant()
        {
            var mockWaiverResponse = new List<MpWaiverResponse>
            {
                new MpWaiverResponse
                {
                    Accepted = true,
                    SigneeContactId = 9876,
                    EventParticipantWaiverId = 0,
                    EventParticipantId = 525123,
                    WaiverId = 2
                }
            };

            var mockWaiverResponse2 = new List<MpWaiverResponse>
            {
                new MpWaiverResponse
                {
                    Accepted = true,
                    SigneeContactId = 9876,
                    EventParticipantWaiverId = 123456,
                    EventParticipantId = 525123,
                    WaiverId = 2
                }
            };

            var emptyList = new List<MpWaiverResponse>();

            _ministryPlatformRestService.Setup(m => m.Search<int>("cr_Event_Participant_Waivers", $"Event_Participant_ID={mockWaiverResponse[0].EventParticipantId} AND Waiver_ID={mockWaiverResponse[0].WaiverId}", "Event_Participant_Waiver_ID")).Returns(123456);
            _ministryPlatformRestService.Setup(m => m.Post(emptyList)).Returns(0);
            _ministryPlatformRestService.Setup(m => m.Put(mockWaiverResponse2)).Returns(1);
            _ministryPlatformRestService.Setup(m => m.UsingAuthenticationToken("ABC")).Returns(_ministryPlatformRestService.Object);

            _fixture.SetWaivers(mockWaiverResponse);

            _ministryPlatformRestService.VerifyAll();
        }

        private static List<MpWaivers> mockWaivers()
        {
            return new List<MpWaivers>
            {
                new MpWaivers
                {
                    Accepted = false,
                    Required = true,
                    SigneeContactId = 0,
                    WaiverName = "NoRights",
                    WaiverId = 123,
                    WaiverText = "I waive ALL my rights."
                },
                new MpWaivers
                {
                    Accepted = false,
                    Required = true,
                    SigneeContactId = 0,
                    WaiverName = "SomeRights",
                    WaiverId = 456,
                    WaiverText = "I waive Some of my rights."
                }
            };
        }

        private static List<Dictionary<string, object>> GetMockedEventGroups(int recordsToGenerate)
        {
            var recordsList = new List<Dictionary<string, object>>();

            for (var i = 0; i < recordsToGenerate; i++)
            {
                recordsList.Add(new Dictionary<string, object>
                {
                    { "Event_Group_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Event_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Group_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Domain_ID", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Closed", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault },
                    { "Event_Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Group_Type_ID",Gen.Sample(7,1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault }
                });
            }

            return recordsList;
        }

        private static List<Dictionary<string, object>> GetMockedEvents(int recordsToGenerate)
        {
            var recordsList = new List<Dictionary<string, object>>();

            for (var i = 0; i < recordsToGenerate; i++)
            {
                recordsList.Add(new Dictionary<string, object>
                {
                    { "Event_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Congregation_Name", Gen.Sample(75, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault },
                    { "Congregation_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Site", Gen.Sample(10, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault },
                    { "Event_Title", Gen.Sample(75, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault },
                    { "Event_Type_ID", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Event_Start_Date", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<DateTime>())).HeadOrDefault },
                    { "Event_End_Date", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<DateTime>())).HeadOrDefault },
                    { "Parent_Event_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault },
                    { "Template", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault }
                });
            }

            return recordsList;
        }
    }
}