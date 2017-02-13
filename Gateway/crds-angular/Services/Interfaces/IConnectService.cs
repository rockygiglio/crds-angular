using System.Device.Location;
using crds_angular.Models.Connect;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IConnectService
    {
        PinDto GetPinDetails(int participantId);
    }
}