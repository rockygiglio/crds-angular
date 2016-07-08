using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class InvitationRepository : BaseRepository, IInvitationRepository
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationRepository _communicationService;
        private readonly IContactRepository _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly int _invitationPageId;
        private readonly int _invitationEmailTemplateId;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private IMinistryPlatformService _ministryPlatformService;

        public InvitationRepository(IMinistryPlatformService ministryPlatformService,
                               IConfigurationWrapper configurationWrapper,
                               IAuthenticationRepository authenticationService,
                               ICommunicationRepository communicationService,
                               IContactRepository contactService)
            : base(authenticationService, configurationWrapper)
        {
            this._ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._contactService = contactService;
            this._communicationService = communicationService;
            this._invitationPageId = _configurationWrapper.GetConfigIntValue("InvitationPageID");
            this._invitationEmailTemplateId = _configurationWrapper.GetConfigIntValue("InvitationEmailTemplateId");
        }

        public int CreateInvitation(MpInvitation dto, string token)
        {
            var invitationType = (int)dto.InvitationType;

            var values = new Dictionary<string, object>
            {
                {"Source_ID", dto.SourceId},
                {"Email_Address", dto.EmailAddress},
                {"Recipient_Name", dto.RecipientName},
                {"Group_Role_ID", dto.GroupRoleId },
                {"Invitation_Type_ID", invitationType }
            };

            try
            {
                var invitationId = _ministryPlatformService.CreateRecord(_invitationPageId, values, token, true);
                SendEmail(dto.EmailAddress);
                return invitationId;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Create Invitation failed.  Invitation Type: {0}, Source Id: {1}", dto.InvitationType, dto.SourceId), e);
            }
        }

        private void SendEmail(string emailAddress)
        {
            var emailTemplate = _communicationService.GetTemplate(_invitationEmailTemplateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            var from = new MpContact
            {
                ContactId = fromContact.Contact_ID,
                EmailAddress = fromContact.Email_Address
            };

            var domainId = Convert.ToInt32(AppSettings("DomainId"));

            var to = new List<MpContact>
                {
                    new MpContact
                    {
                        ContactId = fromContact.Contact_ID,
                        EmailAddress = emailAddress
                    }
                };


            var confirmation = new MpCommunication
            {
                EmailBody = emailTemplate.Body,
                EmailSubject = emailTemplate.Subject,
                AuthorUserId = 5,
                DomainId = domainId,
                FromContact = new MpContact { ContactId = fromContact.Contact_ID, EmailAddress = fromContact.Email_Address },
                ReplyToContact = from,
                TemplateId = _invitationEmailTemplateId,
                ToContacts = to
            };
            _communicationService.SendMessage(confirmation);

        }

    }
}