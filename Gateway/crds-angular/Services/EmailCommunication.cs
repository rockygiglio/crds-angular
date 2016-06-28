using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using IPersonService = crds_angular.Services.Interfaces.IPersonService;

namespace crds_angular.Services
{
    public class EmailCommunication : IEmailCommunication
    {
        private readonly ICommunicationRepository _communicationService;
        private readonly IPersonService _personService;
        private readonly IContactRepository _contactService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly int DefaultContactEmailId;
        private readonly int DomainID;
        private readonly int DefaultAuthorUserId;

        public EmailCommunication(ICommunicationRepository communicationService, 
            IPersonService personService, 
            IContactRepository contactService,
            IConfigurationWrapper configurationWrapper)
        {
            _communicationService = communicationService;
            _personService = personService;
            _contactService = contactService;
            _configurationWrapper = configurationWrapper;
            DefaultContactEmailId = _configurationWrapper.GetConfigIntValue("DefaultContactEmailId");
            DomainID = _configurationWrapper.GetConfigIntValue("DomainId");
            DefaultAuthorUserId = _configurationWrapper.GetConfigIntValue("DefaultAuthorUser");
        }

        public void SendEmail(EmailCommunicationDTO email, string token)
        {
            var communication = new MpCommunication();
            communication.DomainId = DomainID;
            communication.AuthorUserId = email.FromUserId ?? DefaultAuthorUserId;

            if (token == null && email.FromUserId == null)
            {
                throw (new InvalidOperationException("Must provide either email.FromUserId or an authentication token."));
            }

            var replyToContactId = email.ReplyToContactId ?? DefaultContactEmailId;
            var from = new MpContact { ContactId = DefaultContactEmailId, EmailAddress = _communicationService.GetEmailFromContactId(DefaultContactEmailId) };
            var replyTo = new MpContact { ContactId = replyToContactId, EmailAddress = _communicationService.GetEmailFromContactId(replyToContactId) };
            var recipient = new MpContact {ContactId = email.ToContactId, EmailAddress = _communicationService.GetEmailFromContactId(email.ToContactId) };

            communication.FromContact = from;
            communication.ReplyToContact = replyTo;
            communication.ToContacts.Add(recipient);

            var template = _communicationService.GetTemplate(email.TemplateId);
            communication.TemplateId = email.TemplateId;
            communication.EmailBody = template.Body;
            communication.EmailSubject = template.Subject;

            communication.MergeData = email.MergeData;

            if (!communication.MergeData.ContainsKey("BaseUrl"))
            {
                communication.MergeData.Add("BaseUrl", _configurationWrapper.GetConfigValue("BaseUrl"));
            }

            _communicationService.SendMessage(communication);
        }

        public void SendEmail(CommunicationDTO emailData)
        {
            var replyToContactId = emailData.ReplyToContactId ?? DefaultContactEmailId;
                
            var from = new MpContact { ContactId = DefaultContactEmailId, EmailAddress = _communicationService.GetEmailFromContactId(DefaultContactEmailId) };
            var replyTo = new MpContact { ContactId = replyToContactId, EmailAddress = _communicationService.GetEmailFromContactId(replyToContactId) };

            var comm = new MpCommunication
            {
                AuthorUserId = DefaultAuthorUserId,
                DomainId = DomainID,
                EmailBody = emailData.Body,
                EmailSubject = emailData.Subject,
                FromContact = from,
                ReplyToContact = replyTo,
                MergeData = new Dictionary<string, object>(),
                ToContacts = new List<MpContact>()
            };
            foreach (var to in emailData.ToContactIds)
            {
                var contact  = new MpContact { ContactId = to, EmailAddress = _communicationService.GetEmailFromContactId(to) };
                comm.ToContacts.Add(contact);
            }
            _communicationService.SendMessage(comm);
        }
    }
}
