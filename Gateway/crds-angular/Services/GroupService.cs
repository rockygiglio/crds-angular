using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Newtonsoft.Json;
using Event = crds_angular.Models.Crossroads.Events.Event;
using IAttributeRepository = MinistryPlatform.Translation.Repositories.Interfaces.IAttributeRepository;
using IEventRepository = MinistryPlatform.Translation.Repositories.Interfaces.IEventRepository;
using IGroupRepository = MinistryPlatform.Translation.Repositories.Interfaces.IGroupRepository;
using IObjectAttributeService = crds_angular.Services.Interfaces.IObjectAttributeService;
using Participant = MinistryPlatform.Translation.Models.Participant;

namespace crds_angular.Services
{
    public class GroupService : IGroupService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (GroupService));

        private readonly IGroupRepository _mpGroupService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IEventRepository _eventService;
        private readonly IContactRelationshipRepository _contactRelationshipService;
        private readonly IServeService _serveService;
        private readonly IParticipantRepository _participantService;
        private readonly ICommunicationRepository _communicationService;
        private readonly IContactRepository _contactService;
        private readonly IObjectAttributeService _objectAttributeService;
        private readonly IApiUserRepository _apiUserService;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IEmailCommunication _emailCommunicationService;
        private readonly IUserRepository _userRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAttributeService _attributeService;


        /// <summary>
        /// This is retrieved in the constructor from AppSettings
        /// </summary>
        private readonly int _groupRoleDefaultId;
        private readonly int _defaultContactEmailId;
        private readonly int _journeyGroupId;
        private readonly int _groupCategoryAttributeTypeId;
        private readonly int _groupTypeAttributeTypeId;
        private readonly int _groupAgeRangeAttributeTypeId;
        private readonly int _groupRoleLeader;


        public GroupService(IGroupRepository mpGroupService,
                            IConfigurationWrapper configurationWrapper,
                            IEventRepository eventService,
                            IContactRelationshipRepository contactRelationshipService,
                            IServeService serveService,
                            IParticipantRepository participantService,
                            ICommunicationRepository communicationService,
                            IContactRepository contactService, 
                            IObjectAttributeService objectAttributeService, 
                            IApiUserRepository apiUserService, 
                            IAttributeRepository attributeRepository,
                            IEmailCommunication emailCommunicationService,
                            IUserRepository userRepository,
                            IInvitationRepository invitationRepository,
                            IAttributeService attributeService)

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
            _attributeRepository = attributeRepository;
            _emailCommunicationService = emailCommunicationService;
            _userRepository = userRepository;
            _invitationRepository = invitationRepository;
            _attributeService = attributeService;

            _groupRoleDefaultId = _configurationWrapper.GetConfigIntValue("Group_Role_Default_ID");
            _defaultContactEmailId = _configurationWrapper.GetConfigIntValue("DefaultContactEmailId");
            _journeyGroupId = configurationWrapper.GetConfigIntValue("JourneyGroupId");
            _groupCategoryAttributeTypeId = configurationWrapper.GetConfigIntValue("GroupCategoryAttributeTypeId");
            _groupTypeAttributeTypeId = configurationWrapper.GetConfigIntValue("GroupTypeAttributeTypeId");
            _groupAgeRangeAttributeTypeId = configurationWrapper.GetConfigIntValue("GroupAgeRangeAttributeTypeId");
            _groupRoleLeader = configurationWrapper.GetConfigIntValue("GroupRoleLeader");
        }

        public GroupDTO CreateGroup(GroupDTO group)
        {
            try
            {
                var mpGroup = Mapper.Map<MpGroup>(group);

                if (group.AttributeTypes.ContainsKey(_groupCategoryAttributeTypeId) && group.AttributeTypes[90].Attributes.Any(a => a.AttributeId == 0))
                {
                    var categoryAttributes = Mapper.Map<List<MpAttribute>>(group.AttributeTypes[90].Attributes);

                    categoryAttributes = _attributeService.CreateMissingAttributes(categoryAttributes, _groupCategoryAttributeTypeId);
                    group.AttributeTypes[_groupCategoryAttributeTypeId].Attributes = Mapper.Map<List<ObjectAttributeDTO>>(categoryAttributes);
                }

                group.GroupId = _mpGroupService.CreateGroup(mpGroup);
                //save group attributes
                var configuration = MpObjectAttributeConfigurationFactory.Group();            
                _objectAttributeService.SaveObjectAttributes(group.GroupId, group.AttributeTypes, group.SingleAttributes, configuration);

                if (group.MinorAgeGroupsAdded)
                {
                    _mpGroupService.SendNewStudentMinistryGroupAlertEmail((List<MpGroupParticipant>) mpGroup.Participants);
                }
            }
            catch (Exception e)
            {
                var message = String.Format("Could not create group {0}", group.GroupName);
                _logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            return group;
        }

        public void addParticipantToGroupNoEvents(int groupId, ParticipantSignup participant)
        {
            MpGroup group;
            try
            {
                group = _mpGroupService.getGroupDetails(groupId);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not retrieve group details for group {0}: {1}", groupId, e.Message);
                _logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            checkSpaceRemaining(new List<ParticipantSignup> {participant}, group);

            try
            {
                var roleId = participant.groupRoleId ?? _groupRoleDefaultId;

                var participantId = participant.particpantId.Value;
                var groupParticipantId = _mpGroupService.addParticipantToGroup(participantId,
                                                                           Convert.ToInt32(groupId),
                                                                           roleId,
                                                                           participant.childCareNeeded,
                                                                           DateTime.Now,
                                                                           null, false, participant.EnrolledBy);

                var configuration = MpObjectAttributeConfigurationFactory.GroupParticipant();
                _objectAttributeService.SaveObjectAttributes(groupParticipantId, participant.AttributeTypes, participant.SingleAttributes, configuration);

                if (participant.capacityNeeded > 0)
                {
                    DecrementCapacity(participant.capacityNeeded, group);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Could not add user to group", e);
                throw;
            }
        }

        public void endDateGroupParticipant(int groupId, int groupParticipantId)
        {
            
           _mpGroupService.endDateGroupParticipant(groupParticipantId,groupId, DateTime.Now);
        }

        public void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants)
        {
            MpGroup group;
            try
            {
                group = _mpGroupService.getGroupDetails(groupId);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not retrieve group details for group {0}: {1}", groupId, e.Message);
                _logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            checkSpaceRemaining(participants, group);

            try
            {
                foreach (var participant in participants)
                {
                    int groupParticipantId;

                    var roleId = participant.groupRoleId ?? _groupRoleDefaultId;

                    var participantId = participant.particpantId.Value;
                    groupParticipantId = _mpGroupService.addParticipantToGroup(participantId,
                                                               Convert.ToInt32(groupId),
                                                               roleId,
                                                               participant.childCareNeeded,
                                                               DateTime.Now);

                    var configuration = MpObjectAttributeConfigurationFactory.GroupParticipant();
                    _objectAttributeService.SaveObjectAttributes(groupParticipantId, participant.AttributeTypes, participant.SingleAttributes, configuration);                    

                    if (participant.capacityNeeded > 0)
                    {
                        DecrementCapacity(participant.capacityNeeded, group);
                    }

                    _logger.Debug("Added user - group/participant id = " + groupParticipantId);

                    // Now see what future events are scheduled for this group, and register the user for those
                    var events = _mpGroupService.getAllEventsForGroup(Convert.ToInt32(groupId));
                    _logger.Debug("Scheduled events for this group: " + events);
                    if (events != null && events.Count > 0)
                    {
                        foreach (var e in events.Where(x => x.EventType != "Childcare"))
                        {
                            _eventService.RegisterParticipantForEvent(participantId, e.EventId, groupId, groupParticipantId);
                            _logger.Debug("Added participant " + participant + " to group event " + e.EventId);
                        }
                    }

                    if (participant.SendConfirmationEmail)
                    {
                        var waitlist = group.GroupType == _configurationWrapper.GetConfigIntValue("GroupType_Waitlist");
                        _mpGroupService.SendCommunityGroupConfirmationEmail(participantId, groupId, waitlist, participant.childCareNeeded);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("Could not add user to group", e);
                throw;
            }
        }

        private void checkSpaceRemaining(List<ParticipantSignup> participants, MpGroup group)
        {
            var numParticipantsToAdd = participants.Count;
            var spaceRemaining = group.TargetSize - group.Participants.Count;
            if (((group.TargetSize > 0) && (numParticipantsToAdd > spaceRemaining)) || (group.Full))
            {
                throw (new GroupFullException(group));
            }
        }

        private void DecrementCapacity(int capacityNeeded, MpGroup group)
        {
            group.RemainingCapacity = group.RemainingCapacity - capacityNeeded;
            _logger.Debug("Remaining Capacity After decrement: " + capacityNeeded + " : " + group.RemainingCapacity);
            _mpGroupService.UpdateGroupRemainingCapacity(group);
        }

        public void addContactToGroup(int groupId, int contactId)
        {
            Participant participant;

            try
            {
                participant = _participantService.GetParticipant(contactId);
            }
            catch (Exception e)
            {
                var message = string.Format("Could not retrieve particpant for contact {0}: {1}", contactId, e.Message);
                _logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            try
            {
                _mpGroupService.addParticipantToGroup(participant.ParticipantId, groupId, _groupRoleDefaultId, false, DateTime.Now);
            }
            catch (Exception e)
            {
                _logger.Error("Could not add contact to group", e);
                throw;
            }
        }

        public List<Event> GetGroupEvents(int groupId, string token)
        {
            var eventTypes = _mpGroupService.GetEventTypesForGroup(groupId, token);
            var events = new List<MpEvent>();
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
                                                                                                    new MpGroupParticipant
                                                                                                    {
                                                                                                        ContactId = p.ContactId,
                                                                                                        LastName = p.LastName,
                                                                                                        NickName = p.NickName,
                                                                                                        ParticipantId = p.ParticipantId
                                                                                                    }
                    ).ToList();
                var evt = Mapper.Map<Event>(_eventService.GetEvent(eventId));
                return _serveService.PotentialVolunteers(groupId, evt, groupMembers);
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        public GroupDTO GetGroupDetails(int groupId)
        {
            return Mapper.Map<MpGroup, GroupDTO>(_mpGroupService.getGroupDetails(groupId));
        }

        public GroupDTO GetGroupDetailsByInvitationGuid(string token, string invitationGuid)
        {
            var invitation = _invitationRepository.GetOpenInvitation(invitationGuid);

            var group = Mapper.Map<MpGroup, GroupDTO>(_mpGroupService.GetSmallGroupDetailsById(invitation.SourceId));
            var groups = new List<GroupDTO> {@group};

            GetGroupAttributes(token, groups);
            GetGroupParticipants(groups);

            return groups[0];
        }

        public GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken)
        {
            int participantId = participant.ParticipantId;
            MpGroup g = _mpGroupService.getGroupDetails(groupId);

            var signupRelations = _mpGroupService.GetGroupSignupRelations(g.GroupType);

            var currRelationships = _contactRelationshipService.GetMyCurrentRelationships(contactId, authUserToken);

            var events = _mpGroupService.getAllEventsForGroup(groupId);

            MpContactRelationship[] familyToReturn = null;

            if (currRelationships != null)
            {
                familyToReturn = currRelationships.Where(
                    c => signupRelations.Select(s => s.RelationshipId).Contains(c.Relationship_Id)).ToArray();
            }

            var apiToken = _apiUserService.GetToken();
            var configuration = MpObjectAttributeConfigurationFactory.Group();
            var attributesTypes = _objectAttributeService.GetObjectAttributes(apiToken, groupId, configuration);

            var detail = new GroupDTO();
            {
                detail.ContactId = g.ContactId;
                detail.CongregationId = g.CongregationId;
                detail.KidsWelcome = g.KidsWelcome;
                detail.GroupName = g.Name;
                detail.GroupDescription = g.GroupDescription;
                detail.GroupId = g.GroupId;
                detail.GroupFullInd = g.Full;
                detail.WaitListInd = g.WaitList;
                detail.ChildCareAvailable = g.ChildCareAvailable;
                detail.WaitListGroupId = g.WaitListGroupId;
                detail.OnlineRsvpMinimumAge = g.MinimumAge;
                detail.MeetingFrequencyID = g.MeetingFrequencyID;
                detail.AvailableOnline = g.AvailableOnline;
                detail.MeetingTime = g.MeetingTime;
                detail.MeetingDayId = g.MeetingDayId;
                detail.Address = Mapper.Map<MpAddress, AddressDTO>(g.Address);
                detail.StartDate = g.StartDate;
                detail.Participants = (g.Participants.Count > 0) ? g.Participants.Select(p => Mapper.Map<MpGroupParticipant, GroupParticipantDTO>(p)).ToList() : null;

                if (events != null)
                {
                    detail.Events = events.Select(Mapper.Map<MpEvent, Event>).ToList();
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

        public List<GroupDTO> GetGroupsForParticipant(string token, int participantId)
        {
            var groups = _mpGroupService.GetGroupsForParticipant(token, participantId);
            if (groups == null)
                return null;

            var groupDetail = groups.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();
            var configuration = MpObjectAttributeConfigurationFactory.Group();
            var mpAttributes = _attributeRepository.GetAttributes(null);
            foreach (var group in groupDetail)
            {
                var attributesTypes = _objectAttributeService.GetObjectAttributes(token, group.GroupId, configuration, mpAttributes);
                group.AttributeTypes = attributesTypes.MultiSelect;
                group.SingleAttributes = attributesTypes.SingleSelect;
            }

            return groupDetail;
        }

        public List<GroupDTO> GetGroupsByTypeForParticipant(string token, int participantId, int groupTypeId)
        {
            var groupsByType = _mpGroupService.GetGroupsByTypeForParticipant(token, participantId, groupTypeId);
            if (groupsByType == null)
            {
                return null;
            }

            var groupDetail = groupsByType.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();

            var configuration = MpObjectAttributeConfigurationFactory.Group();
            var mpAttributes = _attributeRepository.GetAttributes(null);
            foreach (var group in groupDetail)
            {               
                var attributesTypes = _objectAttributeService.GetObjectAttributes(token, group.GroupId, configuration, mpAttributes);
                group.AttributeTypes = attributesTypes.MultiSelect;
                group.SingleAttributes = attributesTypes.SingleSelect;
            }

            return groupDetail;
        }

        public List<GroupDTO> GetGroupsByTypeForAuthenticatedUser(string token, int groupTypeId, int? groupId = null)
        {
            var groupsByType = _mpGroupService.GetMyGroupParticipationByType(token, groupTypeId, groupId);
            if (groupsByType == null)
            {
                return null;
            }

            var groupDetail = groupsByType.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();

            GetGroupAttributes(token, groupDetail);
            GetGroupParticipants(groupDetail);

            return groupDetail;
        }

        private void GetGroupParticipants(List<GroupDTO> groups)
        {
            foreach (var group in groups)
            {
                var p = _mpGroupService.GetGroupParticipants(group.GroupId, true);
                if (p != null && p.Any())
                {
                    group.Participants = p.Select(Mapper.Map<MpGroupParticipant, GroupParticipantDTO>).ToList();
                }
            }
        }

        private void GetGroupAttributes(string token, List<GroupDTO> groups)
        {
            var configuration = MpObjectAttributeConfigurationFactory.Group();

            foreach (var attributeType in new[] { _groupCategoryAttributeTypeId, _groupTypeAttributeTypeId, _groupAgeRangeAttributeTypeId })
            {
                var types = _attributeRepository.GetAttributes(attributeType);
                foreach (var group in groups)
                {
                    var attributesTypes = _objectAttributeService.GetObjectAttributes(token, group.GroupId, configuration, types);
                    foreach (var multi in attributesTypes.MultiSelect)
                    {
                        group.AttributeTypes.Add(multi.Key, multi.Value);
                    }
                    foreach (var single in attributesTypes.SingleSelect)
                    {
                        group.SingleAttributes.Add(single.Key, single.Value);
                    }
                }
            }
        }

        public void EndDateGroup(int groupId, int? reasonEndedId)
        {
            _mpGroupService.EndDateGroup(groupId, DateTime.Now, reasonEndedId);
        }

        public Participant GetParticipantRecord(string token) 
        {
            var participant = _participantService.GetParticipantRecord(token);            
            return participant;
        }

        public void SendJourneyEmailInvite(EmailCommunicationDTO communication, string token)
        {
            var participant = GetParticipantRecord(token);
            var groups = GetGroupsByTypeForParticipant(token, participant.ParticipantId, _journeyGroupId);

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


        private MpCommunication CreateJourneyInvitation(EmailCommunicationDTO communication, Participant particpant)
        {
            var template = _communicationService.GetTemplate(communication.TemplateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            var replyTo = _contactService.GetContactById(particpant.ContactId);
            var mergeData = SetupMergeData(particpant.PreferredName, communication.groupId.Value);

            return new MpCommunication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new MpContact { ContactId = _defaultContactEmailId, EmailAddress = fromContact.Email_Address },
                ReplyToContact = new MpContact {ContactId = _defaultContactEmailId, EmailAddress = replyTo.Email_Address },
                ToContacts = new List<MpContact> { new MpContact { ContactId = fromContact.Contact_ID, EmailAddress = communication.emailAddress } },
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

        public List<GroupParticipantDTO> GetGroupParticipants(int groupId, bool active = true)
        {
            var groupParticipants = _mpGroupService.GetGroupParticipants(groupId, active);       
            if (groupParticipants == null)
            {
                return null;
            }
            var participants = groupParticipants.Select(Mapper.Map<MpGroupParticipant, GroupParticipantDTO>).ToList();

            var configuration = MpObjectAttributeConfigurationFactory.GroupParticipant();

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

        public List<GroupDTO> GetSmallGroupsForAuthenticatedUser(string token)
        {
            var smallGroups = _mpGroupService.GetSmallGroupsForAuthenticatedUser(token);
            if (smallGroups == null)
            {
                return null;
            }

            var groupDetail = smallGroups.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();
            //var configuration = MpObjectAttributeConfigurationFactory.Group();
            //var mpAttributes = _attributeRepository.GetAttributes(null);

            //foreach (var group in groupDetail)
            //{
                //var attributesTypes = _objectAttributeService.GetObjectAttributes(token, group.GroupId, configuration, mpAttributes);
                //group.AttributeTypes = attributesTypes.MultiSelect;
                //group.SingleAttributes = attributesTypes.SingleSelect;
            //}

            return groupDetail;
        }

        public GroupDTO UpdateGroup(GroupDTO group)
        {
            try
            {
                var mpGroup = Mapper.Map<MpGroup>(group);
                _mpGroupService.UpdateGroup(mpGroup);

                List<MpGroupParticipant> groupParticipants = _mpGroupService.GetGroupParticipants(group.GroupId, true);

                if (groupParticipants.Count(participant => participant.StartDate < group.StartDate) > 0)
                {
                    UpdateGroupParticipantStartDate(groupParticipants.Where(part => part.StartDate < group.StartDate).ToList(), group.StartDate);
                }

                if (group.AttributeTypes.ContainsKey(_groupCategoryAttributeTypeId) && group.AttributeTypes[90].Attributes.Any(a => a.AttributeId == 0))
                {
                    var categoryAttributes = Mapper.Map<List<MpAttribute>>(group.AttributeTypes[90].Attributes);

                    categoryAttributes = _attributeService.CreateMissingAttributes(categoryAttributes, _groupCategoryAttributeTypeId);
                    group.AttributeTypes[_groupCategoryAttributeTypeId].Attributes = Mapper.Map<List<ObjectAttributeDTO>>(categoryAttributes);
                }

                var configuration = MpObjectAttributeConfigurationFactory.Group();
                _objectAttributeService.SaveObjectAttributes(group.GroupId, group.AttributeTypes, group.SingleAttributes, configuration);

                if (group.MinorAgeGroupsAdded)
                {
                    var leaders =groupParticipants.Where(p => p.GroupRoleId == _groupRoleLeader).ToList();
                    _mpGroupService.SendNewStudentMinistryGroupAlertEmail(leaders);
                }
            }
            catch (Exception e)
            {
                var message = String.Format("Could not update group {0}", group.GroupName);
                _logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            return group;
        }

        public void UpdateGroupParticipantRole(string token, GroupParticipantDTO participant)
        {
            try
            {
                var mpParticipant = Mapper.Map<MpGroupParticipant>(participant);
                List<MpGroupParticipant> part = new List<MpGroupParticipant>();
                part.Add(mpParticipant);
                _mpGroupService.UpdateGroupParticipant(part);
                if (participant.GroupRoleId == _groupRoleLeader)
                {
                    if (_mpGroupService.ParticipantGroupHasStudents(token, mpParticipant.ParticipantId, mpParticipant.GroupParticipantId))
                    {
                        _mpGroupService.SendNewStudentMinistryGroupAlertEmail(part);
                    }
                }
            }
            catch (Exception e)
            {
                var message = String.Format("Could not update group participant {0}", participant.ParticipantId);
                _logger.Error(message, e);
            }
        }

        private void UpdateGroupParticipantStartDate(List<MpGroupParticipant> participants, DateTime groupStartDate)
        {
            foreach (var part in participants)
            {
                part.StartDate = groupStartDate;
            }

            _mpGroupService.UpdateGroupParticipant(participants);
        }
    }
}
