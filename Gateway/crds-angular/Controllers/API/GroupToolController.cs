using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Models.Json;
using crds_angular.Security;

namespace crds_angular.Controllers.API
{
    public class GroupToolController : MPAuth
    {
        private readonly Services.Interfaces.IGroupToolService _groupToolService;        

        public GroupToolController(Services.Interfaces.IGroupToolService groupToolService)
        {
            _groupToolService = groupToolService;
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
        [Route("api/grouptool/inquiries/{groupId}")]
        public IHttpActionResult GetInquiries(int groupId)
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

        /// <summary>
        /// Remove a participant from my group.
        /// </summary>
        /// <param name="groupTypeId">An integer identifying the type of group.</param>
        /// <param name="groupId">An integer identifying the group that the inquiry is associated to.</param>
        /// <param name="groupParticipantId">The ID of the group participant to remove</param>
        /// <param name="removalMessage">An optional message to send to the participant when they are removed.  This is sent along with a boilerplate message.</param>
        /// <returns>An empty response with 200 status code if everything worked, 403 if the caller does not have permission to remove a participant, or another non-success status code on any other failure</returns>
        [RequiresAuthorization]
        [Route("api/grouptool/grouptype/{groupTypeId:int}/group/{groupId:int}/participant/{groupParticipantId:int}")]
        [HttpDelete]
        public IHttpActionResult RemoveParticipantFromMyGroup([FromUri]int groupTypeId, [FromUri]int groupId, [FromUri]int groupParticipantId, [FromUri(Name = "removalMessage")]string removalMessage = null)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.RemoveParticipantFromMyGroup(token, groupTypeId, groupId, groupParticipantId, removalMessage);
                    return Ok();
                }
                catch (GroupParticipantRemovalException e)
                {
                    var apiError = new ApiErrorDto(e.Message, null, e.StatusCode);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto(string.Format("Error removing group participant {0} from group {1}", groupParticipantId, groupId), ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Search for a group matching the requested type and search terms.
        /// </summary>
        /// <param name="groupTypeId">An integer identifying the type of group to search for</param>
        /// <param name="keywords">The optional keywords to search for</param>
        /// <param name="location">The optional location/address to search for - if specified, the search results will include approximate distances from this address</param>
        /// <returns>A list of groups matching the terms</returns>
        [AcceptVerbs("GET")]
        [Route("api/grouptool/grouptype/{groupTypeId:int}/group/search")]
        [ResponseType(typeof(List<GroupDTO>))]
        public IHttpActionResult SearchGroups([FromUri] int groupTypeId, [FromUri(Name = "s")] string keywords = null, [FromUri(Name = "loc")] string location = null)
        {
            try
            {
                var result = _groupToolService.SearchGroups(groupTypeId, keywords, location);
                if (result == null || !result.Any())
                {
                    return RestHttpActionResult<List<GroupDTO>>.WithStatus(HttpStatusCode.NotFound, new List<GroupDTO>());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Error searching for group", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        /// <summary>
        /// Allows a group leader to accept or deny a group inquirier.
        /// </summary>
        /// <param name="groupTypeId">An integer identifying the type of group.</param>
        /// <param name="groupId">An integer identifying the group that the inquiry is associated to.</param>
        /// <param name="approve">A boolean showing if the inquiry is being approved or denied. It defaults to approved</param>
        /// <param name="inquiry">An Inquiry JSON Object.</param>
        [AcceptVerbs("POST")]
        [RequiresAuthorization]
        [Route("api/grouptool/grouptype/{groupTypeId:int}/group/{groupId:int}/inquiry/approve/{approve:bool}")]
        [HttpPost]
        public IHttpActionResult ApproveDenyInquiryFromMyGroup([FromUri]int groupTypeId, [FromUri]int groupId, [FromUri]bool approve, [FromBody]Inquiry inquiry)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.ApproveDenyInquiryFromMyGroup(token, groupTypeId, groupId, approve, inquiry, inquiry.Message);
                    return Ok();
                }
                catch (GroupParticipantRemovalException e)
                {
                    var apiError = new ApiErrorDto(e.Message, null, e.StatusCode);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto(string.Format("Error {0} group inquiry {1} from group {2}", approve, inquiry.InquiryId, groupId), ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Allows an invitee to accept or deny a group invitation.
        /// </summary>
        /// <param name="groupId">An integer identifying the group that the invitation is associated to.</param>
        /// <param name="invitationGuid">An string identifying the private invitation.</param>
        /// <param name="accept">A boolean showing if the invitation is being approved or denied.</param>
        [AcceptVerbs("POST")]
        [RequiresAuthorization]
        [Route("api/grouptool/group/{groupId:int}/invitation/{invitationGuid}")]
        [HttpPost]
        public IHttpActionResult ApproveDenyGroupInvitation([FromUri]int groupId, [FromUri]string invitationGuid, [FromBody]bool accept)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.AcceptDenyGroupInvitation(token, groupId, invitationGuid, accept);
                    return Ok();
                }
                catch (GroupParticipantRemovalException e)
                {
                    var apiError = new ApiErrorDto(e.Message, null, e.StatusCode);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto(string.Format("Error when accepting: {0}, for group {1}", accept, groupId), ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Send an email message to all members of a Group
        /// Requires the user to be a leader of the Group
        /// Will return a 404 if the user is not a Leader of the group
        /// </summary>
        [RequiresAuthorization]
        [Route("api/grouptool/{groupId}/{groupTypeId}/groupmessage")]
        public IHttpActionResult PostGroupMessage([FromUri()] int groupId, [FromUri()] int groupTypeId, GroupMessageDTO message)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.SendAllGroupParticipantsEmail(token, groupId, groupTypeId, message.Subject, message.Body);
                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    return (IHttpActionResult)NotFound();
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Error sending a Group email for groupID " + groupId, ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Return if the user is a group leader
        /// </summary>
        /// <param name="groupId">An integer identifying the group that we want to check if the user is a Leader of.</param>
        /// <param name="groupTypeId">An integer identifying the group type that we want to check if the user is a Leader of.</param>
        /// <returns>MyGroup</returns>
        [RequiresAuthorization]
        [AcceptVerbs("GET")]
        [ResponseType(typeof(MyGroup))]
        [Route("api/grouptool/{groupId}/{groupTypeId}/isleader")]
        [HttpGet]
        public IHttpActionResult GetIfIsGroupLeader(int groupId, int groupTypeId)
        {
            return Authorized(token =>
            {
                try
                {
                    var group = _groupToolService.VerifyCurrentUserIsGroupLeader(token, groupTypeId, groupId);

                    //Will return group if they are a group leader
                    return Ok(group);
                }
                catch(NotGroupLeaderException)
                {
                    //Will return empty group if they are not a group leader
                    return Ok(new MyGroup());
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Get if leader Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Create a group inquiry (typically to join a small group)
        /// </summary>
        /// <param name="groupId">An integer identifying the group</param>
        /// <param name="inquiry">The inquiry object submitted by a client.</param>
        [AcceptVerbs("POST")]
        [RequiresAuthorization]
        [Route("api/grouptool/group/{groupId:int}/submitinquiry")]
        [HttpPost]
        public IHttpActionResult SubmitGroupInquiry([FromUri()] int groupId, Inquiry inquiry)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.SubmitInquiry(token, groupId, inquiry);
                    return Ok();
                }
                catch (NotGroupLeaderException)
                {
                    return Conflict();
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto(string.Format("Error when creating inquiry: {0}, for group {1}", inquiry, groupId), ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
