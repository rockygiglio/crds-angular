using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IMedicalInformationRepository
    {
        List<MpMedical> GetMedicalAllergyInfo(int contactId);
        MpMedicalInformation GetMedicalInformation(int contactId);
        MpMedicalInformation SaveMedicalInfo(MpMedicalInformation mpMedicalInfo, int contactId);
        void UpdateOrCreateMedAllergy(List<MpMedicalAllergy> updateToAllergyList, List<MpMedicalAllergy> createToAllergyList );
    }
}
