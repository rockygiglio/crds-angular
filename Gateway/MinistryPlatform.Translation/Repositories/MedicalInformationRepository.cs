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

        public List<MpMedical> GetMedicalAllergyInfo(int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            string columns = "Allergy_ID_Table.[Description],Allergy_ID_Table.[Allergy_ID], " +
                             "Allergy_ID_Table_Allergy_Type_ID_Table.[Allergy_Type],Allergy_ID_Table_Allergy_Type_ID_Table.[Allergy_Type_ID], cr_Medical_Information_Allergies.[Medical_Information_Allergy_ID]";
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken)
                .Search<MpMedical>($"Medical_Information_ID_Table_Contact_ID_Table.Contact_ID={contactId}",columns ).ToList();           
        }

        public MpMedicalInformation GetMedicalInformation(int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var medInfo = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpMedicalInformation>($"Contact_ID_Table.Contact_ID={contactId}", "cr_Medical_Information.[MedicalInformation_ID], cr_Medical_Information.[InsuranceCompany],Contact_ID_Table.[Contact_ID],cr_Medical_Information.[PolicyHolderName],cr_Medical_Information.[PhysicianName] AS[PhysicianName],cr_Medical_Information.[PhysicianPhone]");
            return medInfo.SingleOrDefault();
        }

        public MpMedicalInformation SaveMedicalInfo(MpMedicalInformation mpMedicalInfo, int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var records = new List<MpMedicalInformation> {mpMedicalInfo};
            if (mpMedicalInfo.MedicalInformationId != 0)
            {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put(records);
                return mpMedicalInfo;
            }
            _ministryPlatformRest.UsingAuthenticationToken(apiToken).Post(records);
            return GetMedicalInformation(contactId);
        }

        public void UpdateOrCreateMedAllergy(List<MpMedicalAllergy> updateToAllergyList, List<MpMedicalAllergy> createToAllergyList  )
        {
            var apiToken = _apiUserRepository.GetToken();
            //var records = new List<Dictionary<string, object>>();
            if (updateToAllergyList.Count > 0) {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put(updateToAllergyList);
            }
            if (createToAllergyList.Count > 0) {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Post(createToAllergyList);
            }
        }
    }
}
