using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class InvitationService : MinistryPlatformBaseService, IInvitationService
    {

        private readonly IInvitationRepository _invitationRepository;
        private readonly ICommunicationRepository _communicationService;
        private readonly IGroupRepository _groupRepository;
        private readonly IParticipantRepository _participantRepository;

        private readonly int _groupInvitationType;
        private readonly int _groupInvitationEmailTemplate;
        private readonly int _tripInvitationType;
        private readonly int _tripInvitationEmailTemplate;

        private readonly int _defaultInvitationEmailTemplate;
        private readonly int _domainId;

        private readonly int _groupRoleLeader;

        private readonly ILog _logger = LogManager.GetLogger(typeof(GroupToolService));

        public InvitationService(
                           IInvitationRepository invitationRepository,
                           ICommunicationRepository communicationService, 
                           IConfigurationWrapper configuration,
                           IGroupRepository groupRepository,
                           IParticipantRepository participantRepository)
        {

            _invitationRepository = invitationRepository;
            _communicationService = communicationService;
            _groupRepository = groupRepository;
            _participantRepository = participantRepository;

            _groupInvitationType = configuration.GetConfigIntValue("GroupInvitationType");
            _groupInvitationEmailTemplate = configuration.GetConfigIntValue("GroupInvitationEmailTemplate");
            _tripInvitationType = configuration.GetConfigIntValue("TripInvitationType");
            _tripInvitationEmailTemplate = configuration.GetConfigIntValue("PrivateInviteTemplate");

            _defaultInvitationEmailTemplate = configuration.GetConfigIntValue("DefaultInvitationEmailTemplate");

            _domainId = configuration.GetConfigIntValue("DomainId");

            _groupRoleLeader = configuration.GetConfigIntValue("GroupRoleLeader");
        }

        public int CreateInvitation(Invitation dto, string token)
        {
            try
            {
                var mpInvitation = Mapper.Map<MpInvitation>(dto);

                var invitation = _invitationRepository.CreateInvitation(mpInvitation, token);

                try
                {
                    SendEmail(invitation);
                }
                catch (Exception e)
                {
                    _logger.Error(string.Format("Error sending email to {0} for invitation {1}", invitation.EmailAddress, invitation.InvitationGuid), e);
                }

                return invitation.InvitationId;
            }
            catch (Exception e)
            {
                var message = string.Format("Exception creating invitation for {0}, SourceID = {1}.", dto.RecipientName, dto.SourceId);
                _logger.Error(message, e);
                return -1;
            }
        }

        public void ValidateInvitation(Invitation dto, string token)
        {
            if (dto.InvitationType == _groupInvitationType)
            {
                ValidateGroupInvitation(dto, token);
            } else if (dto.InvitationType == _tripInvitationType)
            {
                ValidateTripInvitation(dto, token);
            }

        }

        private void ValidateGroupInvitation(Invitation dto, string token)
        {
            var me = _participantRepository.GetParticipantRecord(token);
            if (me == null)
            {
                throw new ValidationException("You must be a group leader to invite others to this group (participant not found)");
            }

            var participants = _groupRepository.GetGroupParticipants(dto.SourceId, true);
            if (participants == null || !participants.Any())
            {
                throw new ValidationException("You must be a group leader to invite others to this group (no active participants)");
            }

            var found = participants.Find(p => p.ParticipantId == me.ParticipantId && p.GroupRoleId == _groupRoleLeader);
            if (found == null)
            {
                throw new ValidationException("You must be a group leader to invite others to this group (participant not a leader)");
            }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void ValidateTripInvitation(Invitation dto, string token)
        {
            // TODO Implement validation, make sure the token represents someone who is allowed to send this trip invitation
        }

        private void SendEmail(MpInvitation invitation)
        {
            int emailTemplateId;
            if (invitation.InvitationType == _groupInvitationType)
            {
                emailTemplateId = _groupInvitationEmailTemplate;
            } else if (invitation.InvitationType == _tripInvitationType)
            {
                emailTemplateId = _tripInvitationEmailTemplate;
            }
            else
            {
                emailTemplateId = _defaultInvitationEmailTemplate;
            }
            var emailTemplate = _communicationService.GetTemplate(emailTemplateId);
            var fromContact = new MpContact
            {
                ContactId = emailTemplate.FromContactId,
                EmailAddress = emailTemplate.FromEmailAddress
            };
            var replyTo = new MpContact
            {
                ContactId = emailTemplate.ReplyToContactId,
                EmailAddress = emailTemplate.ReplyToEmailAddress
            };

            var to = new List<MpContact>
                {
                    new MpContact
                    {
                        // Just need a contact ID here, doesn't have to be for the recipient
                        ContactId = emailTemplate.FromContactId,
                        EmailAddress = invitation.EmailAddress
                    }
                };


            var mergeData = new Dictionary<string, object>
            {
                { "Invitation_GUID", invitation.InvitationGuid },
                { "Recipient_Name", invitation.RecipientName },
            };
            var confirmation = new MpCommunication
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
            _communicationService.SendMessage(confirmation);

        }

    }
}