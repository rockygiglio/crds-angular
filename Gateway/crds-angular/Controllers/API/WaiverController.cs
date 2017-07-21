using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Waivers;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;

namespace crds_angular.Controllers.API
{
    public class WaiverController : ReactiveMPAuth
    {

        public readonly IEventService _eventService;
        public readonly IWaiverService _waiverService;

        public WaiverController(
            IEventService eventService,
            IWaiverService waiverService,
            IUserImpersonationService userImpersonationService, 
            IAuthenticationRepository authenticationRepository) : base(userImpersonationService, authenticationRepository)
        {
            _eventService = eventService;
            _waiverService = waiverService;
        }

        [VersionedRoute(template: "waivers/event/{eventId}", minimumVersion: "1.0.0")]
        [ResponseType(typeof(List<WaiverDTO>))]
        [HttpGet]
        public async Task<IHttpActionResult> GetEventWaivers(int eventId)
        {
            return await Authorized(token =>
            {
                var waivers = _eventService.EventWaivers(eventId).Wait();
                return Ok(waivers);
            });
        }

        [VersionedRoute(template: "waivers/{waiverId}", minimumVersion: "1.0.0")]
        [ResponseType(typeof(WaiverDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> GetWaiver(int waiverId)
        {
            return await Authorized(token =>
            {
                //var waiver = _waiverService.GetWaiver(waiverId).Wait();
                return Ok();
            });
        }


    }
}