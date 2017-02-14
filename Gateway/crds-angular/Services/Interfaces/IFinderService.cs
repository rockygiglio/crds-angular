using System.Device.Location;
using crds_angular.Models.Finder;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IFinderService
    {
        PinDto GetPinDetails(int participantId);
    }
}