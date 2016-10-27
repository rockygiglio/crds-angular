using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Models.Json;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using log4net;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class GroupToolController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Services.Interfaces.IGroupToolService _groupToolService;
        private readonly int _defaultGroupTypeId;
        private readonly IConfigurationWrapper _configurationWrapper;

        public GroupToolController(Services.Interfaces.IGroupToolService groupToolService,
            IConfigurationWrapper configurationWrapper)
        {
            _groupToolService = groupToolService;
            _configurationWrapper = configurationWrapper;
            _defaultGroupTypeId = _configurationWrapper.GetConfigIntValue("GroupTypeSmallId");
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
        [VersionedRoute(template: "groupTool/invitations/{sourceId}/{invitationTypeId}", minimumVersion: "1.0.0")]
        [Route("grouptool/invitations/{sourceId}/{invitationTypeId}")]
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
        [VersionedRoute(template: "groupTool/inquiries/{groupId}", minimumVersion: "1.0.0")]
        [Route("grouptool/inquiries/{groupId}")]
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
        /// Ends a group and emails all participants to let them know
        /// it is over
        /// </summary>
        /// <param name="groupId">The id of a group</param>
        /// <param name="groupReasonEndedId">The id of the reason the group was ended</param>
        /// <returns>Http Result</returns>
        [AcceptVerbs("POST")]
        [RequiresAuthorization]
        [VersionedRoute(template: "groupTool/{groupId:int}/endSmallGroup", minimumVersion: "1.0.0")]
        [Route("grouptool/{groupId:int}/endsmallgroup")]
        [HttpPost]
        public IHttpActionResult EndSmallGroup([FromUri]int groupId, [FromUri]int groupReasonEndedId)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.VerifyCurrentUserIsGroupLeader(token, groupId);
                    _groupToolService.EndGroup(groupId, groupReasonEndedId);
                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.Error("Could not end group: " + groupId, e);
                    return BadRequest();
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
        [VersionedRoute(template: "grouptool/groupType/{groupTypeId}/group/{groupId}/participant/{groupParticipantId}", minimumVersion: "1.0.0")]
        [Route("grouptool/grouptype/{groupTypeId:int}/group/{groupId:int}/participant/{groupParticipantId:int}")]
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
        [VersionedRoute(template: "groupTool/groupType/{groupTypeId}/group/search", minimumVersion: "1.0.0")]
        [Route("grouptool/grouptype/{groupTypeId:int}/group/search")]
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
        [RequiresAuthorization]
        [VersionedRoute(template: "groupTool/groupType/{groupTypeId}/group/{groupId}/inquiry/approve/{approve}", minimumVersion: "1.0.0")]
        [Route("grouptool/grouptype/{groupTypeId:int}/group/{groupId:int}/inquiry/approve/{approve:bool}")]
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
        [RequiresAuthorization]
        [VersionedRoute(template: "groupTool/group/{groupId}/invitation/{invitationKey}", minimumVersion: "1.0.0")]
        [Route("grouptool/group/{groupId:int}/invitation/{invitationGuid}")]
        [HttpPost]
        public IHttpActionResult ApproveDenyGroupInvitation([FromUri]int groupId, [FromUri]string invitationKey, [FromBody]bool accept)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.AcceptDenyGroupInvitation(token, groupId, invitationKey, accept);
                    return Ok();
                }
                catch (GroupParticipantRemovalException e)
                {
                    var apiError = new ApiErrorDto(e.Message, null, e.StatusCode);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                catch (DuplicateGroupParticipantException e)
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
        /// Send an email message to all leaders of a Group
        /// </summary>
        /// <param name="groupId">An integer identifying the group that the inquiry is associated to.</param>
        /// <param name="message">A Group Message DTO that holds the subject and body of the email</param>
        [RequiresAuthorization]
        [VersionedRoute(template: "groupTool/group/{groupId}/leaderMessage", minimumVersion: "1.0.0")]
        [Route("grouptool/{groupId}/leadermessage")]
        public IHttpActionResult PostGroupLeaderMessage([FromUri()] int groupId, GroupMessageDTO message)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.SendAllGroupLeadersEmail(token, groupId, message);
                    return Ok();
                }
                catch (InvalidOperationException)
                {
                    return (IHttpActionResult)NotFound();
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Error sending a Leader email to groupID " + groupId, ex);
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
        [VersionedRoute(template: "groupTool/group/{groupId}/invitation/{invitationGuid}", minimumVersion: "1.0.0")]
        [Route("grouptool/{groupId}/{groupTypeId}/groupmessage")]
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
        [VersionedRoute(template: "groupTool/{groupId}/{groupTypeId}/isLeader", minimumVersion: "1.0.0")]
        [Route("grouptool/{groupId}/{groupTypeId}/isleader")]
        [HttpGet]
        public IHttpActionResult GetIfIsGroupLeader(int groupId, int groupTypeId)
        {
            return Authorized(token =>
            {
                try
                {
                    var group = _groupToolService.VerifyCurrentUserIsGroupLeader(token, groupId);
                    return Ok(group);
                }
                catch (GroupNotFoundForParticipantException exception)
                {
                    var apiError = new ApiErrorDto("User is not in GroupId: " + groupId, exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                catch (NotGroupLeaderException)
                {
                    //Will return empty group if they are not a group leader
                    return Ok(new MyGroup());
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Error while verify Group Leader of GroupId: " + groupId, exception);
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
        [VersionedRoute(template: "groupTool/group/{groupId}/submitInquiry", minimumVersion: "1.0.0")]
        [Route("grouptool/group/{groupId:int}/submitinquiry")]
        [HttpPost]
        public IHttpActionResult SubmitGroupInquiry([FromUri()] int groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    _groupToolService.SubmitInquiry(token, groupId);
                    return Ok();
                }
                catch (ExistingRequestException)
                {
                    return Conflict();
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto(string.Format("Error when creating inquiry for group {0}", groupId), ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
