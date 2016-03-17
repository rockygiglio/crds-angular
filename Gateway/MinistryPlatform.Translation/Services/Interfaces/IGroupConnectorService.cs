using System.Collections.Generic;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupConnectorService
    {
        List<MpGroupConnector> GetGroupConnectors(int organizationId, int initiativeName);
    }
}