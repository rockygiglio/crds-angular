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
using crds_angular.Models.Crossroads.Events;

namespace crds_angular.Controllers.API
{
    public class GroupController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private crds_angular.Services.Interfaces.IGroupService groupService;
        private IAuthenticationService authenticationService;
        private IParticipantService participantService;

        private readonly int GroupRoleDefaultId =
            Convert.ToInt32(ConfigurationManager.AppSettings["Group_Role_Default_ID"]);

        public GroupController(crds_angular.Services.Interfaces.IGroupService groupService,
                               IAuthenticationService authenticationService,
                               IParticipantService participantService)
        {
            this.groupService = groupService;
            this.authenticationService = authenticationService;
            this.participantService = participantService;
        }

        /// <summary>
        /// Create Group with provided details, returns created group with ID
        /// </summary>
        [ResponseType(typeof(GroupDTO))]
        [Route("api/group")]
        public IHttpActionResult PostGroup([FromBody] GroupDTO group)
        {
            return Authorized(token =>
            {
                try
                {
                    group = groupService.createGroup(group);
                    _logger.Debug(String.Format("Successfully created group {0} ", group.GroupId));
                    return (Created(String.Format("api/group/{0}", group.GroupId), group));
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
        /// Also send email confirmation to user
        /// </summary>
        [ResponseType(typeof (GroupDTO))]
        [Route("api/group/{groupId}/participants")]
        public IHttpActionResult Post(int groupId, [FromBody] List<ParticipantSignup> partId)
        {
            return Authorized(token =>
            {
                try
                {
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
                    return Ok(groups);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Error getting groups for group type ID " + groupTypeId, ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            });
        }
    }
}