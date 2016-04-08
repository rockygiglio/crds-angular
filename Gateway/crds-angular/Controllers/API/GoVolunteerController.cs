using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GoVolunteerController : MPAuth
    {
        private readonly IAttributeService _attributeService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IGatewayLookupService _gatewayLookupService;
        private readonly IGoVolunteerService _goVolunteerService;
        private readonly IGroupConnectorService _groupConnectorService;
        private readonly IOrganizationService _organizationService;
        private readonly IGoSkillsService _skillsService;

        public GoVolunteerController(IOrganizationService organizationService,
                                     IGroupConnectorService groupConnectorService,
                                     IGatewayLookupService gatewayLookupService,
                                     IGoSkillsService skillsService,
                                     IGoVolunteerService goVolunteerService,
                                     IAttributeService attributeService,
                                     IConfigurationWrapper configurationWrapper)

        {
            _organizationService = organizationService;
            _gatewayLookupService = gatewayLookupService;
            _goVolunteerService = goVolunteerService;
            _groupConnectorService = groupConnectorService;
            _skillsService = skillsService;
            _attributeService = attributeService;
            _configurationWrapper = configurationWrapper;
        }

        [HttpGet]
        [ResponseType(typeof (List<ChildrenOptions>))]
        [Route("api/govolunteer/children")]
        public IHttpActionResult GetGoChildrenOptions()
        {
            try
            {
                // GO Cincinnati - Registration Children
                var options = _goVolunteerService.ChildrenOptions();
                return Ok(options);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Get Go Volunteer Children Options failed: ", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpGet]
        [ResponseType(typeof (List<AttributeDTO>))]
        [Route("api/govolunteer/prep-times")]
        public IHttpActionResult GetPrepTimes()
        {
            try
            {
                return Ok(GetAttributesByType("PrepWorkAttributeTypeId"));
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Get Prep Times failed: ", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpGet]
        [ResponseType(typeof (List<AttributeDTO>))]
        [Route("api/govolunteer/equipment")]
        public IHttpActionResult GetGoEquipment()
        {
            try
            {
                return Ok(GetAttributesByType("GoCincinnatiEquipmentAttributeType"));
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Get Go Volunteer Equipment failed: ", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        private List<AttributeDTO> GetAttributesByType(string attributeType)
        {
            var attributeTypeId = _configurationWrapper.GetConfigIntValue(attributeType);
            var attributeTypes = _attributeService.GetAttributeTypes(attributeTypeId);
            return attributeTypes.Single().Attributes;
        }

        [HttpGet]
        [ResponseType(typeof (List<GoSkills>))]
        [Route("api/govolunteer/skills")]
        public IHttpActionResult GetGoSkills()
        {
            return (Authorized(Skills, () => Skills(string.Empty)));
        }

        private IHttpActionResult Skills(string token)
        {
            try
            {
                var skills = _skillsService.RetrieveGoSkills(token);
                return Ok(skills);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Get Go Volunteer Skills failed: ", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpGet]
        [ResponseType(typeof (List<GroupConnector>))]
        [Route("api/group-connectors/open-orgs/{initiativeId}")]
        public IHttpActionResult GetGetGroupConnectorsOpenOrgs(int initiativeId)
        {
            try
            {
                var groupConnectors = _groupConnectorService.GetGroupConnectorsForOpenOrganizations(initiativeId);

                if (groupConnectors == null)
                {
                    return NotFound();
                }
                return Ok(groupConnectors);
            }
            catch (Exception e)
            {
                const string msg = "GoVolunteerController.GetGetGroupConnectorsOpenOrgs";
                logger.Error(msg, e);
                var apiError = new ApiErrorDto(msg, e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpGet]
        [ResponseType(typeof (List<GroupConnector>))]
        [Route("api/group-connectors/{orgId}/{initiativeId}")]
        public IHttpActionResult GetGroupConnectorsForOrg(int orgId, int initiativeId)
        {
            try
            {
                var groupConnectors = _groupConnectorService.GetGroupConnectorsByOrganization(orgId, initiativeId);

                if (groupConnectors == null)
                {
                    return NotFound();
                }
                return Ok(groupConnectors);
            }
            catch (Exception e)
            {
                const string msg = "GoVolunteerController.GetGroupConnectorsForOrg";
                logger.Error(msg, e);
                var apiError = new ApiErrorDto(msg, e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
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

        [HttpGet]
        [ResponseType(typeof (List<OtherOrganization>))]
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
        [ResponseType(typeof (List<ProjectType>))]
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

        [AcceptVerbs("POST")]
        [Route("api/govolunteer/registration")]
        public IHttpActionResult Post([FromBody] Registration goVolunteerRegistration)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token => SaveRegistration(token, goVolunteerRegistration),
                                  () => SaveRegistration(string.Empty, goVolunteerRegistration));
            }
            var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.ErrorMessage);
            var dataError = new ApiErrorDto("Registration Data Invalid", new InvalidOperationException("Invalid Registration Data" + errors));
            throw new HttpResponseException(dataError.HttpResponseMessage);
        }

        private IHttpActionResult SaveRegistration(string token, Registration goVolunteerRegistration)
        {
            try
            {
                // for testing
                goVolunteerRegistration.Self.FirstName = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                goVolunteerRegistration.InitiativeId = 1;
                // end for testing
                _goVolunteerService.CreateRegistration(goVolunteerRegistration, token);
                return Ok();
            }
            catch (Exception e)
            {
                var msg = "GoVolunteerRegistrationController: POST " + goVolunteerRegistration;
                logger.Error(msg, e);
                var apiError = new ApiErrorDto(msg, e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}