using System;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IMedicalInformationRepository
    {
        void SaveMedicalInformation(MpMedicalInformation medicalInfo, int contactId);
    }
}
