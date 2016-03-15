using System;
using crds_angular.Models.Crossroads.GoVolunteer;
using log4net;
using MinistryPlatform.Translation.Services.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public interface IGoVolunteerService
    {
        bool CreateRegistration(Registration registration, string token);
    }

    public class GoVolunteerService : MinistryPlatformBaseService, IGoVolunteerService

    {
        private readonly IContactService _contactService;

        private readonly ILog _logger = LogManager.GetLogger(typeof (GoVolunteerService));
        private readonly IParticipantService _participantService;
        private readonly IRegistrationService _registrationService;

        public GoVolunteerService(IParticipantService participantService, IRegistrationService registrationService, IContactService contactService)
        {
            _participantService = participantService;
            _registrationService = registrationService;
            _contactService = contactService;
        }

        public bool CreateRegistration(Registration registration, string token)
        {
            var registrationDto = new MinistryPlatform.Translation.Models.GoCincinnati.Registration();

            try
            {
                //throw new NotImplementedException("Create Registration");

                // do we need to create a contact?
                // how do we know that we need to create a contact?
                MinistryPlatform.Models.Participant participant = null;
                if (registration.Self.ContactId != 0)
                {
                    participant = _participantService.GetParticipantRecord(token);
                    registrationDto.ParticipantId = participant.ParticipantId;
                }
                else
                {
                    //create contact & participant
                    var contactId = _contactService.CreateSimpleContact(registration.Self.FirstName, registration.Self.LastName, registration.Self.EmailAddress);
                    var participantId = _participantService.CreateParticipantRecord(contactId);
                    registrationDto.ParticipantId = participantId;
                }

                if (participant == null)
                {
                    throw new ApplicationException("Nooooooo");
                }

                // Create Registration
                registrationDto.AdditionalInformation = registration.AdditionalInformation;
                registrationDto.InitiativeId = registration.InitiativeId;
                registrationDto.OrganizationId = registration.OrganizationId;
                registrationDto.PreferredLaunchSiteId = registration.PreferredLaunchSiteId;
                registrationDto.RoleId = registration.RoleId;
                registrationDto.SpouseParticipation = registration.SpouseParticipation;
                int registrationId;
                try
                {
                    registrationId = _registrationService.CreateRegistration(registrationDto);
                }
                catch (Exception ex)
                {
                    _logger.Error("GO Volunteer Service - Create Registration (Create Registration)", ex);
                    throw;
                }


                // Create or Add Group Connector
                if (registration.CreateGroupConnector)
                {
                    
                }
                else if (registration.GroupConnectorId !=0)
                {
                     
                }

                // Create or Update Contact

                // Create or Update Spouse?

                // Add Children Attributes

                // Prep Work

                // Equipment

                // Project Preferences
            }
            catch (Exception ex)
            {
                var msg = "Go Volunteer Service: CreateRegistration";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
            return true;
        }
    }
}