using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using AutoMapper;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using System.Text.RegularExpressions;
using crds_angular.Models.Finder;
using Crossroads.Utilities.Extensions;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models.Finder;

namespace crds_angular.Services
{
    public class GroupToolService : MinistryPlatformBaseService, IGroupToolService
    {
        private readonly IGroupToolRepository _groupToolRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupService _groupService;
        private readonly IParticipantRepository _participantRepository;
        private readonly ICommunicationRepository _communicationRepository;
        private readonly IContentBlockService _contentBlockService;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAddressProximityService _addressProximityService;
        private readonly IContactRepository _contactRepository;
        private readonly IAddressProximityService _addressMatrixService;
        private readonly IEmailCommunication _emailCommunicationService;
        private readonly IAttributeService _attributeService;
        private readonly IAddressService _addressService;
        private readonly IFinderRepository _finderRepository;

        private readonly int _defaultGroupContactEmailId;
        private readonly int _defaultAuthorUserId;
        private readonly int _defaultGroupRoleId;
        private readonly int _groupRoleLeaderId;
        private readonly int _removeParticipantFromGroupEmailTemplateId;
        private readonly int _gatheringHostAcceptTemplate;
        private readonly int _gatheringHostDenyTemplate;
        private readonly int _domainId;
        private readonly int _groupEndedParticipantEmailTemplate;
        private readonly string _baseUrl;
        private readonly int _addressMatrixSearchDepth;
        private readonly int _groupRequestToJoinEmailTemplate;
        private readonly int _anywhereGroupRequestToJoinEmailTemplate;
        private readonly int _groupRequestPendingReminderEmailTemplateId;
        private readonly int _attributeTypeGroupCategory;
        private readonly int _smallGroupTypeId;
        private readonly int _onsiteGroupTypeId;
        private readonly int _anywhereGroupType;
        private readonly int _connectGatheringStatusAccept;
        private readonly int _connectGatheringStatusDeny;
        private readonly int _connectGatheringRequestToJoin;

        private const string GroupToolRemoveParticipantEmailTemplateTextTitle = "groupToolRemoveParticipantEmailTemplateText";
        private const string GroupToolRemoveParticipantSubjectTemplateText = "groupToolRemoveParticipantSubjectTemplateText";
        private const string GroupToolApproveInquiryEmailTemplateText = "groupToolApproveInquiryEmailTemplateText";
        private const string GroupToolApproveInquirySubjectTemplateText = "groupToolApproveInquirySubjectTemplateText";
        private const string GroupToolDenyInquiryEmailTemplateText = "groupToolDenyInquiryEmailTemplateText";
        private const string GroupToolDenyInquirySubjectTemplateText = "groupToolDenyInquirySubjectTemplateText";

        private readonly ILog _logger = LogManager.GetLogger(typeof(GroupToolService));

