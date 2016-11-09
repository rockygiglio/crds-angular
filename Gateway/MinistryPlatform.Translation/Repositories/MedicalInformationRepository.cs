using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Repository.Hierarchy;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class MedicalInformationRepository : IMedicalInformationRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;

        public MedicalInformationRepository(IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public List<MpMedicalAllergy> GetMedicalAllergyInfo(int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            string columns = "Medical_Information_ID_Table.MedicalInformation_ID,Medical_Information_ID_Table.InsuranceCompany, " +
                             "Medical_Information_ID_Table.PolicyHolderName,Medical_Information_ID_Table.PhysicianName, " +
                             "Medical_Information_ID_Table.PhysicianPhone, Allergy_ID_Table.[Description],Allergy_ID_Table.[Allergy_ID], " +
                             "Allergy_ID_Table_Allergy_Type_ID_Table.[Allergy_Type],cr_Medical_Information_Allergies.[Medical_Information_Allergy_ID]";
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken)
                .Search<MpMedicalAllergy>($"Medical_Information_ID_Table_Contact_ID_Table.Contact_ID={contactId}",columns ).ToList();           
        }

        public int SaveMedicalInfo(MpMedicalInformation mpMedicalInfo, int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var records = new List<MpMedicalInformation> {mpMedicalInfo};
            if (mpMedicalInfo.MedicalInformationId != 0)
            {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put(records);
                return mpMedicalInfo.MedicalInformationId;
            }
            _ministryPlatformRest.UsingAuthenticationToken(apiToken).Post(records);
            var medInfo = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpMedicalInformation>($"Contact_ID_Table.Contact_ID={contactId}", "MedicalInformation_ID");
            return medInfo[0].MedicalInformationId;
        }

        public void UpdateOrCreateMedAllergy(int medicalInformationId, List<MpMedicalAllergy> updateToAllergyList, List<MpMedicalAllergy> createToAllergyList  )
        {
            var apiToken = _apiUserRepository.GetToken();
            //var records = new List<Dictionary<string, object>>();
            if (updateToAllergyList.Count > 0) {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put(updateToAllergyList);
            }
            if (updateToAllergyList.Count > 0) {
                _ministryPlatformRest.UsingAuthenticationToken(_apiUserRepository.GetToken()).Post(createToAllergyList);
            }
        }
    }
}
