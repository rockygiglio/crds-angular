using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICampaignRepository
    {
        MpPledgeCampaign GetPledgeCampaign(int campaignId);
        MpPledgeCampaign GetPledgeCampaign(int campaignId, string token);    
        List<MpTripRecord> GetGoTripDetailsByCampaign(int pledgeCampaignId);
    }
}
