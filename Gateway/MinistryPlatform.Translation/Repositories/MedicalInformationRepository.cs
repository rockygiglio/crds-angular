using System.Collections.Generic;
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

        public void SaveMedicalInformation(MpMedicalAllergy medAllergyInfo, int contactId)
        {
            //var parms = new Dictionary<string, object>
            //{
            //    {"@ContactId", contactId},
            //    {"@InsuranceCompany", medicalInfo.InsuranceCompany},
            //    {"@PolicyHolder", medicalInfo.PolicyHolder},
            //    {"@PhysicianName", medicalInfo.PhysicianName},
            //    {"@PhysicianPhone", medicalInfo.PhysicianPhone},
            //    {"@AllergyType", medicalInfo.AllergyType },
            //    {"@AllergyDescription", medicalInfo.AllergyDescription }
            //};

            //var apiToken = _apiUserRepository.GetToken();
            //_ministryPlatformRest.UsingAuthenticationToken(apiToken).PostStoredProc("api_crds_Create_Medical_Info_For_Contacts", parms);

            var apiToken = _apiUserRepository.GetToken();
            var searchString = $"Contact_ID={contactId}";
            var medicalInformationId = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<int>("cr_Medical_Information", searchString, "MedicalInformation_ID");
            if (medicalInformationId == 0 && medAllergyInfo.Allergy != null)
            {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Post(new List<MpMedicalAllergy> {medAllergyInfo});
            }
            if (medicalInformationId != 0)
            {
                searchString = $"Medical_Information_ID ={medicalInformationId}";
                var allergyIds = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<List<int>>("cr_Medical_Information_Allergy", searchString, "Allergy_ID");
                foreach (var allergyId in allergyIds)
                {
                    
                }


            }
            


        }
    }
}
