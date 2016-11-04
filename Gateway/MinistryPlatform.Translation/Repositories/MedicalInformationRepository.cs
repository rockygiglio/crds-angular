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

            //TODO: get rid of try-catch
            try
            {
                var medInfo = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpMedicalInformation>($"Contact_ID = {contactId}");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"\n\n\n***** Medical Info *****\n{e}\n\n\n{e.Message}");
            }


            //var mpMedicalInformation = medInfo.FirstOrDefault();

            //if(mpMedicalInformation == null) { return null; }

            //var allergyIdList = _ministryPlatformRest.UsingAuthenticationToken(apiToken)
            //    .Search<MpMedicalAllergy>($"Medical_Information_ID = {mpMedicalInformation.MedicalInformationId}");

            //System.Diagnostics.Debug.WriteLine(allergyIdList);


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
