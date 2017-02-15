using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Finder;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;
using log4net;
using MinistryPlatform.Translation.Models.Finder;

namespace crds_angular.Controllers.API
{
    public class FinderController : MPAuth
    {
        private readonly IFinderService _finderService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FinderController(IFinderService finderService, IUserImpersonationService userImpersonationService, IAuthenticationRepository authenticationRepository)
            : base(userImpersonationService, authenticationRepository)
        {
            _finderService = finderService;
        }

        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/pin/{participantId}", minimumVersion: "1.0.0")]
        [Route("finder/pin/{participantId}")]
        [HttpGet]
        public IHttpActionResult GetPinDetails([FromUri]int participantId)
        {
            try
            {
                var list = _finderService.GetPinDetails(participantId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Leaders Groups Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        /// <summary>
        /// Create Pin with provided address details
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/pin", minimumVersion: "1.0.0")]
        [Route("finder/pin")]
        [HttpPost]
        public IHttpActionResult PostPinDetails([FromBody] PinDto pin)
        {
            return Authorized(token =>
            {
                try
                {
                   // _finderService.SavePinDetails(pin);                    
                    _logger.DebugFormat("Successfully created pin for participant {0} ", pin.Contact_ID);
                    return (Ok());
                }
                // TODO: additional exception handling needed?
                catch (Exception e)
                {
                    _logger.Error("Could not create pin", e);
                    return BadRequest();
                }
            });
        }

    }
}