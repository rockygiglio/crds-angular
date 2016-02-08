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
        }

        [Test]
        public void ShouldHandleSubscriptionRequest()
        {
            string email = "me@me.com";
            string listName = "the_daily";

            var result = _subscriptionService.AddListSubscriber(email, listName);
            Assert.IsInstanceOf(typeof(OptInResponse), result);
        }
    }
}
