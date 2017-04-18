using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICampaignRepository
    {
        MpPledgeCampaign GetPledgeCampaign(int campaignId);
        MpPledgeCampaign GetPledgeCampaign(int campaignId, string token);    
        List<MpTripRecord> GetGoTripDetailsByCampaign(int pledgeCampaignId);
        MpPledgeCampaignSummaryDto GetPledgeCampaignSummary(string token, int pledgeCampaignId);
    }
}
