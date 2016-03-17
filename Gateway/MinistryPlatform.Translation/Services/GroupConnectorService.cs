using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupConnectorService : BaseService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public GroupConnectorService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpGroupConnector> GetGroupConnectors(string organizationName)
        {
            var token = ApiLogin();
            var searchString = ",,," + organizationName;
            var result = _ministryPlatformService.GetRecordsDict(_configurationWrapper.GetConfigIntValue("GroupConnectorPage"), token, searchString);

            return result.Select(r => new MpGroupConnector
            {
                Id = r.ToInt("Group_Connector_ID"),
                Name = r.ToString("Primary Registration"),
                ProjectName = r.ToString("Project Name"),
                PreferredLaunchSite = r.ToString("Preferred Launch Site")
            }).ToList();
        }
    }
}