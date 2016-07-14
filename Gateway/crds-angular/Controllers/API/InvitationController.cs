using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Json;
using crds_angular.Security;
using log4net;

namespace crds_angular.Controllers.API
{
    public class InvitationController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Services.Interfaces.IInvitationService _invitationService;        


        public InvitationController(Services.Interfaces.IInvitationService invitationService)
        {
            this._invitationService = invitationService;

        }

        [AcceptVerbs("POST")]
        [RequiresAuthorization]
        [ResponseType(typeof(Invitation))]
        [Route("api/invitation")]
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
                    _invitationService.ValidateInvitation(dto, token);
                    return Ok(_invitationService.CreateInvitation(dto, token));
                }
                catch (ValidationException e)
                {
                    var error = new ApiErrorDto("Not authorized to send invitations of this type", e, HttpStatusCode.Forbidden);
                    throw new HttpResponseException(error.HttpResponseMessage);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("CreateInvitation Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

    }
}