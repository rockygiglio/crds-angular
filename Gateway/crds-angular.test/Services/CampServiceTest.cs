using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.FunctionalHelpers;
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
        private readonly Mock<IMedicalInformationRepository> _medicalInformationRepository;

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
            _medicalInformationRepository = new Mock<IMedicalInformationRepository>();
            _fixture = new CampService(_campService.Object, 
                                       _formSubmissionRepository.Object, 
                                       _configurationWrapper.Object, 
                                       _participantRepository.Object, 
                                       _eventRepository.Object, 
                                       _apiUserRepository.Object, 
                                       _groupService.Object,
                                       _contactService.Object,
                                       _congregationRepository.Object,
                                       _groupRepository.Object,
                                       _eventParticipantRepository.Object,
                                       _medicalInformationRepository.Object);
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

            var newEventparticipantId = 0;

            var formId = 8;

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(household);
            _contactService.Setup(m => m.CreateContact(It.IsAny<MpContact>())).Returns(contact);
            _participantRepository.Setup(m => m.GetParticipant(contact[0].RecordId)).Returns(participant);
            _eventRepository.Setup(m => m.RegisterParticipantForEvent(participant.ParticipantId, eventId, 0, 0)).Returns(eventParticipantId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampFormID")).Returns(formId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.CurrentGrade")).Returns(10);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.PreferredRoommate")).Returns(14);
            _eventRepository.Setup(m => m.GetEventParticipantRecordId(eventId, participant.ParticipantId)).Returns(newEventparticipantId);

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
        public void shouldGetCampFamilyNotHeadNotSignedUp()
        {
            const string token = "asdfasdfasdfasdf";
            const string apiToken = "apiToken";
            const int myContactId = 2187211;
            const int eventId = 5433;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact, false, "Adult Child"));            
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(myContact.Contact_ID, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.EventParticipantSignupDate(myContact.Contact_ID, eventId, apiToken)).Returns((DateTime?)null);

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 1);
            Assert.IsNull(result.FirstOrDefault().SignedUpDate);
            Assert.IsTrue(result.FirstOrDefault().IsEligible);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            _eventParticipantRepository.VerifyAll();
        }

        [Test]
        public void shouldGetCampFamilyHouseholdPositionNotSet()
        {
            const string token = "asdfasdfasdfasdf";
            const string apiToken = "apiToken";
            const int myContactId = 2187211;
            const int eventId = 5433;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact, false));
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(myContact.Contact_ID, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.EventParticipantSignupDate(myContact.Contact_ID, eventId, apiToken)).Returns((DateTime?)null);

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 1);
            Assert.IsNull(result.FirstOrDefault().SignedUpDate);
            Assert.IsTrue(result.FirstOrDefault().IsEligible);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            _eventParticipantRepository.VerifyAll();
        }

        [Test]
        public void shouldGetCampFamilyNotHeadSignedUp()
        {
            const string token = "asdfasdfasdfasdf";
            const string apiToken = "apiToken";
            const int myContactId = 2187211;
            const int eventId = 5433;
            var myContact = getFakeContact(myContactId);

            var signedUpOn = DateTime.Now;

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact, false, "Adult Child"));
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(myContact.Contact_ID, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.EventParticipantSignupDate(myContact.Contact_ID, eventId, apiToken)).Returns(signedUpOn);

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 1);
            Assert.IsNotNull(result.First().SignedUpDate);
            Assert.AreEqual(signedUpOn, result.First().SignedUpDate);
            Assert.IsTrue(result.First().IsEligible);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            _eventParticipantRepository.VerifyAll();
        }

        [Test]
        public void shouldSaveEmergencyContact()
        {
            var contactId = 12345;
            var token = "444";
            var formId = 10;
            var eventId = 6789;
            var participant = new MpParticipant
            {
                ParticipantId = 2
            };
            var myContact = new MpMyContact
            {
                Contact_ID = 999999,
                Household_ID = 77777
            };

            var myHousehold = new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    ContactId = contactId
                }
            };

            var otherHousehold = new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    ContactId = 5555555
                }
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(myHousehold);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContact.Contact_ID)).Returns(otherHousehold);
            _participantRepository.Setup(m => m.GetParticipant(contactId)).Returns(participant);
            _eventRepository.Setup(m => m.SafeRegisterParticipant(eventId, participant.ParticipantId, 0, 0));
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampFormID")).Returns(formId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.EmergencyContactFirstName")).Returns(10);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.EmergencyContactLastName")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.EmergencyContactMobilePhone")).Returns(14);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.EmergencyContactEmail")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.EmergencyContactRelationship")).Returns(14);

            _fixture.SaveCamperEmergencyContactInfo(MockCampEmergencyContactDTO(), eventId, contactId, token);
            _configurationWrapper.VerifyAll();
            _participantRepository.VerifyAll();
            _eventRepository.VerifyAll();
        }

        [Test]
        public void shouldGetEmptyListOfFamilyMembers()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var myContactId = 2187211;
            var eventId = 1234;
            var myContact = getFakeContact(myContactId);

            // Get a family list with only me in it i.e. no family
            var family = getFakeHouseholdMembers(myContact).Where(member => member.ContactId == myContactId).ToList();

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(family);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);            

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 0);
            _contactService.VerifyAll();
        }

        [Test]
        public void ShouldSaveMedicalInfo()
        {
            const string token = "theToken";
            var contactId = 123;
            var medicalInfo = new MedicalInfoDTO
            {
                InsuranceCompany = "Ins. Co. Name",
                PhysicianName = "bobby",
                PhysicianPhone = "123-4567",
                PolicyHolder = "your mom"
            };

            var mpMedInfo = new MpMedicalInformation
            {
                InsuranceCompany = "Ins. Co. Name",
                PhysicianName = "bobby",
                PhysicianPhone = "123-4567",
                PolicyHolder = "your mom"
            };

            var myContact = new MpMyContact
            {
                Contact_ID = 999999,
                Household_ID = 77777
            };

            var myHousehold = new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    ContactId = contactId
                }
            };

            var otherHousehold = new List<MpHouseholdMember>
            {
                new MpHouseholdMember
                {
                    ContactId = 5555555
                }
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(myHousehold);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContact.Contact_ID)).Returns(otherHousehold);

            _medicalInformationRepository.Setup(m => m.SaveMedicalInformation(mpMedInfo, 123));
           
            Assert.DoesNotThrow(() =>_fixture.SaveCamperMedicalInfo(medicalInfo, contactId, token));
        }

        [Test]
        public void shouldGetCamperInfo()
        {
            const int eventId = 123;
            const string token = "asdf";
            const string apiToken = "apiToken";
            const int contactId = 2187211;
            var myContact = getFakeContact(contactId);
            const int childContactId = 123456789;

            var participant = new Result<MpGroupParticipant>(true, new MpGroupParticipant() {GroupName = "6th Grade"});
            
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
                    ContactId = childContactId
                }

            };
            _contactService.Setup(m => m.GetMyProfile(token)).Returns(loggedInContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(loggedInContact.Household_ID)).Returns(householdfamily);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(loggedInContact.Contact_ID)).Returns(otherHouseholdfamily);
            _contactService.Setup(m => m.GetContactById(contactId)).Returns(myContact);
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.GetGradeGroupForContact(contactId, apiToken)).Returns(participant);

            var result = _fixture.GetCamperInfo(token, eventId, contactId);
            Assert.AreEqual(result.ContactId, 2187211);
            Assert.AreEqual(result.CurrentGrade, "6th Grade");
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
        }

        [Test]
        public void shouldGetCamperInfoNoGrade()
        {
            const int eventId = 123;
            const string token = "asdf";
            const string apiToken = "apiToken";
            const int contactId = 2187211;
            var myContact = getFakeContact(contactId);
            const int childContactId = 123456789;

            var participant = new Result<MpGroupParticipant>(false, "some error message");

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
                    ContactId = childContactId
                }

            };
            _contactService.Setup(m => m.GetMyProfile(token)).Returns(loggedInContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(loggedInContact.Household_ID)).Returns(householdfamily);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(loggedInContact.Contact_ID)).Returns(otherHouseholdfamily);
            _contactService.Setup(m => m.GetContactById(contactId)).Returns(myContact);
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.GetGradeGroupForContact(contactId, apiToken)).Returns(participant);

            var result = _fixture.GetCamperInfo(token, eventId, contactId);
            Assert.AreEqual(result.ContactId, 2187211);
            Assert.IsNull(result.CurrentGrade);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
        }

        private List<MpHouseholdMember> getFakeHouseholdMembers(MpMyContact me, bool isHead = true, string positionIfNotHead = null)
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
                    HouseholdPosition = isHead ? "Head of Household" : positionIfNotHead,
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
        private List<CampEmergencyContactDTO> MockCampEmergencyContactDTO()
        {
            return new List<CampEmergencyContactDTO>
            {
                new CampEmergencyContactDTO
                {
                    FirstName = "Jon",
                    LastName = "Horner",
                    Email = "lknair@gmail.com",
                    MobileNumber = "123456789",
                    Relationship = "friend",
                    PrimaryEmergencyContact = true
                }
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