        public GroupToolService(
            IGroupToolRepository groupToolRepository,
            IGroupRepository groupRepository,
            IGroupService groupService,
            IParticipantRepository participantRepository,
            ICommunicationRepository communicationRepository,
            IContentBlockService contentBlockService,
            IConfigurationWrapper configurationWrapper,
            IInvitationRepository invitationRepository,
            IAddressProximityService addressProximityService,
            IContactRepository contactRepository,
            IAddressProximityService addressMatrixService,
            IEmailCommunication emailCommunicationService,
            IAttributeService attributeService,
            IAddressService addressService,
            IFinderRepository finderRepository
            )
        {
            _groupToolRepository = groupToolRepository;
            _groupRepository = groupRepository;
            _groupService = groupService;
            _participantRepository = participantRepository;
            _communicationRepository = communicationRepository;
            _contentBlockService = contentBlockService;
            _invitationRepository = invitationRepository;
            _addressProximityService = addressProximityService;
            _contactRepository = contactRepository;
            _addressMatrixService = addressMatrixService;
            _emailCommunicationService = emailCommunicationService;
            _attributeService = attributeService;
            _addressService = addressService;
            _finderRepository = finderRepository;

            _defaultGroupContactEmailId = configurationWrapper.GetConfigIntValue("DefaultGroupContactEmailId");
            _defaultAuthorUserId = configurationWrapper.GetConfigIntValue("DefaultAuthorUser");
            _groupRoleLeaderId = configurationWrapper.GetConfigIntValue("GroupRoleLeader");
            _defaultGroupRoleId = configurationWrapper.GetConfigIntValue("Group_Role_Default_ID");
            _groupRequestPendingReminderEmailTemplateId = configurationWrapper.GetConfigIntValue("GroupRequestPendingReminderEmailTemplateId");
            _attributeTypeGroupCategory = configurationWrapper.GetConfigIntValue("GroupCategoryAttributeTypeId");
            
            _removeParticipantFromGroupEmailTemplateId = configurationWrapper.GetConfigIntValue("RemoveParticipantFromGroupEmailTemplateId");

            _domainId = configurationWrapper.GetConfigIntValue("DomainId");
            _groupEndedParticipantEmailTemplate = configurationWrapper.GetConfigIntValue("GroupEndedParticipantEmailTemplate");
            _gatheringHostAcceptTemplate = configurationWrapper.GetConfigIntValue("GatheringHostAcceptTemplate");
            _gatheringHostDenyTemplate = configurationWrapper.GetConfigIntValue("GatheringHostDenyTemplate");
            _groupRequestToJoinEmailTemplate = configurationWrapper.GetConfigIntValue("GroupRequestToJoinEmailTemplate");
            _anywhereGroupRequestToJoinEmailTemplate = configurationWrapper.GetConfigIntValue("AnywhereGroupRequestToJoinEmailTemplate");
            _baseUrl = configurationWrapper.GetConfigValue("BaseURL");
            _addressMatrixSearchDepth = configurationWrapper.GetConfigIntValue("AddressMatrixSearchDepth");
            
            _smallGroupTypeId = configurationWrapper.GetConfigIntValue("SmallGroupTypeId");
            _onsiteGroupTypeId = configurationWrapper.GetConfigIntValue("OnsiteGroupTypeId");
            _anywhereGroupType = configurationWrapper.GetConfigIntValue("AnywhereGroupTypeId");
            _connectGatheringStatusAccept = configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusAccepted");
            _connectGatheringStatusDeny = configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusDeclined");
            _connectGatheringRequestToJoin = configurationWrapper.GetConfigIntValue("ConnectCommunicationTypeRequestToJoin");
        }

        public List<Invitation> GetInvitations(int sourceId, int invitationTypeId, string token)
        {
            var invitations = new List<Invitation>();
            try
            {
                VerifyCurrentUserIsGroupLeader(token, sourceId);

                var mpInvitations = _groupToolRepository.GetInvitations(sourceId, invitationTypeId);
                mpInvitations.ForEach(x => invitations.Add(Mapper.Map<Invitation>(x)));
            }
            catch (Exception e)
            {
                var message = $"Exception retrieving invitations for SourceID = {sourceId}, InvitationTypeID = {invitationTypeId}.";
                _logger.Error(message, e);
                throw;
            }
            return invitations;
        }

        public List<Inquiry> GetInquiries(int groupId, string token)
        {
            var requests = new List<Inquiry>();
            try
            {
                VerifyCurrentUserIsGroupLeader(token, groupId);

                var mpRequests = _groupToolRepository.GetInquiries(groupId);
                mpRequests.ForEach(x => requests.Add(Mapper.Map<Inquiry>(x)));
            }
            catch (Exception e)
            {
                var message = $"Exception retrieving inquiries for group id = {groupId}.";
                _logger.Error(message, e);
                throw;
            }
            return requests;
        }

        public void RemoveParticipantFromMyGroup(string token, int groupId, int groupParticipantId, string message = null)
        {
            try
            {
                var myGroup = GetMyGroupInfo(token, groupId);

                _groupService.endDateGroupParticipant(groupId, groupParticipantId);

                try
                {
                    SendGroupParticipantEmail(groupId,
                                              groupParticipantId,
                                              myGroup.Group,
                                              _removeParticipantFromGroupEmailTemplateId,
                                              null,
                                              GroupToolRemoveParticipantSubjectTemplateText,
                                              GroupToolRemoveParticipantEmailTemplateTextTitle,
                                              message,
                                              myGroup.Me);
                }
                catch (Exception e)
                {
                    _logger.Warn($"Could not send email to group participant {groupParticipantId} notifying of removal from group {groupId}", e);
                }
            }
            catch (GroupParticipantRemovalException e)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
            catch (Exception e)
            {
                throw new GroupParticipantRemovalException($"Could not remove group participant {groupParticipantId} from group {groupId}", e);
            }
        }

