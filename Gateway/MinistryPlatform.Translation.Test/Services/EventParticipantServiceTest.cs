using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class EventParticipantServiceTest
    {
        private EventParticipantRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;
        private Mock<IAuthenticationRepository> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _authService = new Mock<IAuthenticationRepository>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            _configWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_USER")).Returns("api-user");
            _configWrapper.Setup(mocked => mocked.GetEnvironmentVarAsString("API_PASSWORD")).Returns("api-password");
            _configWrapper.Setup(mocked => mocked.GetConfigIntValue("TripDestinationDocuments")).Returns(1234);

            _fixture = new EventParticipantRepository(_ministryPlatformService.Object, _ministryPlatformRest.Object, _authService.Object, _configWrapper.Object);
        }

        [Test]
        public void AddDocumentsToTripParticipantTest()
        {
            const int eventParticipantId = 9;
            var docs = new List<MpTripDocuments>
            {
                new MpTripDocuments
                {
                    DocumentId = 1,
                    Description = "doc 1 desc",
                    Document = "doc 1"
                },
                new MpTripDocuments
                {
                    DocumentId = 2,
                    Description = "doc 2 desc",
                    Document = "doc 2"
                }
            };

            _ministryPlatformService.Setup(m => m.CreateSubRecord("EventParticipantDocuments", eventParticipantId, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true))
                .Returns(34567);
            var returnVal = _fixture.AddDocumentsToTripParticipant(docs, eventParticipantId);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(returnVal);
            Assert.AreEqual(true, returnVal);
        }

        [Test]
        public void TestGetEventParticipants()
        {
            var p = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Event_Participant_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Participant_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Participation_Status_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault}
                },
                new Dictionary<string, object>
                {
                    {"Event_Participant_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Participant_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Participation_Status_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault},
                    {"Room_ID", Gen.Sample(7, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault}
                }

            };

            _ministryPlatformService.Setup(mocked => mocked.GetSubpageViewRecords("EventParticipantAssignedToRoomApiSubPageView", 123, "ABC", ",,,\"987\"", "", 0)).Returns(p);
            var result = _fixture.GetEventParticipants(123, 987);

            _ministryPlatformService.VerifyAll();
            Assert.NotNull(result);
            Assert.AreEqual(p.Count, result.Count);
            for (var i = 0; i < p.Count; i++)
            {
                Assert.AreEqual(p[i]["Event_Participant_ID"], result[i].EventParticipantId);
                Assert.AreEqual(p[i]["Participant_ID"], result[i].ParticipantId);
                Assert.AreEqual(p[i]["Participation_Status_ID"], result[i].ParticipantStatus);
                Assert.AreEqual(p[i]["Room_ID"], result[i].RoomId);
            }
        }
    }
}