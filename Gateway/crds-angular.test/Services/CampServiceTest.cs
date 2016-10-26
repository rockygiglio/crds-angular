using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Models.MinistryPlatform.Translation.Models;

namespace crds_angular.test.Services
{
    public class CampServiceTest
    {
        private readonly ICampService _fixture;
        private readonly Mock<ICampRepository> _campService;
        private readonly Mock<IFormSubmissionRepository> _formSubmissionRepository;
        private readonly Mock<IConfigurationWrapper> _configurationWrapper;
        private readonly Mock<IParticipantRepository> _participantRepository;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IApiUserRepository> _apiUserRepository;
        private readonly Mock<IGroupService> _groupService;
        private readonly Mock<IContactRepository> _contactService;
        private readonly Mock<ICongregationRepository> _congregationRepository;
        private readonly Mock<IGroupRepository> _groupRepository;
        private readonly Mock<IEventParticipantRepository> _eventParticipantRepository;

        public CampServiceTest()
        {
            _contactService = new Mock<IContactRepository>();
            _campService = new Mock<ICampRepository>();
            _formSubmissionRepository = new Mock<IFormSubmissionRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _participantRepository = new Mock<IParticipantRepository>();
            _eventRepository = new Mock<IEventRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _groupService = new Mock<IGroupService>();
            _contactService = new Mock<IContactRepository>();
            _congregationRepository = new Mock<ICongregationRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _eventParticipantRepository = new Mock<IEventParticipantRepository>();
            _fixture = new CampService(_campService.Object, _formSubmissionRepository.Object, _configurationWrapper.Object, _participantRepository.Object, _eventRepository.Object, _apiUserRepository.Object, _groupService.Object, _contactService.Object, _congregationRepository.Object, _groupRepository.Object, _eventParticipantRepository.Object);
        }

