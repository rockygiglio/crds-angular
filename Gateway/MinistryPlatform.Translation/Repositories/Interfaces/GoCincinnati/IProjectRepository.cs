using System.Collections.Generic;
using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati
{
    public interface IProjectRepository
    {
        Result<MpProject> GetProject(int projectId, string token);
        Result<MpGroupConnector> GetGroupConnector(int projectId, string token);
        List<MpProject> GetProjectsByInitiative(int initiativeId, string token);
    }
}