        public void SendGroupParticipantEmail(int groupId,
                                              int? toGroupParticipantId,
                                              GroupDTO group,
                                              int emailTemplateId,
                                              MpParticipant toParticipant = null,
                                              string subjectTemplateContentBlockTitle = null,
                                              string emailTemplateContentBlockTitle = null,
                                              string message = null,
                                              MpParticipant fromParticipant = null)
        {
            var participant = toParticipant == null
                ? group.Participants.Find(p => p.GroupParticipantId == toGroupParticipantId)
                : new GroupParticipantDTO
                {
                    ContactId = toParticipant.ContactId,
                    Email = toParticipant.EmailAddress,
                    NickName = toParticipant.PreferredName,
                    ParticipantId = toParticipant.ParticipantId
                };

            var emailTemplate = _communicationRepository.GetTemplate(emailTemplateId);
            var fromContact = new MpContact
            {
                ContactId = emailTemplate.FromContactId,
                EmailAddress = emailTemplate.FromEmailAddress
            };
            var replyTo = new MpContact
            {
                ContactId = fromParticipant?.ContactId ?? emailTemplate.ReplyToContactId,
                EmailAddress = fromParticipant == null ? emailTemplate.ReplyToEmailAddress : fromParticipant.EmailAddress
            };

            var to = new List<MpContact>
            {
                new MpContact
                {
                    ContactId = participant.ContactId,
                    EmailAddress = participant.Email
                }
            };

            var subjectTemplateText = string.IsNullOrWhiteSpace(subjectTemplateContentBlockTitle)
                ? string.Empty
                : _contentBlockService[subjectTemplateContentBlockTitle].Content ?? string.Empty;
           

            var emailTemplateText = string.IsNullOrWhiteSpace(emailTemplateContentBlockTitle) ? string.Empty : _contentBlockService[emailTemplateContentBlockTitle].Content;
            var mergeData = getDictionary(participant);
            mergeData["Email_Custom_Message"] = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
            mergeData["Group_Name"] = group.GroupName;
            mergeData["Group_Description"] = group.GroupDescription;
            if (fromParticipant != null)
            {
                mergeData["From_Display_Name"] = fromParticipant.DisplayName;
                mergeData["From_Preferred_Name"] = fromParticipant.PreferredName;
            }

            // Since the templates are coming from content blocks, they may have replacement tokens in them as well.
            // These will not get replaced with merge data in _communicationRepository.SendMessage(), (it doesn't doubly
            // replace) so we'll parse them here before adding them to the merge data. 
            mergeData["Subject_Template_Text"] = _communicationRepository.ParseTemplateBody(Regex.Replace(subjectTemplateText, "<.*?>", string.Empty), mergeData);
            mergeData["Email_Template_Text"] = _communicationRepository.ParseTemplateBody(emailTemplateText, mergeData);

            var email = new MpCommunication
            {
                EmailBody = emailTemplate.Body,
                EmailSubject = emailTemplate.Subject,
                AuthorUserId = 5,
                DomainId = _domainId,
                FromContact = fromContact,
                ReplyToContact = replyTo,
                TemplateId = emailTemplateId,
                ToContacts = to,
                MergeData = mergeData
            };
            _communicationRepository.SendMessage(email);
        }

        public MyGroup VerifyCurrentUserIsGroupLeader(string token, int groupId)
        {
            var groupParticipant = _groupRepository.GetAuthenticatedUserParticipationByGroupID(token, groupId);

            if (groupParticipant == null)
                throw new GroupNotFoundForParticipantException($"Could not find group {groupId} for user");

            if (groupParticipant.GroupRoleId != _groupRoleLeaderId)
                throw new NotGroupLeaderException($"User is not a leader of group {groupId}");

            return new MyGroup
            {
                Group = new GroupDTO
                {
                    GroupId = groupId
                },
                Me = new MpParticipant
                {
                    ParticipantId = groupParticipant.ParticipantId
                }
            };
        }

