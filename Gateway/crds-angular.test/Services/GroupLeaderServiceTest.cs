using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class GroupLeaderServiceTest
    {
        private Mock<IUserRepository> _userRepo;
        private Mock<IPersonService> _personService;
        private Mock<IFormSubmissionRepository> _formService;
        private Mock<IContactRepository> _contactRepo;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private IGroupLeaderService _fixture;

        [SetUp]
        public void Setup()
        {
            _userRepo = new Mock<IUserRepository>();
            _personService = new Mock<IPersonService>();
            _formService = new Mock<IFormSubmissionRepository>();
            _contactRepo = new Mock<IContactRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _fixture = new GroupLeaderService(_personService.Object, _userRepo.Object, _formService.Object, _configurationWrapper.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _personService.VerifyAll();
            _userRepo.VerifyAll();
            _formService.VerifyAll();
            _contactRepo.VerifyAll();
            _configurationWrapper.VerifyAll();
        }

        [Test]
        public void ShouldSaveProfileWithCorrectDisplayName()
        {
            var leaderDto = GroupLeaderMock();

            const string fakeToken = "letmein";
            const int fakeUserId = 98124;

            _userRepo.Setup(m => m.GetUserIdByUsername(leaderDto.OldEmail)).Returns(fakeUserId);
            _userRepo.Setup(m => m.UpdateUser(It.IsAny<Dictionary<string, object>>()));
            _personService.Setup(m => m.SetProfile(fakeToken, It.IsAny<Person>())).Callback((string token, Person person) =>
            {
                Assert.AreEqual(person.GetContact().Display_Name, $"{leaderDto.LastName}, {leaderDto.NickName}");
            });
            _fixture.SaveProfile(fakeToken, leaderDto).Wait();            
        }

        [Test]
        public void ShouldSaveProfileWithCorrectDisplayNameAndUserWithCorrectEmail()
        {
            var leaderDto = GroupLeaderMock();

            const string fakeToken = "letmein";
            const int fakeUserId = 98124;

            _userRepo.Setup(m => m.GetUserIdByUsername(leaderDto.OldEmail)).Returns(fakeUserId);
            _userRepo.Setup(m => m.UpdateUser(It.IsAny<Dictionary<string, object>>())).Callback((Dictionary<string, object> userData) =>
            {
                Thread.Sleep(5000);
                Assert.AreEqual(leaderDto.Email, userData["User_Name"]);
                Assert.AreEqual(leaderDto.Email, userData["User_Email"]);
            });
            _personService.Setup(m => m.SetProfile(fakeToken, It.IsAny<Person>())).Callback((string token, Person person) =>
            {
                Assert.AreEqual(person.GetContact().Display_Name, $"{leaderDto.LastName}, {leaderDto.NickName}");
            });
            _fixture.SaveProfile(fakeToken, leaderDto).Wait();
        }

        [Test]
        public void ShouldUpdateUserWithCorrectEmail()
        {
            const string fakeToken = "letmein";
            const int fakeUserId = 98124;
            var leaderDto = GroupLeaderMock();
            _personService.Setup(m => m.SetProfile(fakeToken, It.IsAny<Person>()));
            _userRepo.Setup(m => m.GetUserIdByUsername(leaderDto.OldEmail)).Returns(fakeUserId);
            _userRepo.Setup(m => m.UpdateUser(It.IsAny<Dictionary<string, object>>())).Callback((Dictionary<string, object> userData) =>
            {
                Assert.AreEqual(leaderDto.Email, userData["User_Name"]);
                Assert.AreEqual(leaderDto.Email, userData["User_Email"]);
            });
            _fixture.SaveProfile(fakeToken, leaderDto).Wait();
        }

        [Test]
        public void ShouldRethrowExceptionWhenPersonServiceThrows()
        {
            const string fakeToken = "letmein";
            const int fakeUserId = 98124;
            var leaderDto = GroupLeaderMock();
            _personService.Setup(m => m.SetProfile(fakeToken, It.IsAny<Person>())).Throws(new Exception("no person to save"));
            _userRepo.Setup(m => m.GetUserIdByUsername(leaderDto.OldEmail)).Returns(fakeUserId);            

            Assert.Throws<Exception>(() =>
            {
                _fixture.SaveProfile(fakeToken, leaderDto).Wait();
            });
        }

        [Test]
        public void ShouldRethrowExceptionWhenUserServiceThrows()
        {
            const string fakeToken = "letmein";
            const int fakeUserId = 98124;
            var leaderDto = GroupLeaderMock();
            _personService.Setup(m => m.SetProfile(fakeToken, It.IsAny<Person>()));
            _userRepo.Setup(m => m.GetUserIdByUsername(leaderDto.OldEmail)).Returns(fakeUserId);
            _userRepo.Setup(m => m.UpdateUser(It.IsAny<Dictionary<string, object>>())).Throws(new Exception("no user to save"));

            Assert.Throws<Exception>(() =>
            {
                _fixture.SaveProfile(fakeToken, leaderDto).Wait();
            });
        }

        [Test]
        public void ShouldSaveReferenceData()
        {
            var fakeDto = GroupLeaderMock();

            const int groupLeaderFormConfig = 23;
            const int groupLeaderReference = 56;
            const int groupLeaderHuddle = 92;
            const int groupLeaderStudent = 126;

            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormId")).Returns(groupLeaderFormConfig);
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderReferenceFieldId")).Returns(groupLeaderReference);
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderHuddleFieldId")).Returns(groupLeaderHuddle);
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderStudentFieldId")).Returns(groupLeaderStudent);

            _formService.Setup(m => m.SubmitFormResponse(It.IsAny<MpFormResponse>())).Returns((MpFormResponse form) =>
            {
                Assert.AreEqual(groupLeaderFormConfig, form.FormId);
                return 1;
            });
            var responseId = _fixture.SaveReferences(fakeDto).Wait();
            Assert.AreEqual(responseId, 1);
        }
	
	[Test]
        public void ShouldThrowExceptionWhenSaveReferenceDataFails()
        {
            var fakeDto = GroupLeaderMock();

            const int groupLeaderFormConfig = 23;
            const int groupLeaderReference = 56;
            const int groupLeaderHuddle = 92;
            const int groupLeaderStudent = 126;

            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormId")).Returns(groupLeaderFormConfig);
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderReferenceFieldId")).Returns(groupLeaderReference);
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderHuddleFieldId")).Returns(groupLeaderHuddle);
            _configWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderStudentFieldId")).Returns(groupLeaderStudent);

            _formService.Setup(m => m.SubmitFormResponse(It.IsAny<MpFormResponse>())).Returns((MpFormResponse form) =>
            {
                Assert.AreEqual(groupLeaderFormConfig, form.FormId);
                return 0;
            });

            Assert.Throws<ApplicationException>(() => _fixture.SaveReferences(fakeDto).Wait());
        }

        [Test]
        public void ShouldSaveSpiritualGrowthAnswers()
        {
            const int fakeFormId = 5;
            const int fakeStoryFieldId = 1;
            const int fakeTaughtFieldId = 2;
            const int fakeResponseId = 10;
            
            var growthDto = SpiritualGrowthMock();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormId")).Returns(fakeFormId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormStoryFieldId")).Returns(fakeStoryFieldId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormTaughtFieldId")).Returns(fakeTaughtFieldId);

            _formService.Setup(m => m.SubmitFormResponse(It.IsAny<MpFormResponse>())).Returns((MpFormResponse form) =>
            {
                Assert.AreEqual(fakeFormId, form.FormId);
                return fakeResponseId;
            });

            var responseId = _fixture.SaveSpiritualGrowth(growthDto).Wait();
            Assert.AreEqual(fakeResponseId, responseId);
        }

        [Test]
        public void ShouldThrowExceptionWhenSavingSpiritualGrowthFails()
        {
            const int fakeFormId = 5;
            const int fakeStoryFieldId = 1;
            const int fakeTaughtFieldId = 2;
            const int errorResponseId = 0;

            var growthDto = SpiritualGrowthMock();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormId")).Returns(fakeFormId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormStoryFieldId")).Returns(fakeStoryFieldId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormTaughtFieldId")).Returns(fakeTaughtFieldId);

            _formService.Setup(m => m.SubmitFormResponse(It.IsAny<MpFormResponse>())).Returns((MpFormResponse form) =>
            {
                Assert.AreEqual(fakeFormId, form.FormId);
                return errorResponseId;
            });

            Assert.Throws<ApplicationException>(() => _fixture.SaveSpiritualGrowth(growthDto).Wait());
        }

        private static GroupLeaderProfileDTO GroupLeaderMock()
        {
            return new GroupLeaderProfileDTO()
            {
                ContactId = 12345,
                BirthDate = new DateTime(1980, 02, 21),
                Email = "silbermm@gmail.com",
                LastName = "Silbernagel",
                NickName = "Matt",
                Site = 1,            
                OldEmail = "matt.silbernagel@ingagepartners.com",
                HouseholdId = 81562,
                HuddleResponse = "No",
                LeadStudents = "Yes",
                ReferenceContactId = "89158"
            };
        }

        private static SpiritualGrowthDTO SpiritualGrowthMock()
        {
            return new SpiritualGrowthDTO()
            {
                ContactId = 654321,
                EmailAddress = "hornerjn@gmail.com",
                Story = "my diary",
                Taught = "i lEarnDed hOw to ReAd"
            };
        }
    }
}
