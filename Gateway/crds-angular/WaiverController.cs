using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;

namespace crds_angular
{
    public class WaiverController : ReactiveMPAuth
    {

        public readonly IEventService _eventService;

        public WaiverController(
            IEventService eventService,
            IUserImpersonationService userImpersonationService, 
            IAuthenticationRepository authenticationRepository) : base(userImpersonationService, authenticationRepository)
        {
            _eventService = eventService;
        }

        [VersionedRoute(template: "waivers/event/{eventId}", minimumVersion: "1.0.0")]
        [ResponseType(typeof(List<MpWaivers>))]
        [HttpGet]
        public async Task<IHttpActionResult> GetEventWaivers(int eventId)
        {
            return await Authorized(token =>
            {
                var waivers = _eventService.EventWaivers(eventId).Wait();
                return Ok(waivers);
            });
        }


    }
}