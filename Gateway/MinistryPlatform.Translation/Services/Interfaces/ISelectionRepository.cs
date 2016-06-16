using System.Collections.Generic;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ISelectionRepository
    {
        IList<int> GetSelectionRecordIds(string authToken, int selectionId);
    }
}
