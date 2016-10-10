using crds_angular.Models.Crossroads.Camp;

namespace crds_angular.Services.Interfaces
{
    public interface ICampService
    {
        CampDTO GetCampEventDetails(int eventId);
        void SaveCampReservation(CampReservationDTO campReservation);
    }
}
