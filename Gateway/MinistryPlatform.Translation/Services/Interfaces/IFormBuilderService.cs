using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IFormBuilderService
    {
        List<Group> GetGroupsUndividedSession(int pageViewId);
    }
}