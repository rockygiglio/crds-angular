using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Services.Interfaces;

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
                    {"Allow_Checkin", false}
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
                    {"Allow_Checkin", true}
                },
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
                Assert.AreEqual(l[i]["Allow_Checkin"], reservations[i].CheckinAllowed);
            }
        }
    }
}
