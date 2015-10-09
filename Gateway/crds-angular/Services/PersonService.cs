using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Models;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using IPersonService = crds_angular.Services.Interfaces.IPersonService;


namespace crds_angular.Services
{
    public class PersonService : MinistryPlatformBaseService, IPersonService
    {
        private readonly IContactService _contactService;
        private readonly IContactAttributeService _contactAttributeService;

        public PersonService(IContactService contactService, IContactAttributeService contactAttributeService)
        {
            _contactService = contactService;
            _contactAttributeService = contactAttributeService;
        }

        public void SetProfile(String token, Person person)
        {
            var contactDictionary = getDictionary(person.GetContact());
            var householdDictionary = getDictionary(person.GetHousehold());
            var addressDictionary = getDictionary(person.GetAddress());
            addressDictionary.Add("State/Region", addressDictionary["State"]);

            _contactService.UpdateContact(person.ContactId, contactDictionary, householdDictionary, addressDictionary);
        }

        public List<Skill> GetLoggedInUserSkills(int contactId, string token)
        {
            return GetSkills(contactId, token);
        }

        public Person GetPerson(int contactId)
        {
            var contact = _contactService.GetContactById(contactId);
            var person = Mapper.Map<Person>(contact);

            var family = _contactService.GetHouseholdFamilyMembers(person.HouseholdId);
            person.HouseholdMembers = family;

            var attributes = _contactAttributeService.GetCurrentContractAttributes(contactId);
            person.Attributes = attributes;

            return person;
        }

        public List<RoleDto> GetLoggedInUserRoles(string token)
        {
            return GetMyRecords.GetMyRoles(token);
        }

        public Person GetLoggedInUserProfile(String token)
        {
            var contact = _contactService.GetMyProfile(token);
            var person = Mapper.Map<Person>(contact);

            var family = _contactService.GetHouseholdFamilyMembers(person.HouseholdId);
            person.HouseholdMembers = family;

            return person;
        }

        private List<Skill> GetSkills(int recordId, string token)
        {
            var attributes = GetMyRecords.GetMyAttributes(recordId, token);

            var skills =
                Mapper.Map<List<ContactAttribute>, List<Skill>>(attributes);

            return skills;
        }
    }
}