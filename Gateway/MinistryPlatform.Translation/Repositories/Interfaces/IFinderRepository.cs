using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Finder;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IFinderRepository
    {
        FinderPinDto GetPinDetails(int participantId);
    }
}
