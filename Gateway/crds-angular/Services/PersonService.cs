using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;
using MinistryPlatform.Translation.Repositories;
using MPServices = MinistryPlatform.Translation.Repositories.Interfaces;


namespace crds_angular.Services
{
    public class PersonService : MinistryPlatformBaseService, IPersonService
    {
        private readonly MPServices.IContactRepository _contactRepository;
        private readonly IObjectAttributeService _objectAttributeService;
        private readonly IApiUserRepository _apiUserService;
        private readonly MPServices.IParticipantRepository _participantService;
        private readonly MPServices.IUserRepository _userRepository;
        private readonly IAuthenticationRepository _authenticationService;

        public PersonService(MPServices.IContactRepository contactService, 
            IObjectAttributeService objectAttributeService, 
            IApiUserRepository apiUserService,
            MPServices.IParticipantRepository participantService,
            MPServices.IUserRepository userService,
            IAuthenticationRepository authenticationService)
        {
            _contactRepository = contactService;
            _objectAttributeService = objectAttributeService;
            _apiUserService = apiUserService;
            _participantService = participantService;
            _userRepository = userService;
            _authenticationService = authenticationService;
        }

        public void SetProfile(string token, Person person)
        {
            var contactDictionary = getDictionary(person.GetContact());
            var householdDictionary = getDictionary(person.GetHousehold());
            var addressDictionary = getDictionary(person.GetAddress());
            addressDictionary.Add("State/Region", addressDictionary["State"]);
            _contactRepository.UpdateContact(person.ContactId, contactDictionary, householdDictionary, addressDictionary);
            var configuration = MpObjectAttributeConfigurationFactory.Contact();            
            _objectAttributeService.SaveObjectAttributes(person.ContactId, person.AttributeTypes, person.SingleAttributes, configuration);

            var participant = _participantService.GetParticipant(person.ContactId);
            if (participant.AttendanceStart != person.AttendanceStartDate)
            {                
                participant.AttendanceStart = person.AttendanceStartDate;
                _participantService.UpdateParticipant(participant);
            }

            // TODO: It appears we are updating the contact records email address above if the email address is changed
            // TODO: If the password is invalid we would not run the update on user, and therefore create a data integrity problem
            // TODO: See About moving the check for new password above or moving the update for user / person into an atomic operation
            //
            // update the user values if the email and/or password has changed
            if (!(String.IsNullOrEmpty(person.NewPassword)) || (person.EmailAddress != person.OldEmail && person.OldEmail != null))
            {
                var authData = _authenticationService.Authenticate(person.OldEmail, person.OldPassword);

                if (authData == null)
                {
                    throw new Exception("Old password did not match profile");
                }
                else
                {
                    var userUpdateValues = person.GetUserUpdateValues();
                    userUpdateValues["User_ID"] = _userRepository.GetUserIdByUsername(person.OldEmail);
                    _userRepository.UpdateUser(userUpdateValues);
                }
            }
        }

        public Person GetPerson(int contactId)
        {
            var contact = _contactRepository.GetContactById(contactId);
            var person = Mapper.Map<Person>(contact);

            var family = _contactRepository.GetHouseholdFamilyMembers(person.HouseholdId);
            person.HouseholdMembers = family;

            // TODO: Should this move to _contactService or should update move it's call out to this service?
            var apiUser = _apiUserService.GetToken();
            var configuration = MpObjectAttributeConfigurationFactory.Contact();
            var attributesTypes = _objectAttributeService.GetObjectAttributes(apiUser, contactId, configuration);
            person.AttributeTypes = attributesTypes.MultiSelect;
            person.SingleAttributes = attributesTypes.SingleSelect;

            return person;
        }

        public List<MpRoleDto> GetLoggedInUserRoles(string token)
        {
            return GetMyRecordsRepository.GetMyRoles(token);
        }

        public Person GetLoggedInUserProfile(String token)
        {
            var contact = _contactRepository.GetMyProfile(token);
            var person = Mapper.Map<Person>(contact);

            var family = _contactRepository.GetHouseholdFamilyMembers(person.HouseholdId);
            person.HouseholdMembers = family;

            return person;
        }
    }
}
