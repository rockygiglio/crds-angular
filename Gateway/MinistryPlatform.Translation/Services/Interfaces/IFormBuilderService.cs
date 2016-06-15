using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IFormBuilderService
    {
        List<Group> GetGroupsUndividedSession(int pageViewId);
    }
}