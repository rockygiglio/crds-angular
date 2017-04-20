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
        public async void ShouldSaveProfileWithCorrectDisplayName()
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
            await _fixture.SaveProfile(fakeToken, leaderDto);            
        }

        [Test]
        public async void ShouldSaveProfileWithCorrectDisplayNameAndUserWithCorrectEmail()
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
            await _fixture.SaveProfile(fakeToken, leaderDto);
        }

        [Test]
        public async void ShouldUpdateUserWithCorrectEmail()
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
            await _fixture.SaveProfile(fakeToken, leaderDto);
        }

        [Test]
        public void ShouldRethrowExceptionWhenPersonServiceThrows()
        {
            const string fakeToken = "letmein";
            const int fakeUserId = 98124;
            var leaderDto = GroupLeaderMock();
            _personService.Setup(m => m.SetProfile(fakeToken, It.IsAny<Person>())).Throws(new Exception("no person to save"));
            _userRepo.Setup(m => m.GetUserIdByUsername(leaderDto.OldEmail)).Returns(fakeUserId);
            _userRepo.Setup(m => m.UpdateUser(It.IsAny<Dictionary<string, object>>()));

            Assert.Throws<Exception>(async () =>
            {
                await _fixture.SaveProfile(fakeToken, leaderDto);
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

            Assert.Throws<Exception>(async () =>
            {
                await _fixture.SaveProfile(fakeToken, leaderDto);
            });
        }

        [Test]
        public void ShouldSaveSpiritualGrowthAnswers()
        {
            const string fakeToken = "letmein";
            const int fakeUserId = 123456;
            const int fakeFormId = 5;
            const int fakeStoryFieldId = 1;
            const int fakeTaughtFieldId = 2;
            const int fakeResponseId = 10;
            
            var growthDto = SpiritualGrowthMock();

            //_userRepo.Setup(m => m.GetUserIdByUsername(growthDto.EmailAddress)).Returns(fakeUserId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormId")).Returns(fakeFormId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormStoryFieldId")).Returns(fakeStoryFieldId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormTaughtFieldId")).Returns(fakeTaughtFieldId);

            _formService.Setup(m => m.SubmitFormResponse(It.IsAny<MpFormResponse>())).Returns((MpFormResponse form) =>
            {
                Assert.AreEqual(fakeFormId, form.FormId);
                return fakeResponseId;
            });

            var responseId = _fixture.SaveSpiritualGrowth(fakeToken, growthDto).Wait();
            Assert.AreEqual(fakeResponseId, responseId);
        }

        [Test]
        public void ShouldThrowExceptionWhenSavingSpiritualGrowthFails()
        {
            const string fakeToken = "letmein";
            const int fakeUserId = 123456;
            const int fakeFormId = 5;
            const int fakeStoryFieldId = 1;
            const int fakeTaughtFieldId = 2;
            const int errorResponseId = 0;

            var growthDto = SpiritualGrowthMock();

            //_userRepo.Setup(m => m.GetUserIdByUsername(growthDto.EmailAddress)).Returns(fakeUserId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormId")).Returns(fakeFormId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormStoryFieldId")).Returns(fakeStoryFieldId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GroupLeaderFormTaughtFieldId")).Returns(fakeTaughtFieldId);

            _formService.Setup(m => m.SubmitFormResponse(It.IsAny<MpFormResponse>())).Returns((MpFormResponse form) =>
            {
                Assert.AreEqual(fakeFormId, form.FormId);
                return errorResponseId;
            });

            Assert.Throws<ApplicationException>(() => _fixture.SaveSpiritualGrowth(fakeToken, growthDto).Wait());
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
                OldEmail = "matt.silbernagel@ingagepartners.com"
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
