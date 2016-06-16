using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ProjectTypeRepository : BaseService,IProjectTypeService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public ProjectTypeRepository(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpProjectType> GetProjectTypes()
        {
            var token = ApiLogin();
            var result = _ministryPlatformService.GetRecordsDict(_configurationWrapper.GetConfigIntValue("ProjectTypes"), token);
            return MapProjectTypes(result);
        }

        private static List<MpProjectType> MapProjectTypes(IEnumerable<Dictionary<string, object>> records)
        {
            return records.Select(record => new MpProjectType
            {
                ProjectTypeId = record.ToInt("dp_RecordID"),
                Description = record.ToString("Description"),
                MinAge = record.ToInt("Minimum_Age"),
                SortOrder = record.ToInt("Sort_Order"),
                ImageUrl = record.ToString("Image_URL")
            }).ToList();
        }
    }
}
