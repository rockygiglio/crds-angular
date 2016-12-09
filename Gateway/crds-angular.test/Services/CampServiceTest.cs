using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Camp;

using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.test.Helpers;
using Crossroads.Utilities.FunctionalHelpers;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Models.MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Models.Product;

namespace crds_angular.test.Services
{
    public class CampServiceTest
    {
        private ICampService _fixture;
        private Mock<ICampRepository> _campService;
        private Mock<IFormSubmissionRepository> _formSubmissionRepository;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IParticipantRepository> _participantRepository;
        private Mock<IEventRepository> _eventRepository;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IGroupService> _groupService;
        private Mock<IContactRepository> _contactService;
        private Mock<ICongregationRepository> _congregationRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IEventParticipantRepository> _eventParticipantRepository;
        private Mock<IMedicalInformationRepository> _medicalInformationRepository;
        private Mock<IProductRepository> _productRepository;
        private Mock<IInvoiceRepository> _invoiceRepository;
        private Mock<ICommunicationRepository> _communicationRepository;
        private Mock<IPaymentRepository> _paymentRepository;
        private Mock<IObjectAttributeService> _objectAttributeService;

        [SetUp]
        public void SetUp()
        {
            Factories.CampReservationDTO();
            Factories.MpGroupParticipant();

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
            _productRepository = new Mock<IProductRepository>();
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _communicationRepository = new Mock<ICommunicationRepository>();
            _paymentRepository = new Mock<IPaymentRepository>();
            _objectAttributeService = new Mock<IObjectAttributeService>();

            _fixture = new CampService(_campService.Object, 
                                       _formSubmissionRepository.Object, 
                                       _configurationWrapper.Object, 
                                       _participantRepository.Object, 
                                       _eventRepository.Object, 
                                       _apiUserRepository.Object, 
                                       _contactService.Object,
                                       _congregationRepository.Object,
                                       _groupRepository.Object,
                                       _eventParticipantRepository.Object,
                                       _medicalInformationRepository.Object,
                                       _productRepository.Object,
                                       _invoiceRepository.Object,
                                       _communicationRepository.Object,
                                       _paymentRepository.Object,
                                       _objectAttributeService.Object);
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
            var eventParticipant = new MpEventParticipant
            {
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact));
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(123, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(It.IsAny<int>(), It.IsAny<int>())).Returns(eventParticipant);

            var result = _fixture.GetEligibleFamilyMembers(eventId, token);
            Assert.AreEqual(result.Count, 1);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            _eventParticipantRepository.VerifyAll();
        }

        [Test]
        public void shouldCreateNewContactAndCampReservation()
        {
            const int groupId = 123;
            var campReservation = FactoryGirl.NET.FactoryGirl.Build<CampReservationDTO>((res) =>
            {
                res.ContactId = 0;
                res.CurrentGrade = groupId;
            });
            var group = FactoryGirl.NET.FactoryGirl.Build<MpGroupParticipant>((res) => res.GroupId = groupId);
            const string token = "1234";
            const int eventParticipantId = 6;
            const int newEventparticipantId = 0;
            const int eventId = 4;
            const int timeout = 20;
            const int formId = 8;
            const int newContactId = 3;

            var household = new MpMyContact
            {
                Household_ID = 2345
            };

            var contact = new List<MpRecordID>
            {
                new MpRecordID
                {
                    RecordId = newContactId
                }
            };

            var camp = new MpEvent
            {
                EventId = eventId,
                MinutesUntilTimeout = timeout
            };

            var participant = new MpParticipant
            {
                ParticipantId = 2
            };

            var eventParticipant = new MpEventParticipant
            {
                EventParticipantId = newEventparticipantId
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(household);
            _contactService.Setup(m => m.CreateContact(It.IsAny<MpContact>())).Returns(contact);
            _participantRepository.Setup(m => m.GetParticipant(newContactId)).Returns(participant);
            _eventRepository.Setup(m => m.GetEvent(eventId)).Returns(camp);
            _objectAttributeService.Setup(m =>
                                              m.SaveObjectAttributes(newContactId,
                                                                     It.IsAny<Dictionary<int, ObjectAttributeTypeDTO>>(),
                                                                     It.IsAny<Dictionary<int, ObjectSingleAttributeDTO>>(),
                                                                     It.IsAny<MpObjectAttributeConfiguration>()));
            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _groupRepository.Setup(g => g.GetGradeGroupForContact(newContactId, token)).Returns(new Result<MpGroupParticipant>(true, group));
             
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampFormID")).Returns(formId);            
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.PreferredRoommate")).Returns(14);
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(eventId, participant.ParticipantId)).Returns(eventParticipant);

            _eventRepository.Setup(m => m.RegisterInterestedParticipantWithEndDate(
                participant.ParticipantId,
                eventId, 
                It.Is<DateTime>(d => d <= DateTime.Now.AddMinutes(timeout) && d > DateTime.Now.AddMinutes(timeout).AddSeconds(-1))
                ))
                .Returns(eventParticipantId);

            _fixture.SaveCampReservation(campReservation, eventId, token);

            _eventRepository.VerifyAll();
            _participantRepository.VerifyAll();
            _contactService.VerifyAll();
            _configurationWrapper.VerifyAll();
            _formSubmissionRepository.VerifyAll();

        }

