using System.Web.Http;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using System.Web.Http.Description;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class ParticipantController : MPAuth
    {
        private readonly IGroupService _groupService;
        public ParticipantController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Gets a <see cref="Participant">Participant</see> for a logged-in user. 
        /// </summary>
        /// <returns>A <see cref="Participant">Participant</see> record for the logged-in user.</returns>
        [RequiresAuthorization]
        [ResponseType(typeof(Participant))]
        [VersionedRoute(template: "participant", minimumVersion: "1.0.0")]
        [Route("participant")]
        [HttpGet]
        public IHttpActionResult GetParticipant()
        {
            return Authorized(token => Ok(_groupService.GetParticipantRecord(token)));
        }
    }
}