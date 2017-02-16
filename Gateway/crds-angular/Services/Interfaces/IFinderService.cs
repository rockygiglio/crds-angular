using crds_angular.Models.Finder;

namespace crds_angular.Services.Interfaces
{
    public interface IFinderService
    {
        PinDto GetPinDetails(int participantId);
        void EnablePin(int participantId);
        void UpdateHouseholdAddress(PinDto pin);
    }
}