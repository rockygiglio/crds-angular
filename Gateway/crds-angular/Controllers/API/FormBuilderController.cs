
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Security;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using log4net;


namespace crds_angular.Controllers.API
{
    public class FormBuilderController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserService _apiUserService;
        private readonly Services.Interfaces.IFormBuilderService formBuilderService;

        public FormBuilderController(IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            this._ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
        }

        /// <summary>
        /// Get groups associated with a pageViewId
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<GroupDTO>))]
        [System.Web.Http.Route("api/formbuilder/pages/{pageView}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPageViewRecords(int pageView)
        {
            return Authorized(token =>
            {//TODO need to handle security and need to refactor this code
                try
                {
                    var pageViewRecords = formBuilderService.getPageViewRecords(pageView);
                    if (pageViewRecords.Count == 0)
                    {
                        throw new Exception("No records found for page view");
                    }
                    _logger.Debug(String.Format("Successfully returned {0} list of groups", pageView));
                    return Ok(pageViewRecords);
                }
                catch (Exception e)
                {
                    _logger.Error("Could not get page view results", e);
                    return BadRequest();
                }
            });
        }

        /// <summary>
        /// Get groups associated with a pageViewId
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<GroupDTO>))]
        [System.Web.Http.Route("api/formbuilder/pages/{pageView}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPageViewRecordsOld(int pageView)
        {
            return Authorized(token =>
            {//TODO need to handle security and need to refactor this code
                var pageViewRecords = _ministryPlatformService.GetPageViewRecords(pageView, token);

                if (pageViewRecords.Count == 0)
                {
                    return null;
                }

                return Ok(pageViewRecords);

            });
        }



    }
}