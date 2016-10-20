using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class CampServiceTest
    {
        private readonly ICampService _fixture;
        private readonly Mock<IContactRepository> _contactService;
        private readonly Mock<ICampRepository> _campService;
        private readonly Mock<IFormSubmissionRepository> _formSubmissionRepository;
        private readonly Mock<IConfigurationWrapper> _configurationWrapper;
        private readonly Mock<IParticipantRepository> _participantRepository;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IApiUserRepository> _apiUserRepository;
        private readonly Mock<IGroupRepository> _groupRepository;

        public CampServiceTest()
        {
            _contactService = new Mock<IContactRepository>();
            _campService = new Mock<ICampRepository>();
            _formSubmissionRepository = new Mock<IFormSubmissionRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _participantRepository = new Mock<IParticipantRepository>();
            _eventRepository = new Mock<IEventRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _fixture = new CampService(_contactService.Object, _campService.Object, _formSubmissionRepository.Object, _configurationWrapper.Object, _participantRepository.Object, _eventRepository.Object, _apiUserRepository.Object, _groupRepository.Object);
        }

        [Test]
        public void shouldGetCampFamilyMembers()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var isSummerCamp = true;
            var myContactId = 2187211;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(getFakeHouseholdMembers(myContact));
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _groupRepository.Setup(m => m.isMemberOfSummerCampGroups(123, apiToken)).Returns(true);

            var result = _fixture.GetEligibleFamilyMembers(true, token);
            Assert.AreEqual(result.Count, 1);
            _contactService.VerifyAll();
            _groupRepository.VerifyAll();
        }

        [Test]
        public void shouldGetEmptyListOfFamilyMembers()
        {
            var token = "asdfasdfasdfasdf";
            var apiToken = "apiToken";
            var isSummerCamp = true;
            var myContactId = 2187211;
            var myContact = getFakeContact(myContactId);

            _contactService.Setup(m => m.GetMyProfile(token)).Returns(myContact);
            _contactService.Setup(m => m.GetHouseholdFamilyMembers(myContact.Household_ID)).Returns(new List<MpHouseholdMember>());
            _contactService.Setup(m => m.GetOtherHouseholdMembers(myContactId)).Returns(new List<MpHouseholdMember>());
            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);            

            var result = _fixture.GetEligibleFamilyMembers(true, token);
            Assert.AreEqual(result.Count, 0);
            _contactService.VerifyAll();
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
                    Nickname               = "matt"
                }
            };
        }

        private MpMyContact getFakeContact(int contactId)
        {
            return new MpMyContact()
            {
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
    }
}
