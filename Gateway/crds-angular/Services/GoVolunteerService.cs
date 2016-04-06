using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces.GoCincinnati;
using IGroupConnectorService = MinistryPlatform.Translation.Services.Interfaces.GoCincinnati.IGroupConnectorService;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class GoVolunteerService : IGoVolunteerService

    {
        private readonly IAttributeService _attributeService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly MPInterfaces.IContactRelationshipService _contactRelationshipService;
        private readonly MPInterfaces.IContactService _contactService;
        private readonly IGroupConnectorService _groupConnectorService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (GoVolunteerService));
        private readonly MPInterfaces.IParticipantService _participantService;
        private readonly MPInterfaces.IProjectTypeService _projectTypeService;
        private readonly IRegistrationService _registrationService;

        public GoVolunteerService(MPInterfaces.IParticipantService participantService,
                                  IRegistrationService registrationService,
                                  MPInterfaces.IContactService contactService,
                                  IGroupConnectorService groupConnectorService,
                                  IConfigurationWrapper configurationWrapper,
                                  MPInterfaces.IContactRelationshipService contactRelationshipService,
                                  MPInterfaces.IProjectTypeService projectTypeService,
                                  IAttributeService attributeService)
        {
            _participantService = participantService;
            _registrationService = registrationService;
            _contactService = contactService;
            _groupConnectorService = groupConnectorService;
            _configurationWrapper = configurationWrapper;
            _contactRelationshipService = contactRelationshipService;
            _projectTypeService = projectTypeService;
            _attributeService = attributeService;
        }

        public List<ChildrenOptions> ChildrenOptions()
        {
            var attributeTypeId = _configurationWrapper.GetConfigIntValue("GoCincinnatiRegistrationChildrenAttributeType");
            var attributeTypes = _attributeService.GetAttributeTypes(attributeTypeId);
            var attributes = attributeTypes.Single().Attributes;
            return attributes.Select(attribute => new ChildrenOptions
            {
                AttributeId = attribute.AttributeId,
                PluralLabel = string.Concat("Children Ages ", attribute.Name),
                SingularLabel = string.Concat("Child Age ", attribute.Name),
                Value = 0
            }).ToList();
        }

        public bool CreateRegistration(Registration registration, string token)
        {
            var registrationDto = new MinistryPlatform.Translation.Models.GoCincinnati.Registration();

            try
            {
                registrationDto.ParticipantId = RegistrationContact(registration, token, registrationDto);
                var registrationId = CreateRegistration(registration, registrationDto);
                GroupConnector(registration, registrationId);
                SpouseInformation(registration);
                Attributes(registration, registrationId);
            }
            catch (Exception ex)
            {
                var msg = "Go Volunteer Service: CreateRegistration";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
            return true;
        }

        public List<ProjectType> GetProjectTypes()
        {
            //var apiUserToken = _mpApiUserService.GetToken();

            var pTypes = _projectTypeService.GetProjectTypes();
            return pTypes.Select(pt =>
            {
                var projType = new ProjectType();
                return projType.FromMpProjectType(pt);
            }).ToList();
        }

        private void Attributes(Registration registration, int registrationId)
        {
            ChildAgeGroups(registration, registrationId);
            PrepWork(registration, registrationId);
            Equipment(registration, registrationId);
            ProjectPreferences(registration, registrationId);
        }

        private void ProjectPreferences(Registration registration, int registrationId)
        {
            foreach (var projectPreference in registration.ProjectPreferences.Where(pref => pref.Id != 0))
            {
                _registrationService.AddProjectPreferences(registrationId, projectPreference.Id, projectPreference.Priority);
            }
        }

        private void Equipment(Registration registration, int registrationId)
        {
            foreach (var equipment in registration.Equipment.Where(equipment => equipment.Id != 0))
            {
                _registrationService.AddEquipment(registrationId, equipment.Id, equipment.Notes);
            }
        }

        private void PrepWork(Registration registration, int registrationId)
        {
            foreach (var prepWork in registration.PrepWork)
            {
                _registrationService.AddPrepWork(registrationId, prepWork.Id, prepWork.Spouse);
            }
        }

        private void ChildAgeGroups(Registration registration, int registrationId)
        {
            foreach (var ageGroup in registration.ChildAgeGroup)
            {
                _registrationService.AddAgeGroup(registrationId, ageGroup.Id, ageGroup.Count);
            }
        }

        private void SpouseInformation(Registration registration)
        {
            if (!registration.SpouseParticipation)
            {
                return;
            }
            if (registration.Spouse.ContactId != 0)
            {
                return;
            }

            var contactId = _contactService.CreateSimpleContact(registration.Spouse.FirstName, registration.Spouse.LastName, registration.Spouse.EmailAddress);

            // create relationship
            var relationship = new MinistryPlatform.Models.Relationship
            {
                RelationshipID = _configurationWrapper.GetConfigIntValue("MarriedTo"),
                RelatedContactID = contactId,
                StartDate = DateTime.Today
            };
            _contactRelationshipService.AddRelationship(relationship, registration.Self.ContactId);
        }

        private void GroupConnector(Registration registration, int registrationId)
        {
            if (registration.CreateGroupConnector)
            {
                _groupConnectorService.CreateGroupConnector(registrationId);
            }
            else if (registration.GroupConnectorId != 0)
            {
                _groupConnectorService.CreateGroupConnectorRegistration(registration.GroupConnectorId, registrationId);
            }
            // TODO add a flag for lone wolfs to make the group connector private vs. public
            //else // lone wolf
            //{
            //    //var loneWolf = true;
            //    //_groupConnectorService.CreateGroupConnector(registrationId, loneWolf);
            //}
        }

        private int CreateRegistration(Registration registration, MinistryPlatform.Translation.Models.GoCincinnati.Registration registrationDto)
        {
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
            return registrationId;
        }

        private int RegistrationContact(Registration registration, string token, MinistryPlatform.Translation.Models.GoCincinnati.Registration registrationDto)
        {
            // do we need to create a contact?
            // how do we know that we need to create a contact?
            // Create or Update Contact
            //MinistryPlatform.Models.Participant participant = null;
            int participantId;
            if (registration.Self.ContactId != 0)
            {
                // update name/email

                //get participant
                var participant = _participantService.GetParticipantRecord(token);
                participantId = participant.ParticipantId;
            }
            else
            {
                //create contact & participant
                var contactId = _contactService.CreateSimpleContact(registration.Self.FirstName, registration.Self.LastName, registration.Self.EmailAddress);
                registration.Self.ContactId = contactId;
                participantId = _participantService.CreateParticipantRecord(contactId);
                //registrationDto.ParticipantId = participantId;
            }

            //why do i care about this?
            if (participantId == 0)
            {
                throw new ApplicationException("Nooooooo");
            }
            return participantId;
        }
    }
}