using System.Collections.Generic;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IProjectTypeRepository
    {
        List<MpProjectType> GetProjectTypes();
    }
}
