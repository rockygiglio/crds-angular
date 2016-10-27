using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using crds_angular.Models.Json;
using crds_angular.Security;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Newtonsoft.Json;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class MinistryPlatformToolsController: MPAuth
    {
        private readonly ISelectionRepository _selectionService;

        public MinistryPlatformToolsController(ISelectionRepository selectionService)
        {
            _selectionService = selectionService;
        }

        [HttpGet]
        [VersionedRoute(template: "mpTools/selection/{selectionId:regex(\\d+)}", minimumVersion: "1.0.0")]
        [Route("mptools/selection/{selectionId:regex(\\d+)}")]
        public IHttpActionResult GetPageSelectionRecordIds(int selectionId)
        {
            var response = Authorized(token =>
            {
                var selected = _selectionService.GetSelectionRecordIds(token, selectionId);
                if (selected == null || !selected.Any())
                {
                    return (RestHttpActionResult<SelectedRecords>.WithStatus(HttpStatusCode.NotFound, new SelectedRecords()));
                }
                var selectedRecords = new SelectedRecords
                {
                    RecordIds = selected
                };

                return (Ok(selectedRecords));
            });

            return (response);
        }
    }

    public class SelectedRecords
    {
        [JsonProperty("RecordIds", NullValueHandling = NullValueHandling.Ignore)]
        public IList<int> RecordIds { get; set; }
    }
}