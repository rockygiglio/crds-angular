using System.Collections.Generic;
using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IProjectTypeService
    {
        List<ProjectType> GetProjectTypes();
    }
}
