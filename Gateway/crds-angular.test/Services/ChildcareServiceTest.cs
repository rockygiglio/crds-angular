using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using crds_angular.Util.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using IEventRepository = MinistryPlatform.Translation.Repositories.Interfaces.IEventRepository;
using Participant = MinistryPlatform.Translation.Models.People.Participant;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class ChildcareServiceTest
    {
        private Mock<IEventParticipantRepository> _eventParticipantService;
        private Mock<ICommunicationRepository> _communicationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IContactRepository> _contactService;
        private Mock<IEventRepository> _eventService;
        private Mock<IParticipantRepository> _participantService;
        private Mock<IServeService> _serveService;
        private Mock<IDateTime> _dateTimeWrapper;
        // Interfaces.IEventService crdsEventService, IApiUserService apiUserService
        private Mock<crds_angular.Services.Interfaces.IEventService> _crdsEventService;
        private Mock<IApiUserRepository> _apiUserService;
        private Mock<IChildcareRequestRepository> _childcareRequestService;
        private Mock<IGroupService> _groupService;

        private ChildcareService _fixture;

        [SetUp]
        public void SetUp()
        {
            _eventParticipantService = new Mock<IEventParticipantRepository>();
            _communicationService = new Mock<ICommunicationRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _contactService = new Mock<IContactRepository>();
            _eventService = new Mock<IEventRepository>();
            _participantService = new Mock<IParticipantRepository>();
            _serveService = new Mock<IServeService>();
            _dateTimeWrapper = new Mock<IDateTime>();
            _crdsEventService = new Mock<crds_angular.Services.Interfaces.IEventService>();
            _apiUserService = new Mock<IApiUserRepository>();
            _childcareRequestService = new Mock<IChildcareRequestRepository>();
            _groupService = new Mock<IGroupService>();

            _fixture = new ChildcareService(_eventParticipantService.Object,
                                            _communicationService.Object,
                                            _configurationWrapper.Object,
                                            _contactService.Object,
                                            _eventService.Object,
                                            _participantService.Object,
                                            _serveService.Object,
                                            _dateTimeWrapper.Object,
                                            _apiUserService.Object, _crdsEventService.Object, _childcareRequestService.Object, _groupService.Object);
        }

        [Test]
        public void GetMyKidsForChildcare()
        {
            var mockDateTime = new DateTime(2015, 5, 29);
            _dateTimeWrapper.Setup(m => m.Today).Returns(mockDateTime);

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>())).Returns(MockFamily());

            _configurationWrapper.Setup(m => m.GetConfigIntValue("MaxAgeWithoutGrade")).Returns(8);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("MaxGradeForChildcare")).Returns(5);

            var x = _fixture.MyChildren("fake-token");

            _serveService.VerifyAll();
            Assert.AreEqual(2, x.Count);
        }

        private static List<FamilyMember> MockFamily()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Husband",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "tmaddox33mp1@gmail.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 12,
                    HighSchoolGraduationYear = 2021
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 8,
                    HighSchoolGraduationYear = 2025
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child2",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 7
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child2",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 9
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 23
                }
            };
        }

        [Test, TestCaseSource("TestCases")]
        public void CalcGrade(int expectedGrade, int graduationYear, DateTime mockDateTime)
        {
            _dateTimeWrapper.Setup(m => m.Today).Returns(mockDateTime);

            var grade = _fixture.SchoolGrade(graduationYear);

            Assert.AreEqual(expectedGrade, grade);
        }

        private static readonly object[] TestCases =
        {
            new object[] {0, 2014, new DateTime(2015, 5, 29)},
            new object[] {2, 2025, new DateTime(2015, 5, 29)},
            new object[] {3, 2025, new DateTime(2015, 11, 10)},
            new object[] {0, 2035, new DateTime(2015, 5, 29)}
        };

        [Test]
        public void ShouldSendChildcareRequestNotification()
        {
            var startDate = new DateTime(2016, 5, 26);
            var endDate = new DateTime(2016, 7, 2);

            var notificationTemplateId = 0985627;
            var defaultAuthorId = 9087345;

            var template = new MpMessageTemplate()
            {
                Body = "Some long string of text",
                Subject = "A subject"
            };

            var request = new MpChildcareRequestEmail()
            {
                RequestId = 1,
                RequesterEmail = "lakshmi@lak.shmi",
                ChildcareContactEmail = "florencechildcare@crossroads.net",
                ChildcareContactId = 12,
                ChildcareSession = "Monday, 9am - 12pm",
                CongregationName = "Florence",
                EndDate = endDate,
                StartDate = startDate,
                EstimatedChildren = 2,
                Frequency = "once",
                RequesterLastName = "Nair",
                RequesterNickname = "Lak",
                Requester = "Nair, Lak",
                RequesterId = 432,
                GroupName = "FI 101"
            };

            var mergeData = new Dictionary<string, object>
            {
                {"Requester", request.Requester},
                {"Nickname", request.RequesterNickname},
                {"LastName", request.RequesterLastName},
                {"Group", request.GroupName},
                {"Site", request.CongregationName},
                {"StartDate", (request.StartDate).ToShortDateString()},
                {"EndDate", (request.EndDate).ToShortDateString()},
                {"ChildcareSession", request.ChildcareSession},
                {"RequestId", request.RequestId},
                {"Base_Url", "https://localhost:3000"}
            };

            var communication = new MpCommunication
            {
                TemplateId = 0,
                DomainId = 0,
                AuthorUserId = defaultAuthorId,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new MpContact { ContactId = request.RequesterId, EmailAddress = request.RequesterEmail },
                ReplyToContact = new MpContact { ContactId = request.RequesterId, EmailAddress = request.RequesterEmail },
                ToContacts = new List<MpContact> { new MpContact { ContactId = request.ChildcareContactId, EmailAddress = request.ChildcareContactEmail } },
                MergeData = mergeData
            };

            _configurationWrapper.Setup(m => m.GetConfigIntValue("ChildcareRequestNotificationTemplate")).Returns(notificationTemplateId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("DefaultUserAuthorId")).Returns(defaultAuthorId);
            _communicationService.Setup(m => m.GetTemplate(notificationTemplateId)).Returns(template);
            _configurationWrapper.Setup(m => m.GetConfigValue("BaseMPUrl")).Returns("https://localhost:3000");
            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Verifiable();

            _fixture.SendChildcareRequestNotification(request);

            
            _communicationService.VerifyAll();

        }

        [Test]
        public void SendTwoRsvpEmails()
        {
            const int daysBefore = 999;
            const int emailTemplateId = 77;
            var participants = new List<MpEventParticipant>
            {
                new MpEventParticipant
                {
                    ParticipantId = 1,
                    EventId = 123,
                    ContactId = 987654
                },
                new MpEventParticipant
                {
                    ParticipantId = 2,
                    EventId = 456,
                    ContactId = 456123
                }
            };

            var mockPrimaryContact = new MpContact
            {
                ContactId = 98765,
                EmailAddress = "wonder-woman@ip.com"
            };

            var defaultContact = new MpMyContact
            {
                Contact_ID = 123456,
                Email_Address = "gmail@gmail.com"
            };

            var mockEvent1 = new MpEvent {EventType = "Childcare", PrimaryContact = mockPrimaryContact};
            var mockEvent2 = new MpEvent {EventType = "DoggieDaycare", PrimaryContact = mockPrimaryContact};
            var mockEvents = new List<MpEvent> {mockEvent1, mockEvent2};

            _configurationWrapper.Setup(m => m.GetConfigIntValue("NumberOfDaysBeforeEventToSend")).Returns(daysBefore);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("ChildcareRequestTemplate")).Returns(emailTemplateId);
            _communicationService.Setup(m => m.GetTemplate(emailTemplateId)).Returns(new MpMessageTemplate());            
            _eventParticipantService.Setup(m => m.GetChildCareParticipants(daysBefore)).Returns(participants);
            _communicationService.Setup(m => m.SendMessage(It.IsAny<MpCommunication>(), false)).Verifiable();

            var kids = new List<Participant> { new Participant { ContactId = 456321987 } };
            _crdsEventService.Setup(m => m.EventParticpants(987654321, It.IsAny<string>())).Returns(kids);
            var mockChildcareEvent = new MpEvent {EventId = 987654321};
            var mockContact = new MpContact
            {
                ContactId = 8888888,
                EmailAddress = "sometest@test.com"
            };
            mockChildcareEvent.PrimaryContact = mockContact;
            _crdsEventService.Setup(m => m.GetChildcareEvent(participants[0].EventId)).Returns(mockChildcareEvent);
            _crdsEventService.Setup(m => m.GetChildcareEvent(participants[1].EventId)).Returns(mockChildcareEvent);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("DefaultContactEmailId")).Returns(1234);
            _contactService.Setup(mocked => mocked.GetContactById(1234)).Returns(defaultContact); 
            var myKids = new List<Participant>();
            _crdsEventService.Setup(m => m.MyChildrenParticipants(987654, kids, It.IsAny<string>())).Returns(myKids);

            _fixture.SendRequestForRsvp();

            _configurationWrapper.VerifyAll();
            _communicationService.VerifyAll();
            _contactService.VerifyAll();
            _eventParticipantService.VerifyAll();
            _communicationService.VerifyAll();
            _communicationService.Verify(m => m.SendMessage(It.IsAny<MpCommunication>(), false), Times.Exactly(2));
            _eventService.VerifyAll();
        }

        [Test]
        public void GetHeadsOfHousehold()
        {
            const int householdId = 1234;
            const int contactId = 4321;

            _contactService.Setup(m => m.GetHouseholdFamilyMembers(householdId))
                .Returns(
                    new List<MpHouseholdMember>()
                    {
                        new MpHouseholdMember() { Age = 36, ContactId = contactId, DateOfBirth = new DateTime(1980, 2, 21), FirstName = "Matt", LastName = "Silberangel", HouseholdPosition = "Head Of Household", Nickname = "Matt"},
                        new MpHouseholdMember() { Age = 29, ContactId = 54879, DateOfBirth = new DateTime(1987, 11, 5), FirstName = "Leslie", LastName = "Silbernagel", HouseholdPosition = "Head of Household Spouse", Nickname = "Les"}
                    }
                );
            var heads = _fixture.GetHeadsOfHousehold(contactId, householdId);
            _contactService.VerifyAll();

            Assert.AreEqual(2, heads.HeadsOfHousehold.Count());
        }

        [Test]
        public void ShouldThrowExceptionIfNotHeadOfHousehold()
        {
            const int householdId = 1234;
            const int contactId = 9087;

            _contactService.Setup(m => m.GetHouseholdFamilyMembers(householdId))
                .Returns(
                    new List<MpHouseholdMember>()
                    {
                        new MpHouseholdMember() { Age = 36, ContactId = 123456, DateOfBirth = new DateTime(1980, 2, 21), FirstName = "Matt", LastName = "Silberangel", HouseholdPosition = "Head Of Household", Nickname = "Matt"},
                        new MpHouseholdMember() { Age = 29, ContactId = 54879, DateOfBirth = new DateTime(1987, 11, 5), FirstName = "Leslie", LastName = "Silbernagel", HouseholdPosition = "Head of Household Spouse", Nickname = "Les"},
                        new MpHouseholdMember() { Age = 8, ContactId = contactId, DateOfBirth = new DateTime(2008, 4, 3), FirstName = "Miles", LastName = "Silbernagel", HouseholdPosition = "Minor Child", Nickname = "Miles"}
                    }
                );

            Assert.Throws<NotHeadOfHouseholdException>( () => _fixture.GetHeadsOfHousehold(contactId, householdId));
            _contactService.VerifyAll();

        }

    }
}