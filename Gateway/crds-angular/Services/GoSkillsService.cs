using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class GoSkillsService : IGoSkillsService
    {
        private readonly IApiUserService _apiUserService;
        private readonly ISkillsService _skillsService;

        public GoSkillsService(IApiUserService apiUserService, ISkillsService skillsService)
        {
            _apiUserService = apiUserService;
            _skillsService = skillsService;
        }

        public List<GoSkills> RetrieveGoSkills()
        {
            var token = _apiUserService.GetToken();
            var skills = _skillsService.GetGoVolunteerSkills(token);
            return new GoSkills().ToGoSkills(skills);
        }
    }
}