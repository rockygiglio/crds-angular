using System.Collections.Generic;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IProjectTypeRepository
    {
        List<MpProjectType> GetProjectTypes();
    }
}
