using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;
using IGroupConnectorService = crds_angular.Services.Interfaces.IGroupConnectorService;

namespace crds_angular.Services
{
    public class GroupConnectorService : IGroupConnectorService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.GoCincinnati.IGroupConnectorService _mpGroupConnectorService;
        private readonly IApiUserService _apiUserService;

        public GroupConnectorService(MinistryPlatform.Translation.Services.Interfaces.GoCincinnati.IGroupConnectorService groupConnectorService, IApiUserService apiUserService)
        {
            _mpGroupConnectorService = groupConnectorService;
            _apiUserService = apiUserService;            
        }

        public GroupConnector GetGroupConnectorById(int groupConnectorId)
        {
            var groupConnector= _mpGroupConnectorService.GetGroupConnectorById(groupConnectorId);
            return MapGroupConnector(groupConnector);
        }

        public List<GroupConnector> GetGroupConnectorsByOrganization(int organization, int initiativeId)
        {
            var token = _apiUserService.GetToken();
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectorsForOrganization(organization, initiativeId, token);
            return MapGroupConnector(mpGroupConnector);
        }

        public List<GroupConnector> GetGroupConnectorsForOpenOrganizations(int initiativeId)
        {
            var token = _apiUserService.GetToken();
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectorsForOpenOrganizations(initiativeId, token);
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