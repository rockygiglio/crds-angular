using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    class CampRepositoryTest
    {
        
        private readonly Mock<IConfigurationWrapper> _configurationWrapper;
        private readonly Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;
        private readonly Mock<IApiUserRepository> _apiUserRepository; 
        private readonly ICampRepository _fixture;

        private const string token = "token";
        private const string createContactStoredProc = "api_crds_CreateContact";
        private const string campParticipantStoredProc = "api_crds_Add_As_CampParticipant";

        public CampRepositoryTest()
        {
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _apiUserRepository = new Mock<IApiUserRepository>();

            _fixture = new CampRepository(_configurationWrapper.Object, _ministryPlatformRest.Object, _apiUserRepository.Object);
            
        }

        [SetUp]
        public void Setup()
        {
            _configurationWrapper.Setup(m => m.GetConfigValue("CreateContactStoredProc")).Returns(createContactStoredProc);
            _configurationWrapper.Setup(m => m.GetConfigValue("CampParticipantStoredProc")).Returns(campParticipantStoredProc);
        }

        [Test]
        public void ShouldCreateMinorContact()
        {
            var list = ContactIdList();
            var lists = new List<List<MpRecordID>>
            {
                list
            };
            var birthDate = DateTime.Today.AddYears(-13);
            var minorContact = mockMinorContact(birthDate);

            var values = new Dictionary<string, object>
            {
                {"@FirstName", minorContact.FirstName},
                {"@LastName", minorContact.LastName},
                {"@MiddleName", minorContact.MiddleName},
                {"@PreferredName", minorContact.PreferredName },
                {"@NickName", minorContact.NickName },
                {"@Birthdate", minorContact.BirthDate },
                {"@Gender", minorContact.Gender },
                {"@SchoolAttending", minorContact.SchoolAttending },
                {"@HouseholdId", minorContact.HouseholdId },
                {"@HouseholdPosition", minorContact.HouseholdPositionId }
            };

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token).GetFromStoredProc<MpRecordID>(createContactStoredProc, values)).Returns(lists);
            var returnVal = _fixture.CreateMinorContact(mockMinorContact(birthDate));

            _ministryPlatformRest.VerifyAll();
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

        private MpMinorContact mockMinorContact(DateTime birthDate)
        {
            return new MpMinorContact()
            {
                FirstName = "firstname",
                LastName = "LastName",
                MiddleName = "MiddleName",
                PreferredName = "PreferredName",
                NickName = "NickName",
                BirthDate = birthDate,
                Gender = 1,
                SchoolAttending = "SchoolAttending",
                HouseholdId = 12345,
                HouseholdPositionId = 2
            };

        }


    }
}
