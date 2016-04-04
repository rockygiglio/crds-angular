using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;
using IGroupConnectorService = crds_angular.Services.Interfaces.IGroupConnectorService;

namespace crds_angular.Services
{
    public class GroupConnectorService : IGroupConnectorService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IGroupConnectorService _mpGroupConnectorService;
        private readonly IApiUserService _apiUserService;        

        public GroupConnectorService(MinistryPlatform.Translation.Services.Interfaces.IGroupConnectorService groupConnectorService, IApiUserService apiUserService)
        {
            _mpGroupConnectorService = groupConnectorService;
            _apiUserService = apiUserService;            
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
    }
}