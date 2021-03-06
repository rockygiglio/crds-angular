using System.Collections.Generic;
using crds_angular.Models.Crossroads.Camp;

namespace crds_angular.Services.Interfaces
{
    public interface ICampService
    {
        CampDTO GetCampEventDetails(int eventId);
        CampReservationDTO SaveCampReservation(CampReservationDTO campReservation, int eventId, string token);
        void SaveCamperEmergencyContactInfo(List<CampEmergencyContactDTO> emergencyContacts, int eventId, int contactId, string token);
        List<MyCampDTO> GetMyCampInfo(string token);
        List<CampWaiverDTO> GetCampWaivers(int eventId, int contactId);
        void SaveWaivers(string token, int eventId, int contactId, List<CampWaiverResponseDTO> waivers);
        CampReservationDTO GetCamperInfo(string token, int eventId, int contactId);   
        List<CampFamilyMember> GetEligibleFamilyMembers(int eventId, string token);
        void SaveCamperMedicalInfo(MedicalInfoDTO medicalInfo, int contactId, string token);
        MedicalInfoDTO GetCampMedicalInfo(int eventId, int contactId, string token);
        List<CampEmergencyContactDTO> GetCamperEmergencyContactInfo(int eventId, int contactId, string token);
        ProductDTO GetCampProductDetails(int eventId, int contactId, string token);
        void SaveInvoice(CampProductDTO campProductDto, string token);
        bool SendCampConfirmationEmail(int eventId, int invoiceId, int paymentId, string token);
        void SetCamperAsRegistered(int eventId, int contactId);
    }
}
