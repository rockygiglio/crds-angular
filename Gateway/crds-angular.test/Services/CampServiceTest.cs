using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Services;
using Crossroads.Utilities.FunctionalHelpers;
using FsCheck;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class CampServiceTest
    {
        private Mock<IContactRepository> _contactService;
        private Mock<ICampRepository> _campService;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IParticipantRepository> _participantRepository; 

        private CampService _fixture;

        [SetUp]
        public void SetUp()
        {
            _contactService = new Mock<IContactRepository>();
            _campService = new Mock<ICampRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _participantRepository = new Mock<IParticipantRepository>();

            _fixture = new CampService(_contactService.Object, _campService.Object, _participantRepository.Object);
        }

        [Test]

        public void ShouldGetCampEventDetails()
        {
            const string token = "asdfasdf";
            int eventId = 123;

            var campEventDTO = MockCampEvents();
            
            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _campService.Setup(m => m.GetCampEventDetails(eventId)).Returns(campEventDTO);

            var campEventInfo = _fixture.GetCampEventDetails(eventId);
            Assert.IsNotNull(campEventDTO);
            Assert.AreEqual(123, campEventInfo.EventId);
            _campService.VerifyAll();

        }

        [Test]

        public void ShouldCreateContact()
        {
            const string token = "asdfasdf";
            int eventId = 123;
            
            var parentContact = new MpMyContact {Household_ID = 1234567};
            var participantIdResult = new Result<MpRecordID>(true, new MpRecordID { RecordId = 12345});
            var contactId = ContactIdList()[0].RecordId;

            //var minorContactDTO = mockMinorContact();

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _contactService.Setup(m => m.GetMyProfile(token)).Returns(parentContact);
            _campService.Setup(m => m.CreateMinorContact(mockMinorContact(parentContact))).Returns(ContactIdList());
            _campService.Setup(m => m.AddAsCampParticipant(contactId, eventId)).Returns(participantIdResult);
            

            _fixture.SaveCampReservation(MockCampReservationDTO(parentContact), eventId, token);
            _campService.VerifyAll();

        }

        private List<MpCampEvent> MockCampEvents()
        {
            return new List<MpCampEvent>
            {
                new MpCampEvent()
                {
                    EventId = 123,
                    EventTitle = "Camp Event",
                    EventType = "camp",
                    OnlineProductId = 156,
                    ProgramId = 555,
                    RegistrationStartDate = DateTime.Today.AddDays(-3),
                    RegistrationEndDate = DateTime.Today.AddDays(90),
                    StartDate = DateTime.Today.AddDays(150),
                    EndDate = DateTime.Today.AddDays(155)                 
                }
            };
        }

        private CampReservationDTO MockCampReservationDTO(MpMyContact parentContact)
        {
            return new CampReservationDTO()
            {
                FirstName = "firstName",
                LastName = "lastName",
                MiddleName = "middleName",
                PreferredName = "display",
                Gender = 1,
                BirthDate = DateTime.Today.AddYears(-13),
                SchoolAttending = "Primary",
                CrossroadsSite = "Mason",
                CurrentGrade = "5th",
                SchoolAttendingNext = "Secondary"
            };
        }

        private MpMinorContact mockMinorContact(MpMyContact parentContact)
        {
            return new MpMinorContact()
            {
                FirstName = "firstName",
                LastName = "lastName",
                MiddleName = "middleName",
                PreferredName = "prefer",
                Gender = 1,
                BirthDate = DateTime.Today.AddYears(-13),
                HouseholdPositionId = 2,
                HouseholdId = parentContact.Household_ID,
                SchoolAttending = "Primary"
            };
        }

        private List<MpRecordID> ContactIdList()
        {
            return new List<MpRecordID>
            {
                new MpRecordID
                {
                    RecordId = 56789
                }
            };
        }
    }
}
