using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GatewayLookupController : MPAuth

    {
        private readonly IGatewayLookupService _gatewayLookupService;
        public GatewayLookupController(IGatewayLookupService gatewayLookupService)
        {
            _gatewayLookupService = gatewayLookupService;
        }

        [HttpGet]
        [ResponseType(typeof(List<OtherOrganization>))]
        [Route("api/organizations/other")]
        public IHttpActionResult GetOtherOrganizations()
        {
            try
            {
                var Orgs = _gatewayLookupService.GetOtherOrgs();
                return Ok(Orgs);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Unable to get other organizations", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}