        public MyGroup GetMyGroupInfo(string token, int groupId)
        {
            var groups = _groupService.GetGroupByIdForAuthenticatedUser(token, groupId);
            var group = groups == null || !groups.Any() ? null : groups.FirstOrDefault();

            if (group == null)
            {
                throw new GroupNotFoundForParticipantException($"Could not find group {groupId} for user");
            }

            var groupParticipants = group.Participants;
            var me = _participantRepository.GetParticipantRecord(token);

            if (groupParticipants?.Find(p => p.ParticipantId == me.ParticipantId) == null ||
                groupParticipants.Find(p => p.ParticipantId == me.ParticipantId).GroupRoleId != _groupRoleLeaderId)
            {
                throw new NotGroupLeaderException($"User is not a leader of group {groupId}");
            }

            return new MyGroup
            {
                Group = group,
                Me = me
            };
        }

        public void ApproveDenyInquiryFromMyGroup(string token, int groupTypeId, int groupId, bool approve, Inquiry inquiry, string message = null)
        {
            try
            {
                var myGroup = GetMyGroupInfo(token, groupId);

                if (approve)
                {
                    ApproveInquiry(groupId, myGroup.Group, inquiry, myGroup.Me, message);
                }
                else
                {
                    DenyInquiry(groupId, myGroup.Group, inquiry, myGroup.Me, message);
                }
            }
            catch (GroupParticipantRemovalException e)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
            catch (Exception e)
            {
                throw new GroupParticipantRemovalException($"Could not add Inquirier {inquiry.InquiryId} from group {groupId}", e);
            }
        }

        private void RecordConnectInteraction(int groupId, int fromContactId, int toContactId, int connectionType, int connectionStatus)
        {
            //only record anywhere group type interactions
            var group = _groupService.GetGroupDetails(groupId);
            if (group.GroupTypeId != _anywhereGroupType)
            {
                return;
            }

            var connection = new MpConnectCommunication
            {
                GroupId = groupId,
                FromContactId = fromContactId,
                ToContactId = toContactId,
                CommunicationTypeId = connectionType,
                CommunicationStatusId = connectionStatus
            };
            _finderRepository.RecordConnection(connection);
        }

        private void ApproveInquiry(int groupId, GroupDTO group, Inquiry inquiry, MpParticipant me, string message)
        {
            _groupService.addContactToGroup(groupId, inquiry.ContactId);
            _groupRepository.UpdateGroupInquiry(groupId, inquiry.InquiryId, true);
            RecordConnectInteraction(groupId, me.ContactId, inquiry.ContactId, _connectGatheringRequestToJoin, _connectGatheringStatusAccept);
            // For now pick template based on group type
            var emailTemplateId = (group.GroupTypeId == _anywhereGroupType) 
                                                      ? _gatheringHostAcceptTemplate 
                                                      : _removeParticipantFromGroupEmailTemplateId;

            try
            {
                SendApproveDenyInquiryEmail(
                    true,
                    groupId,
                    group,
                    inquiry,
                    me,
                    emailTemplateId,
                    GroupToolApproveInquiryEmailTemplateText,
                    message);
            }
            catch (Exception e)
            {
                _logger.Warn($"Could not send email to Inquirier {inquiry.InquiryId} notifying of being approved to group {groupId}", e);
            }
        }

        private void DenyInquiry(int groupId, GroupDTO group, Inquiry inquiry, MpParticipant me, string message)
        {
            _groupRepository.UpdateGroupInquiry(groupId, inquiry.InquiryId, false);
            RecordConnectInteraction(groupId, me.ContactId, inquiry.ContactId, _connectGatheringRequestToJoin, _connectGatheringStatusDeny);
            // For now pick template based on group type
            var emailTemplateId = (group.GroupTypeId == _anywhereGroupType)
                                                      ? _gatheringHostDenyTemplate
                                                      : _removeParticipantFromGroupEmailTemplateId;

            try
            {
                SendApproveDenyInquiryEmail(
                    false,
                    groupId,
                    group,
                    inquiry,
                    me,
                    emailTemplateId,
                    GroupToolDenyInquiryEmailTemplateText,
                    message);
            }
            catch (Exception e)
            {
                _logger.Warn($"Could not send email to Inquirier {inquiry.InquiryId} notifying of being denied from group {groupId}", e);
            }
        }

