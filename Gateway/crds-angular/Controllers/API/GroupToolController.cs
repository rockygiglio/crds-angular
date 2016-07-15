using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;

namespace crds_angular.Controllers.API
{
    public class GroupToolController : MPAuth
    {
        private readonly Services.Interfaces.IGroupToolService _groupToolService;        

        public GroupToolController(Services.Interfaces.IGroupToolService groupToolService)
        {
            this._groupToolService = groupToolService;
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
        /// <param name="sourceId">An integer identifying a group or a trip campaign or some entity to be named later</param>
        /// <param name="invitationTypeId">An integer indicating which invitations are to be returned. For example, Groups or Trips or a source to be identified later.</param>
        /// <returns>A list of Invitation DTOs</returns>
        [AcceptVerbs("GET")]
        [RequiresAuthorization]
        [ResponseType(typeof(List<Invitation>))]
        [Route("api/grouptool/invitations/{sourceId}/{invitationTypeId}")]
        public IHttpActionResult GetInvitations(int sourceId, int invitationTypeId)
        {
            return Authorized(token =>
            {
                try
                {
                    var invitess = _groupToolService.GetInvitations(sourceId, invitationTypeId, token);
                    return Ok(invitess);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("GetInvitations Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Return all pending inquiries
        /// </summary>
        /// <param name="groupId">An integer identifying the group that we want the inquires for.</param>
        /// <returns>A list of Invitation DTOs</returns>
        [AcceptVerbs("GET")]
        [RequiresAuthorization]
        [ResponseType(typeof(List<Inquiry>))]
        [Route("api/grouptool/inquires/{groupId}")]
        public IHttpActionResult GetInquires(int groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    var requestors = _groupToolService.GetInquiries(groupId, token);
                    return Ok(requestors);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("GetInquires Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}