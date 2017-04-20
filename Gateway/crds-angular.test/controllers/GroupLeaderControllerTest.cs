using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Security;
using Moq;
using NUnit.Framework;
using System.Reactive;
using System.Web.Http;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupLeaderControllerTest
    {
        private Mock<IGroupLeaderService> _groupLeaderService;
        private Mock<IUserImpersonationService> _userImpersonation;
        private Mock<IAuthenticationRepository> _authenticationRepo;
        private GroupLeaderController _fixture;

        private readonly string authToken = "authtoken";
        private readonly string authType = "authtype";

        [SetUp]
        public void Setup()
        {
            _groupLeaderService = new Mock<IGroupLeaderService>();
            _userImpersonation = new Mock<IUserImpersonationService>();
            _authenticationRepo = new Mock<IAuthenticationRepository>();   
            _fixture = new GroupLeaderController(_groupLeaderService.Object, _userImpersonation.Object, _authenticationRepo.Object)
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext()
            };
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
        }

        [TearDown]
        public void Teardown()
        {
            _groupLeaderService.VerifyAll();
            _authenticationRepo.VerifyAll();
            _userImpersonation.VerifyAll();   
        }

        [Test]
        public async void ShouldSaveTheProfile()
        {
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            var mockProfile = GroupLeaderMock();
            _groupLeaderService.Setup(m => m.SaveProfile(It.IsAny<string>(), mockProfile)).Callback((string authTokenParm, GroupLeaderProfileDTO mock) =>
            {
                Assert.AreEqual($"{authType} {authToken}", authTokenParm);
            }).Returns(Task.FromResult(Observable.Empty<Unit>()));

            var response = await _fixture.SaveProfile(mockProfile);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkResult>(response);
        }

        [Test]
        public void ShouldThrowExceptionWhenProfileIsNotSaved()
        {
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            var mockProfile = GroupLeaderMock();
            _groupLeaderService.Setup(m => m.SaveProfile(It.IsAny<string>(), mockProfile)).Throws(new Exception());            
            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.SaveProfile(mockProfile);                
            });           
        }

        [Test]
        public void ShouldOnlyAllowAuthenticatedUsersToSaveProfile()
        {            
            var mockProfile = GroupLeaderMock();
            _groupLeaderService.Setup(m => m.SaveProfile(It.IsAny<string>(), mockProfile)).Throws(new Exception());
            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.SaveProfile(mockProfile);                
            });

        }

        [Test]
        public async void ShouldSaveSpiritualGrowthAnswers()
        {
            var mockSpiritualGrowth = SpiritualGrowthMock();

            _groupLeaderService.Setup(m => m.SaveSpiritualGrowth(It.IsAny<SpiritualGrowthDTO>())).Returns(Observable.Start(() => 1));

            var response = await _fixture.SaveSpiritualGrowth(mockSpiritualGrowth);
            Assert.IsInstanceOf<OkResult>(response);
        }

        [Test]
        public void ShouldThrowExceptionWhenSpiritualGrowthAnswersArentSaved()
        {
            var mockSpiritualGrowth = SpiritualGrowthMock();

            _groupLeaderService.Setup(m => m.SaveSpiritualGrowth(It.IsAny<SpiritualGrowthDTO>())).Throws(new Exception());

            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.SaveSpiritualGrowth(mockSpiritualGrowth);
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