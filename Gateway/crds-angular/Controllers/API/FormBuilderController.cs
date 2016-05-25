
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Security;
using MinistryPlatform.Translation.Services.Interfaces;
using log4net;


namespace crds_angular.Controllers.API
{
    public class FormBuilderController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly Services.Interfaces.IFormBuilderService _formBuilderService;

        public FormBuilderController(Services.Interfaces.IFormBuilderService formBuilderService, 
                                     IMinistryPlatformService ministryPlatformService)        
        {
            _formBuilderService = formBuilderService;
            _ministryPlatformService = ministryPlatformService;
        }

        /// <summary>
        /// Get groups associated with a group key passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<GroupDTO>))]
        [Route("api/formbuilder/groups/{templateType}")]
        [HttpGet]
        public IHttpActionResult GetGroupsUndivided(string templateType)
        {
            return Authorized(token =>
            {
                try
                {
                    var groupsUndivided = _formBuilderService.GetGroupsUndivided(templateType);
                    return Ok(groupsUndivided);
                }
                catch (Exception e)
                {
                    _logger.Error("Could not get undivided groups", e);
                    return BadRequest();
                }
            });
        }
    }
}