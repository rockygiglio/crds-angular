using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupConnectorService
    {
        List<GroupConnector> GetGroupConnectorsByOrganization(int organization, int initiativeId);
    }
}