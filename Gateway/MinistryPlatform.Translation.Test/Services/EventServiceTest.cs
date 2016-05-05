using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
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
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _groupService = new Mock<IGroupService>();

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupsByEventId")).Returns(2221);
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            _fixture = new EventService(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object, _groupService.Object);
        }

        private EventService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<IGroupService> _groupService;
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
                    {"Event End Date", new DateTime(2015, 3, 28, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Event Title", "event-title-200"},
                    {"Event Type", "event-type-200"},
                    {"Event Start Date", new DateTime(2015, 4, 1, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 1, 8, 30, 0)}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Event Title", "event-title-300"},
                    {"Event Type", "event-type-300"},
                    {"Event Start Date", new DateTime(2015, 4, 2, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 2, 8, 30, 0)}
                }
                ,
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 400},
                    {"Event Title", "event-title-400"},
                    {"Event Type", "event-type-400"},
                    {"Event Start Date", new DateTime(2015, 4, 30, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 4, 30, 8, 30, 0)}
                }
                ,
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 500},
                    {"Event Title", "event-title-500"},
                    {"Event Type", "event-type-500"},
                    {"Event Start Date", new DateTime(2015, 5, 1, 8, 30, 0)},
                    {"Event End Date", new DateTime(2015, 5, 1, 8, 30, 0)}
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
                    {"Reminder_Days_Prior_ID", 2}
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
            var eventGroupPageViewId = _configWrapper.Object.GetConfigIntValue("GroupsByEventId");

            Prop.ForAll<string, int>((token, eventId) =>
            {
                var searchString = ",\"" + eventId + "\"";

                _ministryPlatformService.Setup(m => m.GetPageViewRecords(eventGroupPageViewId, token, searchString, "", 0)).Returns(GetMockedEventGroups(3));
                _fixture.GetEventGroupsForEvent(eventId, token);
                _ministryPlatformService.VerifyAll();
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldGetEventsBySite()
        {
            var eventsBySitePageViewId = _configWrapper.Object.GetConfigIntValue("EventsBySite");

            Prop.ForAll<string, bool, string>((site, template, token) =>
            {
                var searchString = ",," + site + ",," + template;

                _ministryPlatformService.Setup(m => m.GetPageViewRecords(eventsBySitePageViewId, token, searchString, "", 0)).Returns(GetMockedEvents(3));
                _fixture.GetEventsBySite(site, template, token);
                _ministryPlatformService.VerifyAll();
            }).QuickCheckThrowOnFailure();
        }

        [Test]
        public void ShouldDeleteEventGroupsForEvent()
        {
            Prop.ForAll<string, int, int>((token, selectionId, eventId) =>
            {
                var searchString = string.Format(",\"{0}\"", eventId);
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
                    { "Event_Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault }
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