using System.Collections.Generic;
using crds_angular.Models.Crossroads.Camp;

namespace crds_angular.Services.Interfaces
{
    public interface ICampService
    {
        CampDTO GetCampEventDetails(int eventId);
        void SaveCampReservation(CampReservationDTO campReservation, int eventId, string token);
        void SaveCamperEmergencyContactInfo(CampEmergencyContactDTO emergencyContact, int eventId, int contactId);
        List<MyCampDTO> GetMyCampInfo(string token);
        List<CampWaiverDTO> GetCampWaivers(int eventId, int contactId);
        void SaveWaivers(string token, int eventId, int contactId, List<CampWaiverResponseDTO> waivers);
        CampReservationDTO GetCamperInfo(string token, int eventId, int contactId);   
        List<CampFamilyMember> GetEligibleFamilyMembers(int eventId, string token);
    }
}
