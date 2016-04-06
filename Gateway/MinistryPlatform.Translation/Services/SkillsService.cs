using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class SkillsService : BaseService, ISkillsService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public SkillsService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpGoVolunteerSkill> GetGoVolunteerSkills(string token)
        {
            var goSkillsPageId = _configurationWrapper.GetConfigIntValue("GoVolunteerSkills");
            var records = _ministryPlatformService.GetRecordsDict(goSkillsPageId, token);

            return records.Select(record => new MpGoVolunteerSkill(record.ToInt("Go_Volunteer_Skills_ID"),
                                                                   record.ToString("Label"),
                                                                   record.ToString("Attribute_Name"),
                                                                   record.ToInt("Attribute_ID"))).ToList();
        }
    }
}