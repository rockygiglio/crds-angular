using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace crds_angular.Services
{
    public class GroupConnectorService : IGroupConnectorService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IGroupConnectorService _mpGroupConnectorService;

        public GroupConnectorService(MinistryPlatform.Translation.Services.Interfaces.IGroupConnectorService groupConnectorService)
        {
            _mpGroupConnectorService = groupConnectorService;
        }

        public List<GroupConnector> GetGroupConnectorsByOrganization(int organization, int initiativeId)
        {
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectorsForOrganization(organization, initiativeId);
            return MapGroupConnector(mpGroupConnector);
        }

        public List<GroupConnector> GetGroupConnectorsForOpenOrganizations(int initiativeId)
        {
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectorsForOpenOrganizations(initiativeId);
            return MapGroupConnector(mpGroupConnector);
        }

        private static List<GroupConnector> MapGroupConnector(List<MpGroupConnector> mpGroupConnectors)
        {
            var groupConnector = new GroupConnector();
            return mpGroupConnectors != null ? groupConnector.FromMpGroupConnectorList(mpGroupConnectors) : null;
        }
    }
}