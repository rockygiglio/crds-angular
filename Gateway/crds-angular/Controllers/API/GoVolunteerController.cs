using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GoVolunteerController : MPAuth
    {
        private readonly IOrganizationService _organizationService;
        private readonly IGatewayLookupService _gatewayLookupService;
        private readonly IGoVolunteerService _goVolunteerService;

        public GoVolunteerController(IOrganizationService organizationService, IGatewayLookupService gatewayLookupService, IGoVolunteerService goVolunteerService)
        {
            _organizationService = organizationService;
            _gatewayLookupService = gatewayLookupService;
            _goVolunteerService = goVolunteerService;
        }

        [HttpGet]
        [ResponseType(typeof(Organization))]
        [Route("api/organization/{name}")]
        public IHttpActionResult GetOrganization(string name)
        {
            try
            {
                var org = _organizationService.GetOrganizationByName(name);
                if (org == null)
                {
                    return NotFound();
                }
                return Ok(org);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Get Organization failed: ", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
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

        [HttpGet]
        [ResponseType(typeof (List<OrgLocation>))]
        [Route("api/organizations/{orgId}/locations")]
        public IHttpActionResult GetLocationsForOrganization(int orgId)
        {
            try
            {
                var Locs = _organizationService.GetLocationsForOrganization(orgId);
                return Ok(Locs);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Unable to get locations", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpGet]
        [ResponseType(typeof(List<ProjectType>))]
        [Route("api/goVolunteer/projectTypes")]
        public IHttpActionResult GetProjectTypes()
        {
            try
            {
                var projectTypes = _goVolunteerService.GetProjectTypes();
                return Ok(projectTypes);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Unable to get project types", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}
