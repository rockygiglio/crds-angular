using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using log4net;

namespace crds_angular.Controllers.API
{
    public class InvitationController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Services.Interfaces.IInvitationService invitationService;        


        public InvitationController(Services.Interfaces.IInvitationService invitationService)
        {
            this.invitationService = invitationService;

        }

        [AcceptVerbs("POST")]
        [RequiresAuthorization]
        [ResponseType(typeof(Invitation))]
        [Route("api/grouptool/invitation")]
        public IHttpActionResult CreateInvitation([FromBody] Invitation dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("CreateInvitation Data Invalid", new InvalidOperationException("Invalid CreateInvitation Data " + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    return Ok(invitationService.CreateInvitation(dto, token));
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("CreateInvitation Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

    }
}