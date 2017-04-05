using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Json;
using Moq;
using NUnit.Framework;
using ILoginService = crds_angular.Services.Interfaces.ILoginService;
using IPersonService = crds_angular.Services.Interfaces.IPersonService;
using MinistryPlatform.Translation.Repositories.Interfaces;
using crds_angular.Services.Interfaces;
using Crossroads.ClientApiKeys;
using Crossroads.Web.Common.Security;

namespace crds_angular.test.controllers
{
    [TestFixture]
    class LoginControllerTest
    {
        private LoginController loginController;

        private Mock<ILoginService> _loginServiceMock;
        private Mock<IPersonService> _personServiceMock;
        private Mock<IUserRepository> _userServiceMock;

        private string authType;
        private string authToken;

        [SetUp]
        public void SetUp()
        {
            _loginServiceMock = new Mock<ILoginService>();
            _personServiceMock = new Mock<IPersonService>();
            _userServiceMock = new Mock<IUserRepository>();

            loginController = new LoginController(_loginServiceMock.Object, _personServiceMock.Object, _userServiceMock.Object, new Mock<IUserImpersonationService>().Object, new Mock<IAuthenticationRepository>().Object);

            authType = "auth_type";
            authToken = "auth_token";
            loginController.Request = new HttpRequestMessage();
            loginController.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            loginController.RequestContext = new HttpRequestContext();

            _loginServiceMock = new Mock<ILoginService>();
            _loginServiceMock.Setup(m => m.PasswordResetRequest(It.IsAny<string>())).Returns(true);
        }

        [Test]
        public void ShouldAcceptResetRequest()
        {
            PasswordResetRequest resetRequest = new PasswordResetRequest();
            resetRequest.Email = "test_email";
            var result = loginController.RequestPasswordReset(resetRequest);
            Assert.AreEqual(typeof(OkResult), result.GetType());
        }

        [Test]
        public void LoginPostShouldIgnoreClientApiKey()
        {
            var loginMethod = loginController.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToList().Find(m => m.Name.Equals("Post"));
            Assert.IsNotNull(loginMethod.GetCustomAttribute<IgnoreClientApiKeyAttribute>(), $"Login Post method should have [IgnoreClientApiKey] attribute, so the check scanner controller can use it");
        }

        [Test]
        public void AllMethodsExceptLoginPostShouldNotIgnoreClientApiKey()
        {
            var publicMethods =
                loginController.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToList().FindAll(m => !m.Name.Equals("Post"));
            publicMethods.ForEach(m =>
            {
                Assert.IsNull(m.GetCustomAttribute<IgnoreClientApiKeyAttribute>(), $"Method {m.Name} should not ignore client API key");
            });
        }
    }
}
