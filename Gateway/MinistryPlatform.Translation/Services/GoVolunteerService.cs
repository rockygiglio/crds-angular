using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GoVolunteerService : BaseService,IGoVolunteerService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public GoVolunteerService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<ProjectType> GetProjectTypes(string token)
        {
            var result = _ministryPlatformService.GetRecordsDict(_configurationWrapper.GetConfigIntValue("ProjectTypes"), token);
            return MapProjectTypes(result);
        }

        private static List<ProjectType> MapProjectTypes(IEnumerable<Dictionary<string, object>> records)
        {
            return records.Select(record => new ProjectType
            {
                ProjectTypeId = record.ToInt(""),
                Description = record.ToString(""),
                MinAge = record.ToInt(""),
                SortOrder = record.ToInt(""),
                ImageUrl = record.ToString("")
            }).ToList();
        }
    }
}
