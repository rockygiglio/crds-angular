using System.Collections.Generic;
using System.Linq;
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