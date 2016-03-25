using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGoVolunteerService
    {
        List<ProjectType> GetProjectTypes(string token);
    }
}
