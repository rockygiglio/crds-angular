using System;
using System.Collections.Generic;
using System.Threading;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
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
        private IGroupLeaderService _fixture;

        [SetUp]
        public void Setup()
        {
            _userRepo = new Mock<IUserRepository>();
            _personService = new Mock<IPersonService>();
            _fixture = new GroupLeaderService(_personService.Object, _userRepo.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _personService.VerifyAll();
            _userRepo.VerifyAll();
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
            }); ;
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
    }
}
