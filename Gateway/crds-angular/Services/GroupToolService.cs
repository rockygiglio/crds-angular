using System;
using System.Collections.Generic;
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

        private readonly int _groupRoleLeaderId;
        private readonly int _removeParticipantFromGroupEmailTemplateId;
        private readonly int _domainId;

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
                           IConfigurationWrapper configurationWrapper)
        {

            _groupToolRepository = groupToolRepository;
            _groupRepository = groupRepository;
            _groupService = groupService;
            _participantRepository = participantRepository;
            _communicationRepository = communicationRepository;
            _contentBlockService = contentBlockService;

            _groupRoleLeaderId = configurationWrapper.GetConfigIntValue("GroupRoleLeader");
            _removeParticipantFromGroupEmailTemplateId = configurationWrapper.GetConfigIntValue("RemoveParticipantFromGroupEmailTemplateId");

            _domainId = configurationWrapper.GetConfigIntValue("DomainId");
        }

        public List<Invitation> GetInvitations(int sourceId, int invitationTypeId, string token)
        {
            var invitations = new List<Invitation>();
            try
            {
                var mpInvitations = _groupToolRepository.GetInvitations(sourceId, invitationTypeId, token);
                mpInvitations.ForEach(x => invitations.Add(Mapper.Map<Invitation>(x)));
            }
            catch (Exception e)
            {
                var message = string.Format("Exception retrieving invitations for SourceID = {0}, InvitationTypeID = {1}.", sourceId, invitationTypeId);
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
                var mpRequests = _groupToolRepository.GetInquiries(groupId, token);
                mpRequests.ForEach(x => requests.Add(Mapper.Map<Inquiry>(x)));
            }
            catch (Exception e)
            {
                var message = string.Format("Exception retrieving inquiries for group id = {0}.", groupId);
                _logger.Error(message, e);
                throw;
            }
            return requests;
        }

        public void RemoveParticipantFromMyGroup(string token, int groupTypeId, int groupId, int groupParticipantId, string message = null)
        {
            try
            {
                var myGroup = VerifyCurrentUserIsGroupLeader(token, groupTypeId, groupId);

                _groupService.endDateGroupParticipant(groupId, groupParticipantId);

                try
                {
                    SendGroupParticipantEmail(groupId,
                                              groupParticipantId,
                                              true,
                                              myGroup.Group,
                                              _removeParticipantFromGroupEmailTemplateId,
                                              GroupToolRemoveParticipantSubjectTemplateText,
                                              GroupToolRemoveParticipantEmailTemplateTextTitle,
                                              message,
                                              myGroup.Me);
                }
                catch (Exception e)
                {
                    _logger.Warn(string.Format("Could not send email to group participant {0} notifying of removal from group {1}", groupParticipantId, groupId), e);
                }
            }
            catch (GroupParticipantRemovalException e)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
            catch (Exception e)
            {
                throw new GroupParticipantRemovalException(string.Format("Could not remove group participant {0} from group {1}", groupParticipantId, groupId), e);
            }

        }

        public void SendGroupParticipantEmail(int groupId, int participantId, bool groupParticpantId, GroupDTO group, int emailTemplateId, string subjectTemplateContentBlockTitle = null, string emailTemplateContentBlockTitle = null, string message = null, Participant fromParticipant = null)
        {
            var participant = groupParticpantId ? group.Participants.Find(p => p.GroupParticipantId == participantId) : group.Participants.Find(p => p.ParticipantId == participantId);

            var emailTemplate = _communicationRepository.GetTemplate(emailTemplateId);
            var fromContact = new MpContact
            {
                ContactId = emailTemplate.FromContactId,
                EmailAddress = emailTemplate.FromEmailAddress
            };
            var replyTo = new MpContact
            {
                ContactId = fromParticipant == null ? emailTemplate.ReplyToContactId : fromParticipant.ContactId,
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

            var subjectTemplateText = string.IsNullOrWhiteSpace(subjectTemplateContentBlockTitle) ? string.Empty : _contentBlockService[subjectTemplateContentBlockTitle].Content;
            var emailTemplateText = string.IsNullOrWhiteSpace(emailTemplateContentBlockTitle) ? string.Empty : _contentBlockService[emailTemplateContentBlockTitle].Content;
            var mergeData = getDictionary(participant);
            mergeData["Email_Template_Text"] = emailTemplateText;
            mergeData["Email_Custom_Message"] = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
            mergeData["Subject_Template_Text"] = subjectTemplateText;
            mergeData["Group_Name"] = group.GroupName;
            mergeData["Group_Description"] = group.GroupDescription;
            if (fromParticipant != null)
            {
                mergeData["From_Display_Name"] = fromParticipant.DisplayName;
                mergeData["From_Preferred_Name"] = fromParticipant.PreferredName;
            }
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

        public MyGroup VerifyCurrentUserIsGroupLeader(string token, int groupTypeId, int groupId)
        {
            var groups = _groupService.GetGroupsByTypeForAuthenticatedUser(token, groupTypeId, groupId);
            var group = groups == null || !groups.Any() ? null : groups.FirstOrDefault();

            if (group == null)
            {
                throw new GroupNotFoundForParticipantException(string.Format("Could not find group {0} for user", groupId));
            }

            var groupParticipants = group.Participants;
            var me = _participantRepository.GetParticipantRecord(token);

            if (groupParticipants?.Find(p => p.ParticipantId == me.ParticipantId) == null ||
                groupParticipants.Find(p => p.ParticipantId == me.ParticipantId).GroupRoleId != _groupRoleLeaderId)
            {
                throw new NotGroupLeaderException(string.Format("User is not a leader of group {0}", groupId));
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
                var myGroup = VerifyCurrentUserIsGroupLeader(token, groupTypeId, groupId);

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
                throw new GroupParticipantRemovalException(string.Format("Could not add Inquirier {0} from group {1}", inquiry.InquiryId, groupId), e);
            }
        }

        private void ApproveInquiry(int groupId, GroupDTO group, Inquiry inquiry, Participant me, string message)
        {
            _groupService.addContactToGroup(groupId, inquiry.ContactId);
            _groupRepository.UpdateGroupInquiry(groupId, inquiry.InquiryId, true);

            try
            {
                //TODO:: Update template id
                SendApproveDenyInquiryEmail(
                    true,
                    groupId,
                    group,
                    inquiry,
                    me,
                    _removeParticipantFromGroupEmailTemplateId,
                    GroupToolApproveInquiryEmailTemplateText,
                    message);
            }
            catch (Exception e)
            {
                _logger.Warn(string.Format("Could not send email to Inquirier {0} notifying of being approved to group {1}", inquiry.InquiryId, groupId), e);
            }
        }

        private void DenyInquiry(int groupId, GroupDTO group, Inquiry inquiry, Participant me, string message)
        {
            _groupRepository.UpdateGroupInquiry(groupId, inquiry.InquiryId, false);

            try
            {
                //TODO:: Update template id
                SendApproveDenyInquiryEmail(
                    false,
                    groupId,
                    group,
                    inquiry,
                    me,
                    _removeParticipantFromGroupEmailTemplateId,
                    GroupToolDenyInquiryEmailTemplateText,
                    message);
            }
            catch (Exception e)
            {
                _logger.Warn(string.Format("Could not send email to Inquirier {0} notifying of being approved to group {1}", inquiry.InquiryId, groupId), e);
            }
        }

        private void SendApproveDenyInquiryEmail(bool approve, int groupId, GroupDTO group, Inquiry inquiry, Participant me, int emailTemplateId, string emailTemplateContentBlockTitle, string message)
        {
            try
            {
                var subject = approve ? GroupToolApproveInquirySubjectTemplateText : GroupToolDenyInquirySubjectTemplateText;
                var participant = _participantRepository.GetParticipant(inquiry.ContactId);

                SendGroupParticipantEmail(groupId,
                                          participant.ParticipantId,
                                          false,
                                          group,
                                          emailTemplateId,
                                          subject,
                                          emailTemplateContentBlockTitle,
                                          message,
                                          me);
            }
            catch (Exception e)
            {
                _logger.Warn(string.Format("Could not send email to Inquirer {0} notifying for group {1}", inquiry.InquiryId, groupId), e);
            }
        }

	public void SendAllGroupParticipantsEmail(string token, int groupId, int groupTypeId, string subject, string body)
        {
            var leaderRecord = _participantRepository.GetParticipantRecord(token);
            var groups = _groupService.GetGroupsByTypeForAuthenticatedUser(token, groupTypeId, groupId);

            ValidateUserAsLeader(token, groupId, groupTypeId, leaderRecord.ParticipantId, groups);

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

            List<MpContact> toContacts = groups.First().Participants.Select(groupParticipant => new MpContact
            {
                ContactId = groupParticipant.ContactId, EmailAddress = groupParticipant.Email
            }).ToList();

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

        public void ValidateUserAsLeader(string token, int groupTypeId, int groupId, int groupParticipantId, List<GroupDTO> groups)
        {
            if (groups == null || !groups.Any())
            {
                throw new GroupNotFoundForParticipantException(string.Format("Could not find group {0} for groupParticipant {1}", groupId, groupParticipantId));
            }

            var groupParticipants = groups.FirstOrDefault().Participants;
            var me = _participantRepository.GetParticipantRecord(token);

            if (groupParticipants == null || groupParticipants.Find(p => p.ParticipantId == me.ParticipantId) == null ||
                groupParticipants.Find(p => p.ParticipantId == me.ParticipantId).GroupRoleId != _groupRoleLeaderId)
            {
                throw new NotGroupLeaderException(string.Format("Group participant {0} is not a leader of group {1}", groupParticipantId, groupId));
            }
        }
    }
}
