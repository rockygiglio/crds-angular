using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;

namespace crds_angular.Services
{
    public class GroupConnectorService : IGroupConnectorService
    {
        private readonly IGroupConnectorRepository _mpGroupConnectorService;
        private readonly IConfigurationWrapper _configWrapper;

        public GroupConnectorService(IGroupConnectorRepository groupConnectorService, IConfigurationWrapper configurationWrapper)
        {
            _mpGroupConnectorService = groupConnectorService;
            _configWrapper = configurationWrapper;
        }

        public GroupConnector GetGroupConnectorById(int groupConnectorId)
        {
            var groupConnector = _mpGroupConnectorService.GetGroupConnectorById(groupConnectorId);
            return MapGroupConnector(groupConnector);
        }

        public List<GroupConnector> GetGroupConnectorsByOrganization(int organization, int initiativeId)
        {           
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectorsForOrganization(organization, initiativeId);          
            return MapGroupConnector(mpGroupConnector);
        }

        public List<GroupConnector> GetGroupConnectorsForOpenOrganizations(int initiativeId)
        {
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectorsForOpenOrganizations(initiativeId);
            var anywhereId = _configWrapper.GetConfigIntValue("AnywhereCongregation");
            // filter out Anywhere group connectors because they are special
            mpGroupConnector = mpGroupConnector.Where((gc) => gc.PreferredLaunchSiteId != anywhereId).ToList();
            return MapGroupConnector(mpGroupConnector);
        }

        private static List<GroupConnector> MapGroupConnector(List<MpGroupConnector> mpGroupConnectors)
        {
            var groupConnector = new GroupConnector();
            return mpGroupConnectors != null ? groupConnector.FromMpGroupConnectorList(mpGroupConnectors) : null;
        }

        private static GroupConnector MapGroupConnector(MpGroupConnector mpGroupConnector)
        {
            var groupConnector = new GroupConnector();
            return mpGroupConnector != null ? groupConnector.FromMpGroupConnector(mpGroupConnector) : null;
        }
    }
}