        private void SendApproveDenyInquiryEmail(bool approve,
                                                 int groupId,
                                                 GroupDTO group,
                                                 Inquiry inquiry,
                                                 MpParticipant me,
                                                 int emailTemplateId,
                                                 string emailTemplateContentBlockTitle,
                                                 string message)
        {
            try
            {
                string subject = null;

                if (group.GroupTypeId != _anywhereGroupType)
                {
                    subject = approve ? GroupToolApproveInquirySubjectTemplateText : GroupToolDenyInquirySubjectTemplateText;
                }
              
                var participant = _participantRepository.GetParticipant(inquiry.ContactId);

                SendGroupParticipantEmail(groupId,
                                          null,
                                          group,
                                          emailTemplateId,
                                          participant,
                                          subject,
                                          emailTemplateContentBlockTitle,
                                          message,
                                          me);
            }
            catch (Exception e)
            {
                _logger.Warn($"Could not send email to Inquirer {inquiry.InquiryId} notifying for group {groupId}", e);
            }
        }

        public void AcceptDenyGroupInvitation(string token, int groupId, string invitationGuid, bool accept)
        {
            try
            {
                //If they accept the invite get their participant record and them to the group as a member.
                if (accept)
                {
                    var participant = _participantRepository.GetParticipantRecord(token);

                    // make sure the person isn't already in a group
                    var groupParticipants = _groupRepository.GetGroupParticipants(groupId, true);

                    if (groupParticipants.Any(p => p.ParticipantId == participant.ParticipantId))
                    {
                        // mark as used here, too, because of exception flow
                        _invitationRepository.MarkInvitationAsUsed(invitationGuid);
                        throw new DuplicateGroupParticipantException("Cannot accept invite - already member of group");
                    }

                    _groupRepository.addParticipantToGroup(participant.ParticipantId, groupId, _defaultGroupRoleId, false, DateTime.Now);
                }

                _invitationRepository.MarkInvitationAsUsed(invitationGuid);
            }
            catch (DuplicateGroupParticipantException e)
            {
                throw e;
            }
            catch (GroupParticipantRemovalException e)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
            catch (Exception e)
            {
                throw new GroupParticipantRemovalException($"Could not accept = {accept} from group {groupId}", e);
            }
        }

        public void SendAllGroupLeadersEmail(string token, int groupId, GroupMessageDTO message)
        {
            var requestor = _participantRepository.GetParticipantRecord(token);
            var requestorContact = _contactRepository.GetContactById(requestor.ContactId);
            var group = _groupService.GetGroupDetails(groupId);

            var fromContact = new MpContact
            {
                ContactId = _defaultGroupContactEmailId,
                EmailAddress = "groups@crossroads.net"
            };

            var replyToContact = new MpContact
            {
                ContactId = requestor.ContactId,
                EmailAddress = requestor.EmailAddress
            };

            var leaders = @group.Participants.
                Where(groupParticipant => groupParticipant.GroupRoleId == _groupRoleLeaderId).
                Select(groupParticipant => new MpContact
                       {
                           ContactId = groupParticipant.ContactId,
                           EmailAddress = groupParticipant.Email,
                           LastName = groupParticipant.LastName,
                           Nickname = groupParticipant.NickName
                       }).ToList();

            var fromString = "<p><i>This email was sent from: " + requestorContact.Nickname + " " + requestorContact.Last_Name + " (" + requestor.EmailAddress + ")</i></p>";

            var toString = "<p><i>This email was sent to: ";

            foreach (var item in leaders)
            {
                toString += item.Nickname + " " + item.LastName + " (" + item.EmailAddress + "), ";
            }

            char[] trailingChars = { ',', ' ' };
            toString = toString.TrimEnd(trailingChars);
            toString += "</i></p>";

            var email = new MpCommunication
            {
                EmailBody = fromString + "<p>" + message.Body + "</p>" + toString,
                EmailSubject = $"Crossroads Group {@group.GroupName}: {message.Subject}",
                AuthorUserId = _defaultAuthorUserId,
                DomainId = _domainId,
                FromContact = fromContact,
                ReplyToContact = replyToContact,
                ToContacts = leaders
            };

            _communicationRepository.SendMessage(email);
        }

