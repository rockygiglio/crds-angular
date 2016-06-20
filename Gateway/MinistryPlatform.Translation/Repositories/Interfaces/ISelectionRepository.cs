using System.Collections.Generic;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ISelectionRepository
    {
        IList<int> GetSelectionRecordIds(string authToken, int selectionId);
    }
}
