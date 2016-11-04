using System;
using System.Collections.Generic;
using System.Linq;
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

        public MpMedicalInformation GetMedicalInfo(int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var medInfo = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpMedicalInformation>($"Contact_ID = {contactId},MedicalInformation_ID = 33");
            //var medInfo = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpMedicalAllergy>($"Medical_Information_ID_Table_Contact_ID_Table.[Contact_ID] = {contactId}");
            var mpMedicalInformation = medInfo.FirstOrDefault();

            if(mpMedicalInformation == null) { return null; }

            var filter = new Dictionary<string, object> { { "Medical_Information_ID_Table.MedicalInformation_ID", mpMedicalInformation.MedicalInformationId } };

            var allergyIdList = _ministryPlatformRest.UsingAuthenticationToken(apiToken)
               .Get<MpMedicalAllergy>("cr_Medical_Information_Allergies", filter);

            System.Diagnostics.Debug.WriteLine(allergyIdList);


            return null;

        }

        public void UpdateMedicalRecords(MpMedicalInformation mpMedicalInfo, List<MpAllergy> allergyList, int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            if (allergyList.Count <= 0 )
            {
                
            }




        }

        public void CreateMedicalRecords(MpMedicalInformation mpMedicalInfo, List<MpAllergy> allergyList, int contactId)
        {

        }
    }
}
