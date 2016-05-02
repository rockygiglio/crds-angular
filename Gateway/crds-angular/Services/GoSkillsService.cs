using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IObjectAttributeService = crds_angular.Services.Interfaces.IObjectAttributeService;

namespace crds_angular.Services
{
    public class GoSkillsService : IGoSkillsService
    {
        private readonly IApiUserService _apiUserService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;
        private readonly IObjectAttributeService _objectAttributeService;
        private readonly ISkillsService _skillsService;

        public GoSkillsService(IApiUserService apiUserService,
                               ISkillsService skillsService,
                               IObjectAttributeService objectAttributeService,
                               IContactService contactService,
                               IConfigurationWrapper configurationWrapper)
        {
            _apiUserService = apiUserService;
            _skillsService = skillsService;
            _objectAttributeService = objectAttributeService;
            _contactService = contactService;
            _configurationWrapper = configurationWrapper;
        }

        public List<GoSkills> RetrieveGoSkills(string token)
        {
            // get all the skill attributes
            var apiToken = _apiUserService.GetToken();
            var skills = _skillsService.GetGoVolunteerSkills(apiToken);

            // if the user is logged in, check if they have skills
            if (token == string.Empty)
            {
                // not logged in, get out
                return new GoSkills().ToGoSkills(skills);
            }

            // get skills using the logged in users token
            var contactSkills = ContactSkills(token, apiToken);

            // match our list to the users, update "checked" to true when appropriate
            if (contactSkills != null)
            {
                foreach (var skill in skills.Where(skill => contactSkills.Attributes.Any(s => s.AttributeId == skill.AttributeId && s.Selected)))
                {
                    skill.Checked = true;                    
                }
            }

            return new GoSkills().ToGoSkills(skills);
        }

        public void UpdateSkills(int participantId, List<GoSkills> skills, string token)
        {
            ObjectAttributeConfiguration configuration;
            if (token == String.Empty)
            {
                token = _apiUserService.GetToken();
                configuration = ObjectAttributeConfigurationFactory.Contact();
            }
            else
            {
                configuration = ObjectAttributeConfigurationFactory.MyContact();
            }

            var contactObs = Observable.Start(() => _contactService.GetContactByParticipantId(participantId));
            contactObs.Subscribe(con =>
            {
                var attrs = Observable.Start(() => _objectAttributeService.GetObjectAttributes(token, con.Contact_ID, configuration));

                attrs.Subscribe(attr =>
                {
                    var curSk = attr.MultiSelect
                        .FirstOrDefault(kvp => kvp.Value.AttributeTypeId == _configurationWrapper.GetConfigIntValue("AttributeTypeIdSkills")).Value.Attributes
                        .Where(attribute => attribute.Selected).ToList();

                    var skillsEndDate = SkillsToEndDate(skills, curSk).Select(sk =>
                    {
                        sk.EndDate = DateTime.Now;
                        return sk;
                    });

                    var addSkills = SkillsToAdd(skills, curSk).ToList();
                    var allSkills = addSkills.Concat(skillsEndDate).ToList();
                    var allSkillsObs = allSkills.ToObservable();
                    try
                    {
                        allSkillsObs.ForEachAsync(skill =>
                        {
                            _objectAttributeService.SaveObjectMultiAttribute(token, con.Contact_ID, skill, configuration, true);
                        });
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException("Updating skills caused an error");
                    }
                });
            });                        
          
            

        }        

        public List<ObjectAttributeDTO> SkillsToEndDate(List<GoSkills> skills, List<ObjectAttributeDTO> currentSkills)
        {
            return currentSkills.Where(sk =>
            {
                var contains = skills.Where(s => s.AttributeId == sk.AttributeId).ToList();
                return contains.Count == 0;
            }).ToList();
        }

        public List<ObjectAttributeDTO> SkillsToAdd(List<GoSkills> skills, List<ObjectAttributeDTO> currentSkills)
        {
            return skills.Where(s =>
            {
                var contains = currentSkills.Where(c => c.AttributeId == s.AttributeId);
                return !contains.Any();
            }).Select(s => new ObjectAttributeDTO()
            {
                AttributeId = s.AttributeId,
                Name = s.Name,
                StartDate = DateTime.Now
            }).ToList();
        } 

        private ObjectAttributeTypeDTO ContactSkills(string token, string apiToken)
        {
            var contact = _contactService.GetMyProfile(token);
            var configuration = ObjectAttributeConfigurationFactory.Contact();
            var attributesTypes = _objectAttributeService.GetObjectAttributes(apiToken, contact.Contact_ID, configuration);
            ObjectAttributeTypeDTO contactSkills;
            var skillsAttributeTypeId = _configurationWrapper.GetConfigIntValue("AttributeTypeIdSkills");
            attributesTypes.MultiSelect.TryGetValue(skillsAttributeTypeId, out contactSkills);
            return contactSkills;
        }
    }
}