        public void SendAllGroupParticipantsEmail(string token, int groupId, int groupTypeId, string subject, string body)
        {
            var leaderRecord = _participantRepository.GetParticipantRecord(token);
            var groups = _groupService.GetGroupByIdForAuthenticatedUser(token, groupId);

            if (groups == null || !groups.Any())
            {
                throw new GroupNotFoundForParticipantException($"Could not find group {groupId} for groupParticipant {leaderRecord.ParticipantId}");
            }

            if (!ValidateUserAsLeader(token, groupTypeId, groupId, leaderRecord.ParticipantId, groups.First()))
            {
                throw new NotGroupLeaderException($"Group participant ID {leaderRecord.ParticipantId} is not a leader of group {groupId}");
            }

            var fromContact = new MpContact
            {
                ContactId = 1519180,
                EmailAddress = "updates@crossroads.net"
            };

            var replyToContact = new MpContact
            {
                ContactId = leaderRecord.ContactId,
                EmailAddress = leaderRecord.EmailAddress
            };

            var toContacts = groups.First().Participants.Select(groupParticipant => new MpContact
                                                                            {
                                                                                ContactId = groupParticipant.ContactId,
                                                                                EmailAddress = groupParticipant.Email
                                                                            }).DistinctBy(c => c.EmailAddress).ToList();

            var email = new MpCommunication
            {
                EmailBody = body,
                EmailSubject = subject,
                AuthorUserId = 5,
                DomainId = _domainId,
                FromContact = fromContact,
                ReplyToContact = replyToContact,
                ToContacts = toContacts
            };

            _communicationRepository.SendMessage(email);
        }

        public bool ValidateUserAsLeader(string token, int groupTypeId, int groupId, int groupParticipantId, GroupDTO group)
        {
            var groupParticipants = group.Participants;
            var me = _participantRepository.GetParticipantRecord(token);

            if (groupParticipants == null || groupParticipants.Find(p => p.ParticipantId == me.ParticipantId) == null)
            {
                throw new NotGroupLeaderException($"Group participant {groupParticipantId} is not a leader of group {groupId}");

            }
            var isLeader = false;
            foreach (var part in groupParticipants)
            {
                if ( (me.ParticipantId == part.ParticipantId) &&
                        (_groupRoleLeaderId == part.GroupRoleId) )
                {
                    isLeader = true;
                    break;

                }
            }
            return isLeader;
        }

        public void EndGroup(int groupId, int reasonEndedId)
        {
            //get all participants before we end the group so they are not endDated and still
            //available from this call.
            // ReSharper disable once RedundantArgumentDefaultValue
            var participants = _groupService.GetGroupParticipants(groupId, true);
            _groupService.EndDateGroup(groupId, reasonEndedId);
            foreach (var participant in participants)
            {
                var mergeData = new Dictionary<string, object>
                {
                    {"Participant_Name", participant.NickName},
                    {"Group_Tool_Url", @"https://" + _baseUrl + "/groups/search"}
                };
                SendSingleGroupParticipantEmail(participant, _groupEndedParticipantEmailTemplate, mergeData);
            }
        }

