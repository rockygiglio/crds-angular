using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using crds_angular.Models.Crossroads.Waivers;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class WaiverService : IWaiverService
    {
        private readonly IWaiverRepository _waiverRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ICommunicationRepository _communicationRepository;
        private readonly IEventParticipantRepository _eventParticipantRepository;
        private readonly IConfigurationWrapper _confiurationWrapper;


        public WaiverService(IWaiverRepository waiverRepository, 
            IAuthenticationRepository authenticationRepository, 
            IInvitationRepository invitationRepository, 
            IContactRepository contactRepository, 
            ICommunicationRepository communicationRepository, 
            IEventParticipantRepository eventParticipantRepository,
            IConfigurationWrapper configurationWrapper)
        {
            _waiverRepository = waiverRepository;
            _authenticationRepository = authenticationRepository;
            _invitationRepository = invitationRepository;
            _contactRepository = contactRepository;
            _communicationRepository = communicationRepository;
            _eventParticipantRepository = eventParticipantRepository;
            _confiurationWrapper = configurationWrapper;
        }

        public IObservable<WaiverDTO> GetWaiver(int waiverId)
        {            
            return _waiverRepository.GetWaiver(waiverId).Select<MpWaivers, WaiverDTO>(w => new WaiverDTO
            {
                Accepted = w.Accepted,
                Required = w.Required,
                SigneeContactId = w.SigneeContactId,
                WaiverId = w.WaiverId,
                WaiverName = w.WaiverName,
                WaiverText = w.WaiverText
            });
        }

        public IObservable<WaiverDTO> EventWaivers(int eventId)
        {
            return _waiverRepository.GetEventWaivers(eventId).Select(w => new WaiverDTO
            {
                WaiverId = w.WaiverId,
                Required = w.Required,
                WaiverName = w.WaiverName,
                WaiverText = w.WaiverText
            });
        }

        public IObservable<int> SendAcceptanceEmail(ContactInvitation contactInvitation)
        {
            var templateId = _confiurationWrapper.GetConfigIntValue("WaiverEmailTemplateId");
            var template = _communicationRepository.GetTemplate(templateId);               

            // get the event name for the waiver...
            return _eventParticipantRepository.GetEventParticpant(contactInvitation.Invitation.SourceId).Select(ep =>
            {
                var mergeData = new Dictionary<string, object>
                {
                    {"Event_Name", ep.EventTitle },
                    {"Confirmation_Url", $"https://{_confiurationWrapper.GetConfigValue("BaseUrl")}/waivers/accept-waiver/{contactInvitation.Invitation.InvitationGuid}" }
                };

                var comm = _communicationRepository.GetTemplateAsCommunication(templateId,
                                                                                template.FromContactId,
                                                                                template.FromEmailAddress,
                                                                                template.ReplyToContactId,
                                                                                template.ReplyToEmailAddress,
                                                                                contactInvitation.Contact.Contact_ID,
                                                                                contactInvitation.Contact.Email_Address,
                                                                                mergeData);
                return _communicationRepository.SendMessage(comm);
            });
        }

        public IObservable<ContactInvitation> CreateWaiverInvitation(int waiverId, int eventParticipantId, string token)
        {           
            var contactId = _authenticationRepository.GetContactId(token);
            var contact = _contactRepository.GetContactById(contactId);

            return _waiverRepository.CreateEventParticipantWaiver(waiverId, eventParticipantId, contactId).SelectMany(eventParticipantWaiver =>
            {
                // create private invite
                var mpInvitation = new MpInvitation
                {
                    SourceId = eventParticipantWaiver.EventParticipantId,
                    EmailAddress = contact.Email_Address,
                    InvitationType = _confiurationWrapper.GetConfigIntValue("WaiverInvitationType"),
                    RecipientName = $"{contact.Nickname ?? contact.First_Name} {contact.Last_Name}",
                    RequestDate = DateTime.Now
                };

                return _invitationRepository.CreateInvitationAsync(mpInvitation).Select(invite => new ContactInvitation
                {
                    Contact = contact,
                    Invitation = invite
                });
            });            
        }
    }

}