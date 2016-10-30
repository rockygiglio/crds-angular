using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Payments;
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

        public void SaveMedicalInformation(MpMedicalInformation medicalInfo, int contactId)
        {
            var parms = new Dictionary<string, object>
            {
                {"@ContactId", contactId},
                {"@InsuranceCompany", medicalInfo.InsuranceCompany},
                {"@PolicyHolder", medicalInfo.PolicyHolder},
                {"@PhysicianName", medicalInfo.PhysicianName},
                {"@PhysicianPhone", medicalInfo.PhysicianPhone}
            };

            var apiToken = _apiUserRepository.GetToken();
            _ministryPlatformRest.UsingAuthenticationToken(apiToken).PostStoredProc("api_crds_Create_Medical_Info_For_Contacts", parms);
        }
    }
}