        /// <summary>
        /// Sends a participant and email based off of a template ID and any merge data you need for that template. 
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="templateId"></param>
        /// <param name="mergeData"></param>
        public int SendSingleGroupParticipantEmail(GroupParticipantDTO participant, int templateId, Dictionary<string, object> mergeData)
        {
            var emailTemplate = _communicationRepository.GetTemplate(templateId);

            var domainId = Convert.ToInt32(_domainId);
            var from = new MpContact
            {
                ContactId = emailTemplate.FromContactId,
                EmailAddress = emailTemplate.FromEmailAddress
            };

            var to = new List<MpContact>
            {
                new MpContact
                {
                    ContactId = participant.ContactId,
                    EmailAddress = participant.Email
                }
            };

            var message = new MpCommunication
            {
                EmailBody = emailTemplate.Body,
                EmailSubject = emailTemplate.Subject,
                AuthorUserId = 5,
                DomainId = domainId,
                FromContact = from,
                MergeData = mergeData,
                ReplyToContact = from,
                TemplateId = templateId,
                ToContacts = to,
                StartDate = DateTime.Now
            };
            // ReSharper disable once RedundantArgumentDefaultValue
            return _communicationRepository.SendMessage(message, false);
        }


        public List<GroupDTO> SearchGroups(int[] groupTypeIds, 
                                           string keywords = null, 
                                           string location = null, 
                                           int? groupId = null,
                                           GeoCoordinate originCoords = null)
        {
            // Split single search term into multiple words, broken on whitespace
            // TODO Should remove stopwords from search - possibly use a configurable list of words (http://www.link-assistant.com/seo-stop-words.html)
            var search = string.IsNullOrWhiteSpace(keywords)
                ? null
                : keywords
                    .Replace("'", "''") // Replace single quote with two, since the MP Rest API doesn't do it
                    .Replace("&", "%26") // Replace & with the hex representation, to avoid looking like a stored proc parameter
                    .Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);

            var results = _groupToolRepository.SearchGroups(groupTypeIds, search, groupId);

            if (results == null || !results.Any())
            {
                return null;
            }

            var groups = results.Select(Mapper.Map<GroupDTO>).ToList();

            if (string.IsNullOrWhiteSpace(location))
            {
                return groups;
            }

            try
            {
                // first call is for all results
                var proximities = _addressProximityService.GetProximity(location, groups.Select(g => g.Address).ToList(), originCoords);
                for (var i = 0; i < groups.Count; i++)
                {
                    groups[i].Proximity = proximities[i];
                }

                // order by closest n raw results, then get driving directions
                groups = groups.OrderBy(r => r.Proximity ?? decimal.MaxValue).ToList();

                var closestGroups = groups.Take(_addressMatrixSearchDepth).ToList();
                var drivingProximities = _addressMatrixService.GetProximity(location, closestGroups.Select(g => g.Address).ToList());

                for (var i = 0; i < closestGroups.Count; i++)
                {
                    groups[i].Proximity = drivingProximities[i];
                }

                // order again in case proximties changed because of driving directions
                groups = groups.OrderBy(r => r.Proximity ?? decimal.MaxValue).ToList();
            }
            catch (InvalidAddressException e)
            {
                _logger.Info($"Can't validate origin address {location}", e);
            }
            catch (Exception e)
            {
                _logger.Error($"Can't search by proximity for address {location}", e);
            }

            return groups;
        }

        public List<GroupDTO> GetGroupToolGroups(string token)
        {
            var groups = _groupService.GetGroupsByTypeOrId(token,null, new int[] { _smallGroupTypeId, _onsiteGroupTypeId }, null);

            return _groupService.RemoveOnsiteParticipantsIfNotLeader(groups, token);
        }

