using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Finder;
using System.Device.Location;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IFinderRepository
    {
        FinderPinDto GetPinDetails(int participantId);
        List<SpPinDto> GetPinsInRadius(GeoCoordinate originCoords);
        void EnablePin(int participantId);
        List<MpConnectAws> GetAllPinsForAws();
        MpAddress GetPinAddress(int participantId);
        FinderGatheringDto UpdateGathering(FinderGatheringDto finderGathering);
        void RecordConnection(MpConnectCommunication connection);
    }
}