        [Test]
        public void shouldUpdateContactAndCreateCampReservation()
        {

            const int groupId = 123;
            const string token = "1234";
            const int eventParticipantId = 6;
            const int eventId = 4;
            const int formId = 8;
            const int contactId = 231;

            var campReservation = FactoryGirl.NET.FactoryGirl.Build<CampReservationDTO>((res) =>
            {
                res.ContactId = contactId;
                res.CurrentGrade = groupId;
            });
            var group = FactoryGirl.NET.FactoryGirl.Build<MpGroupParticipant>((res) => res.GroupId = groupId);
           

            var household = new MpMyContact
            {
                Household_ID = 2345
            };

            var participant = new MpParticipant
            {
                ParticipantId = 2
            };

            var eventParticipant = new MpEventParticipant
            {
                EventParticipantId = eventParticipantId
            };

            var congregation = new MpCongregation
            {
                Name = "mASON"
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(household);
            _congregationRepository.Setup(m => m.GetCongregationById(campReservation.CrossroadsSite)).Returns(congregation);
            _contactService.Setup(m => m.UpdateContact(contactId, It.IsAny<Dictionary<string, object>>()));
            _participantRepository.Setup(m => m.GetParticipant(contactId)).Returns(participant);

            _objectAttributeService.Setup(m =>
                                              m.SaveObjectAttributes(contactId,
                                                                     It.IsAny<Dictionary<int, ObjectAttributeTypeDTO>>(),
                                                                     It.IsAny<Dictionary<int, ObjectSingleAttributeDTO>>(),
                                                                     It.IsAny<MpObjectAttributeConfiguration>()));
            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _groupRepository.Setup(g => g.GetGradeGroupForContact(contactId, token)).Returns(new Result<MpGroupParticipant>(true, group));

            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampFormID")).Returns(formId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear")).Returns(12);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("SummerCampForm.PreferredRoommate")).Returns(14);            
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(eventId, It.IsAny<int>())).Returns(eventParticipant);
            _fixture.SaveCampReservation(campReservation, eventId, token);
            _eventRepository.VerifyAll();
            _participantRepository.VerifyAll();
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
            var eventParticipant = new MpEventParticipant();

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact));
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(123, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(eventId, It.IsAny<int>())).Returns(eventParticipant);

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
            var eventParticipant = new MpEventParticipant();

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact, false, "Adult Child"));            
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(myContact.Contact_ID, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(eventId, It.IsAny<int>())).Returns(eventParticipant);

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
            var eventParticipant = new MpEventParticipant();

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact, false));
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(myContact.Contact_ID, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(eventId, It.IsAny<int>())).Returns(eventParticipant);

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
            var eventParticipant = new MpEventParticipant
            {
                SetupDate = signedUpOn
            };

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact, false, "Adult Child"));
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.IsMemberOfEventGroup(myContact.Contact_ID, eventId, apiToken)).Returns(true);
            _eventParticipantRepository.Setup(m => m.GetEventParticipantEligibility(eventId, It.IsAny<int>())).Returns(eventParticipant);

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
        public void ShouldGetMyCampInfo()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var myContactId = 2187211;
            var myContact = getFakeContact(myContactId);
            var campType = "Camp";
            var camps = new List<MpEvent>
            {
                new MpEvent
                {
                    EventId = 123,
                    EventEndDate = new DateTime(2017, 5, 20),
                    EventStartDate = new DateTime(2017, 5, 15),
                    EventTitle = "Summer Camp",
                    PrimaryContact = new MpContact
                    {
                        ContactId =  90876,
                        EmailAddress = "studentministry@crossroads.com"
                    }
                }
            };

            IEnumerable<MinistryPlatform.Translation.Models.MpParticipant> campers = new List<MinistryPlatform.Translation.Models.MpParticipant>
            {
                new MinistryPlatform.Translation.Models.MpParticipant
                {
                    ContactId = 123,
                    ParticipantId = 77777,
                    EmailAddress = "x@x",
                    DisplayName = "Jony",
                    Nickname = "campell",
                    GroupName = ""
                }
            };
                  


            // Get a family list with only me in it i.e. no family
            var family = getFakeHouseholdMembers(myContact).ToList();

            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _configurationWrapper.Setup(m => m.GetConfigValue("CampEventTypeName")).Returns(campType);
            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(family);
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
           

            _eventRepository.Setup(m => m.GetEvents(campType, apiToken)).Returns(camps);
            _eventRepository.Setup(m => m.EventParticipants(apiToken, camps.First().EventId)).Returns(campers);
            _eventRepository.Setup(m => m.GetEvent(camps.First().EventId)).Returns(camps.First());

            var result = _fixture.GetMyCampInfo(token);
            Assert.AreEqual(result.Count, 1);
            _eventRepository.VerifyAll();
            _contactService.VerifyAll();
        }

        //[Test]
        //public void ShouldSaveMedicalInfo()
        //{
        //    const string token = "theToken";
        //    var contactId = 123;
        //    var medicalInfo = new MedicalInfoDTO
        //    {
        //        InsuranceCompany = "Ins. Co. Name",
        //        PhysicianName = "bobby",
        //        PhysicianPhone = "123-4567",
        //        PolicyHolder = "your mom"
        //    };

        //    var mpMedInfo = new MpMedicalInformation
        //    {
        //        InsuranceCompany = "Ins. Co. Name",
        //        PhysicianName = "bobby",
        //        PhysicianPhone = "123-4567",
        //        PolicyHolder = "your mom"
        //    };

        //    var myContact = new MpMyContact
        //    {
        //        Contact_ID = 999999,
        //        Household_ID = 77777
        //    };

        //    var myHousehold = new List<MpHouseholdMember>
        //    {
        //        new MpHouseholdMember
        //        {
        //            ContactId = contactId
        //        }
        //    };

        //    var otherHousehold = new List<MpHouseholdMember>
        //    {
        //        new MpHouseholdMember
        //        {
        //            ContactId = 5555555
        //        }
        //    };

        //    _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
        //    _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(myHousehold);
        //    _contactService.Setup(m => m.GetOtherHouseholdMembers(myContact.Contact_ID)).Returns(otherHousehold);

        //    _medicalInformationRepository.Setup(m => m.SaveMedicalInformation(mpMedInfo, 123));

        //    Assert.DoesNotThrow(() =>_fixture.SaveCamperMedicalInfo(medicalInfo, contactId, token));
        //}

        [Test]
        public void shouldGetCamperInfo()
        {
            const int eventId = 123;
            const string token = "asdf";
            const string apiToken = "apiToken";
            const int contactId = 2187211;
            var myContact = getFakeContact(contactId);
            const int childContactId = 123456789;

            var participant = new Result<MpGroupParticipant>(true, new MpGroupParticipant() {GroupName = "6th Grade", GroupId = 34});
            var attributesDto = new ObjectAllAttributesDTO();

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
            _objectAttributeService.Setup(m => m.GetObjectAttributes(apiToken, contactId, It.IsAny<MpObjectAttributeConfiguration>())).Returns(attributesDto);

            var result = _fixture.GetCamperInfo(token, eventId, contactId);
            Assert.AreEqual(result.ContactId, 2187211);
            Assert.AreEqual(result.CurrentGrade, 34);
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
            var attributesDto = new ObjectAllAttributesDTO();

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
            _objectAttributeService.Setup(m => m.GetObjectAttributes(apiToken, contactId, It.IsAny<MpObjectAttributeConfiguration>())).Returns(attributesDto);

            var result = _fixture.GetCamperInfo(token, eventId, contactId);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
            Assert.AreEqual(result.ContactId, 2187211);
            Assert.AreEqual(0, result.CurrentGrade);
            
        }

        [Test]
        public void ShouldGetProductInfo()
        {
            var me = this.getFakeContact(1);
            const int eventId = 1234;
            const string token = "1aaaa";
            var product = new MpProduct
            {
                ProductId = 111,
                BasePrice = 1000,
                DepositPrice = 200,
                ProductName = "Hipster Beard Wax"
            };

            var mpevent = new MpEvent
            {
                EventId = 999,
                EventTitle = "Hipster Beard Training",
                EventType = "event-type-100",
                EventStartDate = new DateTime(2017, 3, 28, 8, 30, 0),
                EventEndDate = new DateTime(2017, 4, 28, 8, 30, 0),
                PrimaryContact = new MpContact { ContactId = 12345, EmailAddress = "thedude@beautifulbeards.com" },
                ParentEventId = 6543219,
                CongregationId = 2,
                ReminderDaysPriorId = 2,
                RegistrationStartDate = new DateTime(2017, 1, 1, 8, 30, 0),
                RegistrationEndDate = new DateTime(2017, 3, 15, 8, 30, 0),
                Cancelled = false
            };

            var mpprodoption1 = new MpProductOptionPrice
            {
                ProductOptionPriceId = 1,
                OptionTitle = "Option 1",
                OptionPrice = 20,
                DaysOutToHide = 90
            };

            var mpprodoption2 = new MpProductOptionPrice
            {
                ProductOptionPriceId = 1,
                OptionTitle = "Option 1",
                OptionPrice = 20,
                DaysOutToHide = 90
            };

            var mpoptionlist = new List<MpProductOptionPrice>() {mpprodoption1, mpprodoption2};
            int contactid = 12345;

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(me);
            _eventRepository.Setup(m => m.GetEvent(eventId)).Returns(mpevent);
            _productRepository.Setup(m => m.GetProductForEvent(eventId)).Returns(product);
            _productRepository.Setup(m => m.GetProductOptionPricesForProduct(product.ProductId)).Returns(mpoptionlist);
            _formSubmissionRepository.Setup(m => m.GetFormResponseAnswer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns("true");
            _invoiceRepository.Setup(m => m.GetInvoiceDetailsForProductAndCamperAndContact(product.ProductId, contactid, me.Contact_ID))
                .Returns(new Result<MpInvoiceDetail>(true, new MpInvoiceDetail() {InvoiceId = 1234}));

            var result = _fixture.GetCampProductDetails(eventId,contactid, token);
            Assert.IsTrue(result.Options.Count == 2);
            Assert.IsTrue(result.ProductId == 111);
        }

        [Test]
        public void shouldSendConfirmationEmail()
        {
            const int eventId = 1234554;
            const string token = "letmein";
            const int paymentId = 98789;
            const int invoiceId = 8767;
            const int contactId = 67676;
            const int templateId = 12345;
            const string baseUrl = "localhost:3000";

            var startDate = DateTime.Now;
            var endDate = new DateTime(2017, 8, 20); 

            var mpEvent = fakeEvent(eventId, startDate, endDate, contactId);
            var mpTemplate = fakeTemplate();
            var mpPayment = fakePayments(contactId, paymentId);

            _configurationWrapper.Setup(m => m.GetConfigValue("BaseUrl")).Returns(baseUrl);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("CampConfirmationEmailTemplate")).Returns(templateId);

            // get the event and the message Id
            _eventRepository.Setup(m => m.GetEvent(eventId)).Returns(mpEvent);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(invoiceId)).Returns(mpPayment);
            _communicationRepository.Setup(m => m.GetTemplateAsCommunication(templateId, contactId, "some@email2.com", contactId, "some@email2.com", contactId, "Ok@email.com", It.IsAny<Dictionary<string,object>>()));
            _contactService.Setup(m => m.GetContactById(mpPayment.First().ContactId)).Returns(new MpMyContact() { Contact_ID = contactId, Email_Address = "some@email2.com", First_Name = "Natt", Last_Name = "last"});
            _contactService.Setup(m => m.GetContactById(mpPayment.First().ContactId)).Returns(new MpMyContact() {Contact_ID = contactId, Email_Address = "some@email.com"});
          
            _communicationRepository.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Returns(1);

            var resp = _fixture.SendCampConfirmationEmail(eventId, invoiceId, paymentId, token);
           
            _paymentRepository.VerifyAll();
            _contactService.VerifyAll();
            _configurationWrapper.VerifyAll();
            _eventRepository.VerifyAll();

            Assert.IsTrue(resp);
        }

        private static List<MpPayment> fakePayments(int contactId, int paymentId)
        {
            return new List<MpPayment>
            {
                new MpPayment()
                {
                    ContactId = contactId,
                    PaymentTotal = 900,
                    PaymentId = paymentId
                },
                new MpPayment()
                {
                    ContactId = contactId,
                    PaymentTotal = 100,
                    PaymentId = 8989
                }
            };
        }

        private static MpEvent fakeEvent(int eventId, DateTime startDate, DateTime endDate, int contactId)
        {
            return new MpEvent()
            {
                EventId = eventId,
                EventTitle = "Awesome Camp",
                EventStartDate = startDate,
                EventEndDate = endDate,
                RegistrationURL = "https://blah.com/some/url",
                PrimaryContactId = contactId,
                PrimaryContact = new MpContact()
                {
                    ContactId = contactId,
                    EmailAddress = "some@email2.com"
                }
            };
        }

        private static MpMessageTemplate fakeTemplate()
        {
            return new MpMessageTemplate()
            {
                Body = "Ok Body",
                FromContactId = 1234,
                FromEmailAddress = "some@email.com",
                ReplyToContactId = 1234,
                ReplyToEmailAddress = "some@email.com",
                Subject = "RE: Your Brains"
            };

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
        //private CampReservationDTO MockCampReservationDTO()
        //{
        //    return new CampReservationDTO
        //    {
        //        ContactId = 0,
        //        FirstName = "Jon",
        //        LastName = "Horner",
        //        MiddleName = "",
        //        BirthDate = new DateTime(2006, 04, 03) + "",
        //        Gender = 1,
        //        PreferredName = "Jon",
        //        SchoolAttending = "Mason",
        //        CurrentGrade = 3,
        //        SchoolAttendingNext = "Mason",
        //        CrossroadsSite = 3,
        //        RoomMate = ""
        //    };
        //}
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
                },
                 new CampEmergencyContactDTO
                {
                    FirstName = "Bob",
                    LastName = "Horner",
                    Email = "lknair@gmail.com",
                    MobileNumber = "123456780",
                    Relationship = "friend",
                    PrimaryEmergencyContact = false
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
                CurrentGrade = 3,
                SchoolAttendingNext = "Mason",
                CrossroadsSite = 3,
                RoomMate = ""
            };
        }
    }
}
