using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Models.Crossroads.Subscription;

namespace crds_angular.Controllers.API
{
    public class SubscriptionsController : MPAuth
    {
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly ISubscriptionsService _subscriptionService;

        public SubscriptionsController(ISubscriptionsService subscriptionService, MPInterfaces.IAuthenticationService authenticationService)
        {
            _subscriptionService = subscriptionService;
            _authenticationService = authenticationService;
        }

        [ResponseType(typeof (List<Dictionary<string, object>>))]
        [Route("api/subscriptions")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return (Authorized(token =>
            {
                var contactId = _authenticationService.GetContactId(token);
                return (Ok(_subscriptionService.GetSubscriptions(contactId, token)));
            }));
        }

        [Route("api/subscriptions")]
        [HttpPost]
        public IHttpActionResult Post(Dictionary<string, object> subscription)
        {
            return (Authorized(token =>
            {
                var contactId = _authenticationService.GetContactId(token);
                var recordId = new {dp_RecordID = _subscriptionService.SetSubscriptions(subscription, contactId, token)};
                return this.Ok(recordId);
            }));
        }

        [Route("api/subscriptions/optin")]
        [HttpPost]
        public IHttpActionResult PostOptIn(OptInRequest request)
        {
            try
            {
                var response = _subscriptionService.AddListSubscriber(request.EmailAddress, request.ListName);
                return this.Ok(response);
            }
            catch (System.Exception ex)
            {
                return this.InternalServerError();
            }
        }
    }
}