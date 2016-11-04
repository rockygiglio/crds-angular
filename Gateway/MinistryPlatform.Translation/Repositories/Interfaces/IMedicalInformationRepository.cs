using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IMedicalInformationRepository
    {
        void UpdateMedicalRecords(MpMedicalInformation mpMedicalInfo, List<MpAllergy> allergyList, int contactId);
        void CreateMedicalRecords(MpMedicalInformation mpMedicalInfo, List<MpAllergy> allergyList, int contactId);
        MpMedicalInformation GetMedicalInfo(int contactId);
    }
}
