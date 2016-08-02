using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    class CampaignRepository : BaseRepository, ICampaignRepository
    {
        
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;

        public CampaignRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public MpPledgeCampaign GetPledgeCampaign(int campaignId)
        {
            return WithApiLogin<MpPledgeCampaign>(token =>
            {
                var results = _ministryPlatformService.GetPageViewRecords(_configurationWrapper.GetConfigIntValue("GoTripsWithForms"), token, campaignId.ToString());
                var campaigns = new List<MpPledgeCampaign>();
                foreach (var result in results)
                {
                    var ageExceptions = _ministryPlatformService.GetSubPageRecords(_configurationWrapper.GetConfigIntValue("GoTripAgeExceptions"), campaignId, token);
                    var exceptions = ageExceptions.Select(ae => ae.ToInt("Contact_ID")).ToList();
                    var campaign = new MpPledgeCampaign()
                    {
                        Id = result.ToInt("Pledge_Campaign_ID"),
                        Name = result.ToString("Campaign_Name"),
                        Type = result.ToString("Campaign_Type"),
                        StartDate = result.ToDate("Start_Date"),
                        EndDate = result.ToDate("End_Date"),
                        Goal = result.ToInt("Campaign_Goal"),
                        FormId = result.ToInt("Form_ID"),
                        Nickname = result.ToString("Nickname"),
                        YoungestAgeAllowed = result.ToInt("Youngest_Age_Allowed"),
                        RegistrationEnd = result.ToDate("Registration_End"),
                        RegistrationStart = result.ToDate("Registration_Start"),
                        AgeExceptions = exceptions,
                        EventId = result.ToInt("Event_ID")
                    };
                    campaigns.Add(campaign);
                }
                
                return campaigns.FirstOrDefault();
            });
           
        }

        public List<MpTripRecord> GetGoTripDetailsByCampaign(int pledgeCampaignId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var parms = new Dictionary<string, object> { { "@Pledge_Campaign_ID", pledgeCampaignId } };
            var tripRecords = _ministryPlatformRest.UsingAuthenticationToken(apiToken).GetFromStoredProc<MpTripRecord>(_configurationWrapper.GetConfigValue("TripRecordProc"), parms);
            List<MpTripRecord> tripRecord = tripRecords.FirstOrDefault()?? new List<MpTripRecord>();
            return tripRecord;
        }
    }
}
