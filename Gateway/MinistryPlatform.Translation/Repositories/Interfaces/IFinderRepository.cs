using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Finder;
using System.Device.Location;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IFinderRepository
    {
        FinderPinDto GetPinDetails(int participantId);
        List<SpPinDto> GetPinsInRadius(GeoCoordinate originCoords);
        void EnablePin(int participantId);
    }
}
