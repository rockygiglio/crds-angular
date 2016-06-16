using System.Collections.Generic;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Services.Interfaces.GoCincinnati
{
    public interface IGroupConnectorRepository
    {
        int CreateGroupConnector(int registrationId, bool privateGroup);
        int CreateGroupConnectorRegistration(int groupConnectorId, int registrationId);
        MpGroupConnector GetGroupConnectorById(int groupConnectorId);
        List<MpGroupConnector> GetGroupConnectorsForOpenOrganizations(int initiativeId);
        List<MpGroupConnector> GetGroupConnectorsForOrganization(int organizationId, int initiativeId);
    }
}