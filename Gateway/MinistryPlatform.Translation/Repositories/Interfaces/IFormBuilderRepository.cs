using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IFormBuilderRepository
    {
        List<MpGroup> GetGroupsUndividedSession(int pageViewId);
    }
}