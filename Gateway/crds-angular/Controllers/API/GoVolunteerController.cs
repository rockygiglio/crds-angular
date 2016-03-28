using System;
using System.Collections.Generic;
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
        private readonly IGatewayLookupService _gatewayLookupService;
        private readonly IGroupConnectorService _groupConnectorService;
        private readonly IOrganizationService _organizationService;
        private readonly IGoSkillsService _skillsService;
        private readonly IAttributeService _attributeService;
        private readonly IConfigurationWrapper _configurationWrapper;

        public GoVolunteerController(IOrganizationService organizationService, 
            IGroupConnectorService groupConnectorService, 
            IGatewayLookupService gatewayLookupService, 
            IGoSkillsService skillsService,
            IAttributeService attributeService,
            IConfigurationWrapper configWrapper)
        {
            _organizationService = organizationService;
            _gatewayLookupService = gatewayLookupService;
            _groupConnectorService = groupConnectorService;
            _skillsService = skillsService;
            _attributeService = attributeService;
            _configurationWrapper = configWrapper;
        }

        [HttpGet]
        [ResponseType(typeof(List<AttributeDTO>))]
        [Route("api/govolunteer/prep-times")]
        public IHttpActionResult GetPrepTimes()
        {
            try
            {
                var prepTypeId = _configurationWrapper.GetConfigIntValue("PrepWorkAttributeTypeId");
                var attributes = _attributeService.GetAttributeTypes(prepTypeId);
                var attributeTypeDto = attributes.Single();
                return Ok(attributeTypeDto.Attributes);;
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Get Prep Times failed: ", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [HttpGet]
        [ResponseType(typeof(List<GoSkills>))]
        [Route("api/govolunteer/skills")]
        public IHttpActionResult GetGoSkills()
        {
            try
            {
                var skills =_skillsService.RetrieveGoSkills();
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
    }
}
