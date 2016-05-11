using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Translation.Test.Helpers;

namespace MinistryPlatform.Translation.Test.Services
{
    public class RoomServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IConfigurationWrapper> _config;
        private Mock<IAuthenticationService> _authenticationService;

        private RoomService _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _config = new Mock<IConfigurationWrapper>();
            _authenticationService = new Mock<IAuthenticationService>();

            _authenticationService.Setup(mocked => mocked.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object>
            {
                {"token", "abc"}
            });

            _fixture = new RoomService(_ministryPlatformService.Object, _authenticationService.Object, _config.Object);
        }

        [Test]
        public void TestGetRoomReservations()
        {
            var l = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Cancelled", false},
                    {"Event_Room_ID", 1},
                    {"Hidden", false},
                    {"Notes", "Notes 1"},
                    {"Room_ID", 11},
                    {"Room_Layout_ID", 111},
                    {"Capacity", 1111},
                    {"Label", "Label 1"},
                    {"Room_Name", "Name 1"},
                    {"Allow_Checkin", false},
                    {"Volunteers", 1}
                },
                new Dictionary<string, object>
                {
                    {"Cancelled", true},
                    {"Event_Room_ID", 2},
                    {"Hidden", true},
                    {"Notes", "Notes 2"},
                    {"Room_ID", 22},
                    {"Room_Layout_ID", 222},
                    {"Capacity", 2222},
                    {"Label", "Label 2"},
                    {"Room_Name", "Name 2"},
                    {"Allow_Checkin", true},
                    {"Volunteers", 10}
                },
                new Dictionary<string, object>
                {
                    {"Cancelled", true},
                    {"Event_Room_ID", 3},
                    {"Hidden", true},
                    {"Notes", "Notes 3"},
                    {"Room_ID", 33},
                    {"Room_Layout_ID", 333},
                    {"Capacity", 3333},
                    {"Label", "Label 3"},
                    {"Room_Name", "Name 3"},
                    {"Allow_Checkin", null},
                    {"Volunteers", 11}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords("GetRoomReservations", It.IsAny<string>(), ",\"123\"", "", 0)).Returns(l);

            var reservations = _fixture.GetRoomReservations(123);
            _ministryPlatformService.VerifyAll();
            _authenticationService.VerifyAll();
            _config.VerifyAll();

            Assert.IsNotNull(reservations);
            Assert.AreEqual(l.Count, reservations.Count);

            for (var i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i]["Cancelled"], reservations[i].Cancelled);
                Assert.AreEqual(l[i]["Event_Room_ID"], reservations[i].EventRoomId);
                Assert.AreEqual(l[i]["Hidden"], reservations[i].Hidden);
                Assert.AreEqual(l[i]["Notes"], reservations[i].Notes);
                Assert.AreEqual(l[i]["Room_ID"], reservations[i].RoomId);
                Assert.AreEqual(l[i]["Room_Layout_ID"], reservations[i].RoomLayoutId);
                Assert.AreEqual(l[i]["Capacity"], reservations[i].Capacity);
                Assert.AreEqual(l[i]["Label"], reservations[i].Label);
                Assert.AreEqual(l[i]["Room_Name"], reservations[i].Name);

                // this is to handle a null value for allow checkin values, which is getting set to false if null
                var allowCheckinValue = l[i]["Allow_Checkin"];

                if (allowCheckinValue != null)
                {
                    Assert.AreEqual(l[i]["Allow_Checkin"], reservations[i].CheckinAllowed);
                }
                else
                {
                    Assert.AreEqual(false, reservations[i].CheckinAllowed);
                }

                Assert.AreEqual(l[i]["Volunteers"], reservations[i].Volunteers);
            }
        }

        [Test]
        public void ShouldDeleteEventRoomsForEvent()
        {
            Prop.ForAll<string, int, int>((token, selectionId, eventId) =>
            {
                var searchString = string.Format(",\"{0}\"", eventId);
                var eventRooms = GetMockedEventRooms(3);

                _ministryPlatformService.Setup(m => m.GetPageViewRecords(It.IsAny<string>(), It.IsAny<string>(), searchString, "", 0)).Returns(eventRooms);

                var eventRoomIds = Conversions.BuildIntArrayFromKeyValue(eventRooms, "Event_Room_ID").ToArray();

                _ministryPlatformService.Setup(m => m.CreateSelection(It.IsAny<SelectionDescription>(), token)).Returns(selectionId);
                _ministryPlatformService.Setup(m => m.AddToSelection(selectionId, eventRoomIds, token));
                _ministryPlatformService.Setup(m => m.DeleteSelectionRecords(selectionId, token));
                _ministryPlatformService.Setup(m => m.DeleteSelection(selectionId, token));

                _fixture.DeleteEventRoomsForEvent(eventId, token);
                _ministryPlatformService.VerifyAll();
            }).QuickCheckThrowOnFailure();
        }

        private List<Dictionary<string, object>> GetMockedEventRooms(int recordsToGenerate)
        {
            var recordsList = new List<Dictionary<string, object>>();

            for (var i = 0; i < recordsToGenerate; i++)
            {
                recordsList.Add(new Dictionary<string, object>
                {
                    {"Cancelled", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault},
                    {"Event_Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Hidden", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault},
                    {"Notes", Gen.Sample(75, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault},
                    {"Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Room_Layout_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Capacity", Gen.Sample(3, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Label", Gen.Sample(75, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault},
                    {"Room_Name", Gen.Sample(75, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault},
                    {"Allow_Checkin", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault},
                    {"Volunteers", Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault}
                });
            }

            return recordsList;
        }
    }
}
