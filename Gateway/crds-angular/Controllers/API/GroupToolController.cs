using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Security;
using log4net;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Repositories.Interfaces;
using crds_angular.Services.Interfaces;
using Event = crds_angular.Models.Crossroads.Events.Event;

namespace crds_angular.Controllers.API
{
    public class GroupToolController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Services.Interfaces.IGroupToolService groupToolService;        
        private readonly IAuthenticationRepository authenticationService;


        public GroupToolController(Services.Interfaces.IGroupToolService groupToolService,
                               IAuthenticationRepository authenticationService)
        {
            this.groupToolService = groupToolService;
            this.authenticationService = authenticationService;

        }

        [AcceptVerbs("POST")]
        [Route("api/grouptool/join-request")]
        public IHttpActionResult CreateRequestToJoin([FromBody] Invitation dto)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Return all pending invitations
        /// </summary>
        /// <param name="SourceId">An integer identifying a group or a trip campaign or some entity to be named later</param>
        /// <param name="InvitationTypeId">An integer indicating which invitations are to be returned. For example, Groups or Trips or a source to be identified later.</param>
        /// <returns>A list of Invitation DTOs</returns>
        [AcceptVerbs("GET")]
        [RequiresAuthorization]
        [ResponseType(typeof(List<Invitation>))]
        [Route("api/grouptool/invitations/{SourceId}/{InvitationTypeId}")]
        public IHttpActionResult GetInvitations(int SourceId, int InvitationTypeId)
        {
            return Authorized(token =>
            {
                try
                {
                    var invitessAndRequestors = groupToolService.GetInvitations(SourceId, InvitationTypeId, token);
                    return Ok(invitessAndRequestors);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("GetInvitations Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

    }
}