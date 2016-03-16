using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GoVolunteerController : MPAuth
    {
        private readonly IOrganizationService _organizationService;

        public GoVolunteerController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        [ResponseType(typeof ())]
        [Route("api/group-connectors/{org}")]
        public IHttpActionResult GetGroupConnectors(Organization org)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Event Data Invalid", new InvalidOperationException("Invalid Event Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    var groupConnectors = _groupConnectorService.GetGroupConnectorsByOrganization(org);

                    if (groupConnectors == null)
                    {
                        return NotFound();
                    }
                    return Ok(groupConnectors);
                }
                catch (Exception e)
                {
                    const string msg = "GoVolunteerController.GetGroupConnectors";
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [HttpGet]
        [ResponseType(typeof (Organization))]
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
    }
}