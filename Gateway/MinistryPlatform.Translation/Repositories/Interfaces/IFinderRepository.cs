using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Finder;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IFinderRepository
    {
        FinderPinDto GetPinDetails(int participantId);
        void UpdatePinAddress(int contactId, Dictionary<string, object> addressDictionary, Dictionary<string, object> householdDictionary);
    }
}
