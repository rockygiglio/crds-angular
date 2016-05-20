
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Security;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;


namespace crds_angular.Controllers.API
{
    public class FormBuilderController : MPAuth
    {
        private IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserService _apiUserService;

        public FormBuilderController(IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            this._ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
        }

        /// <summary>
        /// Get page view for groups
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<GroupDTO>))]

        [System.Web.Http.Route("api/formbuilder/pages/{pageView}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPageView(int pageView)
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