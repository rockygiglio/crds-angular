using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = crds_angular.Models.Crossroads.Events.Event;
using IEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;
using IObjectAttributeService = crds_angular.Services.Interfaces.IObjectAttributeService;

namespace crds_angular.Services
{
    public class GroupService : crds_angular.Services.Interfaces.IGroupService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (GroupService));

        private readonly IGroupService _mpGroupService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IEventService _eventService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IServeService _serveService;
        private readonly IParticipantService _participantService;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IObjectAttributeService _objectAttributeService;
        private readonly IApiUserService _apiUserService;


        /// <summary>
        /// This is retrieved in the constructor from AppSettings
        /// </summary>
        private readonly int GroupRoleDefaultId;
        private readonly int DefaultContactEmailId;
        private readonly int MyCurrentGroupsPageView;

        public GroupService(IGroupService mpGroupService,
                            IConfigurationWrapper configurationWrapper,
                            IEventService eventService,
                            IContactRelationshipService contactRelationshipService,
                            IServeService serveService,
                            IParticipantService participantService,
                            ICommunicationService communicationService,
                            IContactService contactService, 
                            IObjectAttributeService objectAttributeService, 
                            IApiUserService apiUserService)

        {
            _mpGroupService = mpGroupService;
            _configurationWrapper = configurationWrapper;
            _eventService = eventService;
            _contactRelationshipService = contactRelationshipService;
            _serveService = serveService;
            _participantService = participantService;
            _communicationService = communicationService;
            _contactService = contactService;
            _objectAttributeService = objectAttributeService;
            _apiUserService = apiUserService;

            GroupRoleDefaultId = Convert.ToInt32(_configurationWrapper.GetConfigIntValue("Group_Role_Default_ID"));
            DefaultContactEmailId = _configurationWrapper.GetConfigIntValue("DefaultContactEmailId");
        }

        public GroupDTO CreateGroup(GroupDTO group)
        {
            try
            {
                var mpGroup = Mapper.Map<Group>(group);
                group.GroupId = _mpGroupService.CreateGroup(mpGroup);

                var configuration = ObjectAttributeConfigurationFactory.Group();
                _objectAttributeService.SaveObjectAttributes(group.GroupId, group.AttributeTypes, group.SingleAttributes, configuration);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not create group {0}", group.GroupName, e.Message);
                logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            return group;
        }

        public void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants)
        {
            Group group;
            try
            {
                group = _mpGroupService.getGroupDetails(groupId);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not retrieve group details for group {0}: {1}", groupId, e.Message);
                logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            checkSpaceRemaining(participants, group);

            try
            {
                foreach (var participant in participants)
                {
                    int groupParticipantId;

                    var roleId = participant.groupRoleId ?? GroupRoleDefaultId;

                    var participantId = participant.particpantId.Value;
                    groupParticipantId = _mpGroupService.addParticipantToGroup(participantId,
                                                               Convert.ToInt32(groupId),
                                                               roleId,
                                                               participant.childCareNeeded,
                                                               DateTime.Now);

                    var configuration = ObjectAttributeConfigurationFactory.GroupParticipant();
                    _objectAttributeService.SaveObjectAttributes(groupParticipantId, participant.AttributeTypes, participant.SingleAttributes, configuration);                    

                    if (participant.capacityNeeded > 0)
                    {
                        decrementCapacity(participant.capacityNeeded, group);
                    }

                    logger.Debug("Added user - group/participant id = " + groupParticipantId);

                    // Now see what future events are scheduled for this group, and register the user for those
                    var events = _mpGroupService.getAllEventsForGroup(Convert.ToInt32(groupId));
                    logger.Debug("Scheduled events for this group: " + events);
                    if (events != null && events.Count > 0)
                    {
                        foreach (var e in events)
                        {
                            _eventService.RegisterParticipantForEvent(participantId, e.EventId, groupId, groupParticipantId);
                            logger.Debug("Added participant " + participant + " to group event " + e.EventId);
                        }
                    }

                    if (participant.SendConfirmationEmail)
                    {
                        var waitlist = group.GroupType == _configurationWrapper.GetConfigIntValue("GroupType_Waitlist");
                        _mpGroupService.SendCommunityGroupConfirmationEmail(participantId, groupId, waitlist, participant.childCareNeeded);
                    }
                }

                return;
            }
            catch (Exception e)
            {
                logger.Error("Could not add user to group", e);
                throw;
            }
        }

        private void checkSpaceRemaining(List<ParticipantSignup> participants, Group group)
        {
            var numParticipantsToAdd = participants.Count;
            var spaceRemaining = group.TargetSize - group.Participants.Count;
            if (((group.TargetSize > 0) && (numParticipantsToAdd > spaceRemaining)) || (group.Full))
            {
                throw (new GroupFullException(group));
            }
        }

        private void decrementCapacity(int capacityNeeded, Group group)
        {
            group.RemainingCapacity = group.RemainingCapacity - capacityNeeded;
            logger.Debug("Remaining Capacity After decrement: " + capacityNeeded + " : " + group.RemainingCapacity);
            _mpGroupService.UpdateGroupRemainingCapacity(group);
        }

        public List<Event> GetGroupEvents(int groupId, string token)
        {
            var eventTypes = _mpGroupService.GetEventTypesForGroup(groupId, token);
            var events = new List<MinistryPlatform.Models.Event>();
            foreach (var eventType in eventTypes.Where(eventType => !string.IsNullOrEmpty(eventType)))
            {
                events.AddRange(_eventService.GetEvents(eventType, token));
            }
            var futureEvents = events.Where(e => e.EventStartDate >= DateTime.Now).OrderBy(e => e.EventStartDate);
            var eventList = Mapper.Map<List<Event>>(futureEvents.GroupBy(x => x.EventId).Select(y => y.First()));
            return eventList;
        }

        public List<GroupContactDTO> GetGroupMembersByEvent(int groupId, int eventId, string recipients)
        {
            return recipients == "current" ? GroupMembersThatAreServingForEvent(groupId, eventId) : GroupMembersThatHaveNotRepliedToEvent(groupId, eventId);
        }

        private List<GroupContactDTO> GroupMembersThatAreServingForEvent(int groupId, int eventId)
        {
            return _mpGroupService.getEventParticipantsForGroup(groupId, eventId)
                .Select(part => new GroupContactDTO
                {
                    ContactId = part.ContactId,
                    DisplayName = part.LastName + ", " + part.NickName
                }).ToList();
        }

        private List<GroupContactDTO> GroupMembersThatHaveNotRepliedToEvent(int groupId, int eventId)
        {
            try
            {
                var groupMembers = _mpGroupService.getGroupDetails(groupId).Participants.Select(p =>
                                                                                                    new GroupParticipant
                                                                                                    {
                                                                                                        ContactId = p.ContactId,
                                                                                                        LastName = p.LastName,
                                                                                                        NickName = p.NickName,
                                                                                                        ParticipantId = p.ParticipantId
                                                                                                    }
                    ).ToList();
                var evt = Mapper.Map<crds_angular.Models.Crossroads.Events.Event>(_eventService.GetEvent(eventId));
                return _serveService.PotentialVolunteers(groupId, evt, groupMembers);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        public GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken)
        {
            int participantId = participant.ParticipantId;
            Group g = _mpGroupService.getGroupDetails(groupId);

            var signupRelations = _mpGroupService.GetGroupSignupRelations(g.GroupType);

            var currRelationships = _contactRelationshipService.GetMyCurrentRelationships(contactId, authUserToken);

            var events = _mpGroupService.getAllEventsForGroup(groupId);

            ContactRelationship[] familyToReturn = null;

            if (currRelationships != null)
            {
                familyToReturn = currRelationships.Where(
                    c => signupRelations.Select(s => s.RelationshipId).Contains(c.Relationship_Id)).ToArray();
            }

            var apiToken = _apiUserService.GetToken();
            var configuration = ObjectAttributeConfigurationFactory.Group();
            var attributesTypes = _objectAttributeService.GetObjectAttributes(apiToken, groupId, configuration);

            var detail = new GroupDTO();
            {
                detail.GroupName = g.Name;
                detail.GroupId = g.GroupId;
                detail.GroupFullInd = g.Full;
                detail.WaitListInd = g.WaitList;
                detail.ChildCareAvailable = g.ChildCareAvailable;
                detail.WaitListGroupId = g.WaitListGroupId;
                detail.OnlineRsvpMinimumAge = g.MinimumAge;
                if (events != null)
                {
                    detail.Events = events.Select(Mapper.Map<MinistryPlatform.Models.Event, crds_angular.Models.Crossroads.Events.Event>).ToList();
                }
                //the first instance of family must always be the logged in user
                var fam = new SignUpFamilyMembers
                {
                    EmailAddress = participant.EmailAddress,
                    PreferredName = participant.PreferredName,
                    UserInGroup = _mpGroupService.checkIfUserInGroup(participantId, g.Participants),
                    ParticpantId = participantId,
                    ChildCareNeeded = false
                };
                detail.SignUpFamilyMembers = new List<SignUpFamilyMembers> {fam};

                if (familyToReturn != null)
                {
                    foreach (var f in familyToReturn)
                    {
                        var fm = new SignUpFamilyMembers
                        {
                            EmailAddress = f.Email_Address,
                            PreferredName = f.Preferred_Name,
                            UserInGroup = _mpGroupService.checkIfUserInGroup(f.Participant_Id, g.Participants),
                            ParticpantId = f.Participant_Id,
                        };
                        detail.SignUpFamilyMembers.Add(fm);
                    }
                }

                detail.AttributeTypes = attributesTypes.MultiSelect;
                detail.SingleAttributes = attributesTypes.SingleSelect;
            }

            return (detail);
        }

        public List<GroupDTO> GetGroupsByTypeForParticipant(string token, int participantId, int groupTypeId)
        {
            var groupsByType = _mpGroupService.GetGroupsByTypeForParticipant(token, participantId, groupTypeId);
            if (groupsByType == null)
            {
                return null;
            }

            var groupDetail = groupsByType.Select(Mapper.Map<Group, GroupDTO>).ToList();

            var configuration = ObjectAttributeConfigurationFactory.Group();
            foreach (var group in groupDetail)
            {               
                var attributesTypes = _objectAttributeService.GetObjectAttributes(token, group.GroupId, configuration);
                group.AttributeTypes = attributesTypes.MultiSelect;
                group.SingleAttributes = attributesTypes.SingleSelect;
            }


            return groupDetail;
        }
        
        public Participant GetParticipantRecord(string token) 
        {
            var participant = _participantService.GetParticipantRecord(token);            
            return participant;
        }

        public void SendJourneyEmailInvite(EmailCommunicationDTO communication, string token)
        {
            var participant = GetParticipantRecord(token);
            var groups = GetGroupsByTypeForParticipant(token, participant.ParticipantId, 19);

            if (groups == null ||  groups.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var membership = groups.Where(group => @group.GroupId == communication.groupId).ToList();
            if (membership.Count <= 0)
            {
                throw new InvalidOperationException();
            }

            var invitation = CreateJourneyInvitation(communication, participant);
            _communicationService.SendMessage(invitation);
        }


        private Communication CreateJourneyInvitation(EmailCommunicationDTO communication, Participant particpant)
        {
            var template = _communicationService.GetTemplate(communication.TemplateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            var replyTo = _contactService.GetContactById(particpant.ContactId);
            var mergeData = SetupMergeData(particpant.PreferredName, communication.groupId.Value);

            return new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new Contact { ContactId = DefaultContactEmailId, EmailAddress = fromContact.Email_Address },
                ReplyToContact = new Contact {ContactId = DefaultContactEmailId, EmailAddress = replyTo.Email_Address },
                ToContacts = new List<Contact> { new Contact { ContactId = fromContact.Contact_ID, EmailAddress = communication.emailAddress } },
                MergeData = mergeData
            };
        }

        private Dictionary<string, object> SetupMergeData(string preferredName, int groupId)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"BaseUrl", _configurationWrapper.GetConfigValue("BaseUrl")},
                {"GroupId", groupId},
                {"PreferredName", preferredName},
            };
            return mergeData;
        }

        public List<GroupParticipantDTO> GetGroupParticipants(int groupId)
        {
            var groupParticipants = _mpGroupService.GetGroupParticipants(groupId);       
            if (groupParticipants == null)
            {
                return null;
            }
            var participants = groupParticipants.Select(Mapper.Map<GroupParticipant, GroupParticipantDTO>).ToList();

            var configuration = ObjectAttributeConfigurationFactory.GroupParticipant();

            var apiToken = _apiUserService.GetToken();            
                        
            foreach (var participant in participants)
            {
                var attributesTypes = _objectAttributeService.GetObjectAttributes(apiToken, participant.GroupParticipantId, configuration);
                participant.AttributeTypes = attributesTypes.MultiSelect;
                participant.SingleAttributes = attributesTypes.SingleSelect;
            }

            return participants;
        }

        public void LookupParticipantIfEmpty(string token, List<ParticipantSignup> partId)
        {
            var participantsToLookup = partId.Where(x => x.particpantId == null).ToList();
            if (participantsToLookup.Count <= 0)
            {
                return;
            }

            var participant = _participantService.GetParticipantRecord(token);

            foreach (var currentParticpant in participantsToLookup)
            {
                currentParticpant.particpantId = participant.ParticipantId;
            }
        }
    }
}
