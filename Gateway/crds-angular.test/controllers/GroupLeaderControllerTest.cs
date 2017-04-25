using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Security;
using Moq;
using NUnit.Framework;
using System.Reactive;
using System.Reactive.Disposables;
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

        private const string authToken = "authtoken";
        private const string authType = "authtype";

        [SetUp]
        public void Setup()
        {
            _groupLeaderService = new Mock<IGroupLeaderService>(MockBehavior.Strict);
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
            _groupLeaderService.Setup(m => m.SaveReferences(It.IsAny<GroupLeaderProfileDTO>())).Returns(Observable.Start(() => 1));
            _groupLeaderService.Setup(m => m.SaveProfile(It.IsAny<string>(), It.IsAny<GroupLeaderProfileDTO>())).Callback((string authTokenParm, GroupLeaderProfileDTO mock) =>
            {
                Assert.AreEqual($"{authType} {authToken}", authTokenParm);
            }).Returns(Observable.Empty<IList<Unit>>());

            var response = await _fixture.SaveProfile(mockProfile);
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkResult>(response);
        }

        [Test]
        public void ShouldThrowExceptionWhenProfileIsNotSaved()
        {
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            var mockProfile = GroupLeaderMock();
            _groupLeaderService.Setup(m => m.SaveReferences(It.IsAny<GroupLeaderProfileDTO>())).Returns(Observable.Start(() => 1));
            _groupLeaderService.Setup(m => m.SaveProfile(It.IsAny<string>(), mockProfile)).Throws(new Exception());            
            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.SaveProfile(mockProfile);                
            });           
        }

        [Test]
        public void ShouldOnlyAllowAuthenticatedUsers()
        {            
            var mockProfile = GroupLeaderMock();
            _groupLeaderService.Setup(m => m.SaveReferences(It.IsAny<GroupLeaderProfileDTO>())).Returns(Observable.Start(() => 1));
            _groupLeaderService.Setup(m => m.SaveProfile(It.IsAny<string>(), mockProfile)).Throws(new Exception());
            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.SaveProfile(mockProfile);                
            });

        }

        [Test]
        public async void ShouldSaveInterestedStaus()
        {
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            _groupLeaderService.Setup(m => m.SetInterested($"{authType} {authToken}"));

            var response = await _fixture.InterestedInGroupLeadership();
            Assert.IsNotNull(response);
        }

        [Test]
        public async void ShouldSaveSpiritualGrowthAnswers()
        {
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            var mockSpiritualGrowth = SpiritualGrowthMock();

            _groupLeaderService.Setup(m => m.SaveSpiritualGrowth(It.IsAny<SpiritualGrowthDTO>())).Returns(Observable.Start(() => 1));
            _groupLeaderService.Setup(m => m.SetApplied(It.IsAny<string>())).Returns(Observable.Start(() => 1));

            var response = await _fixture.SaveSpiritualGrowth(mockSpiritualGrowth);
            Assert.IsInstanceOf<OkResult>(response);
        }

        [Test]
        public void ShouldThrowWhenSavingInterestedStaus()
        {
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            _groupLeaderService.Setup(m => m.SetInterested($"{authType} {authToken}")).Throws(new Exception());

            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.InterestedInGroupLeadership();
            });
        }

        [Test]
        public void ShouldThrowExceptionWhenSpiritualGrowthAnswersArentSaved()
        {
            var mockSpiritualGrowth = SpiritualGrowthMock();
            _groupLeaderService.Setup(m => m.SaveSpiritualGrowth(It.IsAny<SpiritualGrowthDTO>())).Returns(Observable.Start(() => 1));
            _groupLeaderService.Setup(m => m.SetApplied(It.IsAny<string>())).Returns(Observable.Create<int>((observer) =>
            {
                observer.OnError(new Exception(""));
                return Disposable.Empty;
            }));

            Assert.Throws<HttpResponseException>(async () =>
            {
                await _fixture.SaveSpiritualGrowth(mockSpiritualGrowth);
            });
        }

        [Test]
        public void ShouldThrowExceptionWhenSpiritualGrowthAnswersAreSavedButNotApplied()
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
