using System.Collections.Generic;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupConnectorService
    {
        List<MpGroupConnector> GetGroupConnectorsForOrganization(int organizationId, int initiativeId);
        List<MpGroupConnector> GetGroupConnectorsForOpenOrganizations(int initiativeId);
    }
}