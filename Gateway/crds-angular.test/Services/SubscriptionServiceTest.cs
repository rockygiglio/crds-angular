using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using crds_angular.Services.Interfaces;
using crds_angular.Services;
using crds_angular.Models.Crossroads.Subscription;

namespace crds_angular.test.Services
{
    internal class SubscriptionServiceTest
    {
        private ISubscriptionsService _subscriptionService;

        private Mock<MPInterfaces.IMinistryPlatformService> _ministryPlatformService;
        private Mock<Util.Interfaces.IEmailListHandler> _emailListHandler;
        private Mock<MPInterfaces.IApiUserService> _apiUserService;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<MPInterfaces.IMinistryPlatformService>();
            _emailListHandler = new Mock<Util.Interfaces.IEmailListHandler>();
            _apiUserService = new Mock<MPInterfaces.IApiUserService>();

            _subscriptionService = new SubscriptionsService(_ministryPlatformService.Object, _emailListHandler.Object, _apiUserService.Object);

            _emailListHandler.Setup(m => m.AddListSubscriber(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new OptInResponse());

            //_logger = new Mock<ILog>();
            //_authenticationService = new Mock<MPInterfaces.IAuthenticationService>();
            //_configurationWrapper = new Mock<IConfigurationWrapper>();
            //_contactService = new Mock<MPInterfaces.IContactService>();
            //_emailCommunication = new Mock<IEmailCommunication>();
            //_userService = new Mock<MPInterfaces.IUserService>();

            //_loginService = new LoginService(_authenticationService.Object, _configurationWrapper.Object, _contactService.Object, _emailCommunication.Object, _userService.Object);

            //_contactService.Setup(m => m.GetContactIdByEmail(It.IsAny<string>())).Returns(123456);
            //_userService.Setup(m => m.GetUserIdByUsername(It.IsAny<string>())).Returns(123456);
        }

        [Test]
        public void ShouldHandleSubscriptionRequest()
        {
            //string email = "someone@someone.com";

            //var result = _loginService.PasswordResetRequest(email);
            //Assert.AreEqual(true, result);
            string email = "me@me.com";
            string listName = "the_daily";
            string token = "111";

            var result = _subscriptionService.AddListSubscriber(email, listName, token);
        }
    }
}