        [Test]
        public void shouldGetCampFamilyMembers()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var myContactId = 2187211;
            var signedUpDate = DateTime.Now;
            var eventId = 5433;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact));
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(123, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.EventParticipantSignupDate(123, eventId, apiToken)).Returns(signedUpDate);

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 1);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            _eventParticipantRepository.VerifyAll();
        }

        [Test]
        public void shouldCreateNewContactAndCampReservation()
        {

            var campReservation = MockCampReservationDTO();
            var token = "1234";
            var household = new MpMyContact
            {
                Household_ID = 2345
            };

            var contact = new List<MpRecordID>
            {
                new MpRecordID
                {
                    RecordId = 3
                }
            };

            var eventId = 4;

            var participant = new MpParticipant
            {
                ParticipantId = 2
            };

            var eventParticipantId = 6;
            var formId = 8;

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(household);

            var minorContact = new MpContact
            {
                ContactId = 0,
                FirstName = campReservation.FirstName,
                LastName = campReservation.LastName,
                MiddleName = campReservation.MiddleName,
                BirthDate = Convert.ToDateTime(campReservation.BirthDate),
                Gender = campReservation.Gender,
                PreferredName = campReservation.PreferredName,
                Nickname = campReservation.PreferredName ?? campReservation.FirstName,
                SchoolAttending = campReservation.SchoolAttending,
                HouseholdId = household.Household_ID,
                HouseholdPositionId = 2
            };

            _contactService.Setup(m => m.CreateContact(It.IsAny<MpContact>())).Returns(contact);
            _participantRepository.Setup(m => m.GetParticipant(contact[0].RecordId)).Returns(participant);
            _eventRepository.Setup(m => m.RegisterParticipantForEvent(participant.ParticipantId, eventId, 0, 0)).Returns(eventParticipantId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampFormID")).Returns(formId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.CurrentGrade")).Returns(10);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.PreferredRoommate")).Returns(14);
            _eventRepository.Setup(m => m.GetEventParticipantRecordId(eventId, participant.ParticipantId)).Returns(eventParticipantId);

            _fixture.SaveCampReservation(MockCampReservationDTO(), eventId, token);
            _eventRepository.VerifyAll();
            _participantRepository.VerifyAll();
            _contactService.VerifyAll();
            _configurationWrapper.VerifyAll();
            _formSubmissionRepository.VerifyAll();

        }

        [Test]
        public void shouldUpdateContactAndCreateCampReservation()
        {

            var token = "1234";

            var household = new MpMyContact
            {
                Household_ID = 2345
            };

            var eventId = 4;

            var participant = new MpParticipant
            {
                ParticipantId = 2
            };

            var eventParticipantId = 6;
            var formId = 8;
            var congregation = new MpCongregation
            {
                Name = "mASON"
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(household);

            _participantRepository.Setup(m => m.GetParticipant(Convert.ToInt32(MockCampReservationDTOwithContactId().ContactId))).Returns(participant);
            _eventRepository.Setup(m => m.RegisterParticipantForEvent(participant.ParticipantId, eventId, 0, 0)).Returns(eventParticipantId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampFormID")).Returns(formId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.CurrentGrade")).Returns(10);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.PreferredRoommate")).Returns(14);
            _congregationRepository.Setup(m => m.GetCongregationById(MockCampReservationDTOwithContactId().CrossroadsSite)).Returns(congregation);
            _eventRepository.Setup(m => m.GetEventParticipantRecordId(eventId, participant.ParticipantId)).Returns(eventParticipantId);
            _fixture.SaveCampReservation(MockCampReservationDTOwithContactId(), eventId, token);
            _eventRepository.VerifyAll();
            _participantRepository.VerifyAll();
            _contactService.VerifyAll();
            _configurationWrapper.VerifyAll();
            _formSubmissionRepository.VerifyAll();

        }

        [Test]
        public void shouldGetCampFamilyMembersNotSignedUp()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var myContactId = 2187211;
            var signedUpDate = DateTime.Now;
            var eventId = 5433;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact));
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(123, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.EventParticipantSignupDate(123, eventId, apiToken)).Returns((DateTime?)null);

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 1);
            Assert.IsNull(result.FirstOrDefault().SignedUpDate);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            _eventParticipantRepository.VerifyAll();
        }

        [Test]
        public void shouldGetEmptyListOfFamilyMembers()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var myContactId = 2187211;
            var eventId = 1234;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(new List<MpHouseholdMember>());
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);            

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 0);
            _contactService.VerifyAll();
        }

        [Test]
        public void shouldGetCamperInfo()
        {
            var eventId = 123;
            var token = "asdf";
            var contactId = 2187211;
            var myContact = getFakeContact(contactId);
            var gradeGroupId = 5;
            var participant = new MpParticipant
            {
                ParticipantId = 9876
            };
            var gradeGroupList = new List<GroupDTO>
            {
                new GroupDTO
                {
                    GroupName = "6th Grade"
                }
            };
            var loggedInContact = new MpMyContact
            {
                Contact_ID = 56789,
                Household_ID = 10000
            };
            var householdfamily = new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    ContactId = contactId
                }

            };
            var otherHouseholdfamily = new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    ContactId = 123456789
                }

            };
            _contactService.Setup(m => m.GetMyProfile(token)).Returns(loggedInContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(loggedInContact.Household_ID)).Returns(householdfamily);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(loggedInContact.Contact_ID)).Returns(otherHouseholdfamily);
            _contactService.Setup(m => m.GetContactById(contactId)).Returns(myContact);
            _participantRepository.Setup(m => m.GetParticipant(contactId)).Returns(participant);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("AgeorGradeGroupType")).Returns(gradeGroupId);
            _groupService.Setup(m => m.GetGroupsByTypeForParticipant(token, participant.ParticipantId, gradeGroupId))
                .Returns(gradeGroupList);

            var result = _fixture.GetCamperInfo(token, eventId, contactId);
            Assert.AreEqual(result.ContactId, 2187211);
            _contactService.VerifyAll();
            _congregationRepository.VerifyAll();
            _participantRepository.VerifyAll();
        }

        private List<MpHouseholdMember> getFakeHouseholdMembers(MpMyContact me)
        {
            return new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    Age = 10,
                    ContactId = 123,
                    DateOfBirth = new DateTime(2006, 04, 03),
                    HouseholdPosition = "Minor Child",
                    FirstName = "Miles",
                    LastName = "Sil",
                    Nickname = "Miles"
                },
                new MpHouseholdMember
                {
                    Age = me.Age,
                    ContactId = me.Contact_ID,
                    FirstName = me.First_Name,
                    LastName = me.Last_Name,
                    HouseholdPosition = "Head of Household",
                    Nickname = "matt"
                }
            };
        }

        private MpMyContact getFakeContact(int contactId)
        {
            return new MpMyContact()
            {
                First_Name = "Jon",
                Last_Name = "Baker",
                Middle_Name = "",
                Display_Name = "Jon",
                Current_School = "mASON",
                Gender_ID = 1,
                Address_ID = 12,
                Address_Line_1 = "adsfasdf",
                Age = 36,
                Contact_ID = contactId,
                City = "Cincinnati",
                Date_Of_Birth = DateTime.Now.ToLongDateString(),
                Congregation_ID = 1,
                Household_ID = 23               
            };
        }
        private CampReservationDTO MockCampReservationDTO()
        {
            return new CampReservationDTO
            {
                ContactId = 0,
                FirstName = "Jon",
                LastName = "Horner",
                MiddleName = "",
                BirthDate = new DateTime(2006, 04, 03) + "",
                Gender = 1,
                PreferredName = "Jon",
                SchoolAttending = "Mason",
                CurrentGrade = "6th Grade",
                SchoolAttendingNext = "Mason",
                CrossroadsSite = 3,
                RoomMate = ""
            };
        }
        private CampReservationDTO MockCampReservationDTOwithContactId()
        {
            return new CampReservationDTO
            {
                ContactId = 12345,
                FirstName = "Jon",
                LastName = "Horner",
                MiddleName = "",
                BirthDate = new DateTime(2006, 04, 03) + "",
                Gender = 1,
                PreferredName = "Jon",
                SchoolAttending = "Mason",
                CurrentGrade = "6th Grade",
                SchoolAttendingNext = "Mason",
                CrossroadsSite = 3,
                RoomMate = ""
            };
        }
    }
}
