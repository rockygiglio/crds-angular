using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IMedicalInformationRepository
    {
        List<MpMedicalAllergy> GetMedicalAllergyInfo(int contactId);
        int SaveMedicalInfo(MpMedicalInformation mpMedicalInfo, int contactId);
        void UpdateOrCreateMedAllergy(int medicalInformationId, MpAllergy allergy);
    }
}
