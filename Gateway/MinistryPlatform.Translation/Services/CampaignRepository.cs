using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    class CampaignRepository : BaseRepository, ICampaignRepository
    {
        private IConfigurationWrapper _configurationWrapper;
        private IMinistryPlatformService _ministryPlatformService;

        public CampaignRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _configurationWrapper = configurationWrapper;
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

        
    }
}