        public void SubmitInquiry(string token, int groupId)
        {
            var participant = _participantRepository.GetParticipantRecord(token);
            var contact = _contactRepository.GetContactById(participant.ContactId);

            // check to see if the inquiry is going against a group where a person is already a member or has an outstanding request to join
            var requestsForContact = _groupToolRepository.GetInquiries(groupId).Where(r => r.ContactId == participant.ContactId && r.Placed == null);
            var participants = _groupRepository.GetGroupParticipants(groupId, true).Where(r => r.ContactId == participant.ContactId);

            if (participants.Any())
            {
                throw new ExistingRequestException("User already a member");

            }
            if (requestsForContact.Any())
            {
                throw new ExistingRequestException("User already has request");
            }

            var mpInquiry = new MpInquiry
            {
                ContactId = participant.ContactId,
                GroupId = groupId,
                EmailAddress = participant.EmailAddress,
                PhoneNumber = contact.Home_Phone,
                FirstName = contact.Nickname,
                LastName = contact.Last_Name,
                RequestDate = DateTime.Now
            };

            _groupRepository.CreateGroupInquiry(mpInquiry);

            var group = _groupService.GetGroupDetails(groupId);
            var leaders = group.Participants.
                Where(groupParticipant => groupParticipant.GroupRoleId == _groupRoleLeaderId).ToList();

            foreach (var leader in leaders)
            {
                if (group.GroupTypeId == _anywhereGroupType)
                {
                    var Requestor = "<i>" + contact.Nickname + " " + contact.Last_Name.Substring(0, 1) + "." + "</i> ";
                    var RequestorSub = contact.Nickname + " " + contact.Last_Name.Substring(0,1) + ".";
                    var mergeData = new Dictionary<string, object> { { "Name", leader.NickName }, { "Requestor", Requestor }, { "RequestorSub", RequestorSub } };
                    SendSingleGroupParticipantEmail(leader, _anywhereGroupRequestToJoinEmailTemplate, mergeData);
                }
                else
                {
                    var Requestor = "<i>" + contact.Nickname + " " + contact.Last_Name + "</i> ";
                    var mergeData = new Dictionary<string, object> { { "Name", leader.NickName }, { "Requestor", Requestor } };
                    SendSingleGroupParticipantEmail(leader, _groupRequestToJoinEmailTemplate, mergeData);
                }
            }
        }

        public void SendSmallGroupPendingInquiryReminderEmails()
        {
            try
            {
                var mpRequests = _groupToolRepository.GetInquiries();
                var groupsWithPendingRequests = mpRequests.Select(r => r.GroupId).Distinct().ToList();

                foreach (var groupId in groupsWithPendingRequests)
                {
                    var group = _groupRepository.getGroupDetails(groupId);
                    var requests = mpRequests.FindAll(r => r.GroupId == groupId);
                    SendPendingInquiryReminderEmail(group, requests);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Exception retrieving pending inquiries", e);
                throw;
            }
        }

        public List<AttributeCategoryDTO> GetGroupCategories()
        {
            List<AttributeCategoryDTO> cats = _attributeService.GetAttributeCategory(_attributeTypeGroupCategory);
            
            foreach (AttributeCategoryDTO cat in cats)
            {
                if (cat.RequiresActiveAttribute)
                {
                    cat.Attribute = _attributeService.GetOneAttributeByCategoryId(cat.CategoryId);
                }
            }

            //do not return any categories where it requires an active attribute but there are no active attributes
            cats.RemoveAll(cat => cat.RequiresActiveAttribute && cat.Attribute == null );

            return cats;
        }

        private void SendPendingInquiryReminderEmail(MpGroup group, IReadOnlyCollection<MpInquiry> inquiries)
        {
            var leaders = ((List<MpGroupParticipant>) group.Participants).FindAll(p => p.GroupRoleId == _groupRoleLeaderId).ToList();
            var allLeaders = string.Join(", ", leaders.Select(leader => $"{leader.NickName} {leader.LastName} ({leader.Email})").ToArray());
            var pendingRequests = string.Join("<br>", inquiries.Select(inquiry => $"{inquiry.FirstName} {inquiry.LastName} ({inquiry.EmailAddress})").ToArray());

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var leader in leaders)
            {
                var mergeData = new Dictionary<string, object>
                {
                    {"Nick_Name", leader.NickName},
                    {"Last_Name", leader.LastName},
                    {"All_Leaders", allLeaders},
                    {"Pending_Requests", pendingRequests},
                    {"Group_Name", group.Name},
                    {"Group_Description", group.GroupDescription},
                    {"Group_ID", group.GroupId},
                    {"Pending_Requests_Count", inquiries.Count}
                };

                var email = new EmailCommunicationDTO
                {
                    groupId = group.GroupId,
                    TemplateId = _groupRequestPendingReminderEmailTemplateId,
                    ToContactId = leader.ContactId,
                    MergeData = mergeData
                };
                _emailCommunicationService.SendEmail(email);
            }
        }
    }
}