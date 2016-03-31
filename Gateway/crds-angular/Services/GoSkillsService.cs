using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IObjectAttributeService = crds_angular.Services.Interfaces.IObjectAttributeService;

namespace crds_angular.Services
{
    public class GoSkillsService : IGoSkillsService
    {
        private readonly IApiUserService _apiUserService;
        private readonly ISkillsService _skillsService;
        private readonly crds_angular.Services.Interfaces.IObjectAttributeService _objectAttributeService;

        public GoSkillsService(IApiUserService apiUserService, ISkillsService skillsService, IObjectAttributeService objectAttributeService)
        {
            _apiUserService = apiUserService;
            _skillsService = skillsService;
            _objectAttributeService = objectAttributeService;
        }

        public List<GoSkills> RetrieveGoSkills()
        {
            //make this a parm?
            var contactId = 768379;
            var token = _apiUserService.GetToken();
            var skills = _skillsService.GetGoVolunteerSkills(token);


            var configuration = ObjectAttributeConfigurationFactory.Contact();
            var attributesTypes =_objectAttributeService.GetObjectAttributes(token,contactId,configuration);

            ObjectAttributeTypeDTO value;
            attributesTypes.MultiSelect.TryGetValue(24, out value);

            //can we updated 'checked' on skills list when there's a match in attributetypes?
            foreach (var skill in skills)
            {
                if (value != null)
                {
                    var thisValue = value.Attributes.SingleOrDefault(w => w.AttributeId == skill.AttributeId);
                }
                var inList = value != null && value.Attributes.Any(w => w.AttributeId == skill.AttributeId && w.Selected);
                if (inList)
                {
                    Console.WriteLine("hi");
                    skill.Checked =true;

                }
            }




            return new GoSkills().ToGoSkills(skills);
        }
    }
}