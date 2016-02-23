using System;
using System.Collections.Generic;
using System.Configuration;
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
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Event = crds_angular.Models.Crossroads.Events.Event;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Services.Interfaces.IGroupService groupService;
        private readonly IAuthenticationService authenticationService;
        private readonly IParticipantService participantService;
        private readonly Services.Interfaces.IAddressService _addressService;

        private readonly int GroupRoleDefaultId =
            Convert.ToInt32(ConfigurationManager.AppSettings["Group_Role_Default_ID"]);

        public GroupController(Services.Interfaces.IGroupService groupService,
                               IAuthenticationService authenticationService,
                               IParticipantService participantService,
                               Services.Interfaces.IAddressService addressService)
        {
            this.groupService = groupService;
            this.authenticationService = authenticationService;
            this.participantService = participantService;
            _addressService = addressService;
        }

        /// <summary>
        /// Create Group with provided details, returns created group with ID
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(GroupDTO))]
        [Route("api/group")]
        public IHttpActionResult PostGroup([FromBody] GroupDTO group)
        {
            return Authorized(token =>
            {
                try
                {
                    if (group.Address != null && string.IsNullOrEmpty(group.Address.AddressLine1) == false)
                    {
                        _addressService.FindOrCreateAddress(group.Address);
                    }

                    group = groupService.CreateGroup(group);
                    _logger.DebugFormat("Successfully created group {0} ", group.GroupId);
                    return (Created(string.Format("api/group/{0}", group.GroupId), group));
                }
                catch (Exception e)
                {
                    _logger.Error("Could not create group", e);
                    return BadRequest();
                }
            });
        }

        /// <summary>
        /// Enroll the currently logged-in user into a Community Group, and register this user for all events for the CG.
        /// Also send email confirmation to user if joining a CG
        /// Or Add Journey/Small Group Participant to a Group 
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/participants")]
        public IHttpActionResult Post(int groupId, [FromBody] List<ParticipantSignup> partId)
        {
            return Authorized(token =>
            {
                try
                {
                    groupService.LookupParticipantIfEmpty(token, partId);

                    groupService.addParticipantsToGroup(groupId, partId);
                    _logger.Debug(String.Format("Successfully added participants {0} to group {1}", partId, groupId));
                    return (Ok());
                }
                catch (GroupFullException e)
                {
                    var responseMessage = new ApiErrorDto("Group Is Full", e).HttpResponseMessage;

                    // Using HTTP Status code 422/Unprocessable Entity to indicate Group Is Full
                    // http://tools.ietf.org/html/rfc4918#section-11.2
                    responseMessage.StatusCode = (HttpStatusCode) 422;
                    throw new HttpResponseException(responseMessage);
                }
                catch (Exception e)
                {
                    _logger.Error("Could not add user to group", e);
                    return BadRequest();
                }
            });
        }

        [RequiresAuthorization]
        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}")]
        public IHttpActionResult Get(int groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    var participant = participantService.GetParticipantRecord(token);
                    var contactId = authenticationService.GetContactId(token);

                    var detail = groupService.getGroupDetails(groupId, contactId, participant, token);

                    return Ok(detail);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Get Group", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            });
        }

        [RequiresAuthorization]
        [ResponseType(typeof(List<Event>))]
        [Route("api/group/{groupId}/events")]
        public IHttpActionResult GetEvents(int groupId)
        {
            return Authorized(token =>
                {
                    try
                    {
                        var eventList = groupService.GetGroupEvents(groupId, token);
                        return Ok(eventList);
                    }
                    catch (Exception e)
                    {
                        var apiError = new ApiErrorDto("Error getting events ", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                }
            );
        }

        [RequiresAuthorization]
        [ResponseType(typeof(List<GroupContactDTO>))]
        [Route("api/group/{groupId}/event/{eventId}")]
        public IHttpActionResult GetParticipants(int groupId, int eventId, string recipients)
        {
            return Authorized(token =>
                {
                    try
                    {
                        if (recipients != "current" && recipients != "potential")
                        {
                            throw new ApplicationException("Recipients should be 'current' or 'potential'");
                        }
                        var memberList = groupService.GetGroupMembersByEvent(groupId, eventId, recipients);
                        return Ok(memberList);
                    }
                    catch (Exception e)
                    {
                        var apiError = new ApiErrorDto("Error getting participating group members ", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                }
            );
        }

        /// <summary>
        /// This takes in a Group Type ID and retrieves all groups of that type for the current user.
        /// If one or more groups are found, then the group detail data is returned.
        /// If no groups are found, then a 404 will be returned.
        /// </summary>
        /// <param name="groupTypeId">This is the Ministry Platform Group Type ID for the specific group being requested..</param>
        /// <returns>A list of all groups for the given user based on the Group Type ID passed in.</returns>
        [RequiresAuthorization]
        [ResponseType(typeof (List<GroupContactDTO>))]
        [Route("api/group/groupType/{groupTypeId}")]
        public IHttpActionResult GetGroups(int groupTypeId)
        {
            return Authorized(token =>
            {
                try
                {
                    var participant = participantService.GetParticipantRecord(token);
                    var groups = groupService.GetGroupsByTypeForParticipant(token, participant.ParticipantId, groupTypeId);
                    return groups == null ? (IHttpActionResult) NotFound() : Ok(groups);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Error getting groups for group type ID " + groupTypeId, ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            });
        }

        /// <summary>
        /// Takes in a Group ID and retrieves all active participants for the group id.
        /// </summary>
        /// <param name="groupId">GroupId of the group.</param>
        /// <returns>A list of active participants for the group id passed in.</returns>
        [RequiresAuthorization]
        [ResponseType(typeof(List<GroupParticipantDTO>))]
        [Route("api/group/{groupId}/participants")]
        public IHttpActionResult GetGroupParticipants(int groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    var participants = groupService.GetGroupParticipants(groupId);
                    return participants == null ? (IHttpActionResult)NotFound() : Ok(participants);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Error getting participants for group ID " + groupId, ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }
    }
}