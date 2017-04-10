using System;
using System.Threading.Tasks;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;

namespace crds_angular.Controllers.API
{
    public class GroupLeaderController : ReactiveMPAuth
    {
        private readonly IGroupLeaderService _groupLeaderService;

        public GroupLeaderController(IGroupLeaderService groupLeaderService, IUserImpersonationService userImpersonationService, IAuthenticationRepository authenticationRepository) : base(userImpersonationService, authenticationRepository)
        {
            _groupLeaderService = groupLeaderService;
        }

        [VersionedRoute(template: "group-leader/profile", minimumVersion: "1.0.0")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveProfile([FromBody] GroupLeaderProfileDTO profile)
        {
            return await Authorized(token => {
                try
                {
                    Task.Run(() => _groupLeaderService.SaveProfile(token, profile));
                    return Ok();
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Saving Leader Profile failed:", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }                
            });
        }
    }
}