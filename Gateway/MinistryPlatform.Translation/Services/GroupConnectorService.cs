using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class GroupConnectorService : BaseService, IGroupConnectorService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public GroupConnectorService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpGroupConnector> GetGroupConnectors(int organizationId, int initiativeName)
        {
            var token = ApiLogin();
            var searchString = string.Format(",,,{0},{1}", organizationId, initiativeName);
            //var pageKey = _configurationWrapper.GetConfigIntValue("GroupConnectorPage");
            //var result = _ministryPlatformService.GetRecordsDict(pageKey, token, searchString);
            var result = _ministryPlatformService.GetPageViewRecords("GroupConnectorPageView", token, searchString);

            return result.Select(r => new MpGroupConnector
            {
                Id = r.ToInt("GroupConnector_ID"),
                Name = r.ToString("Primary_Registration"),
                ProjectName = r.ToString("Project_Name"),
                PreferredLaunchSite = r.ToString("Preferred_Launch_Site")
            }).ToList();
        }
    }
}