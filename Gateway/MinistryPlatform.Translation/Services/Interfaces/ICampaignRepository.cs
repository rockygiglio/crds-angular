using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICampaignRepository
    {
        MpPledgeCampaign GetPledgeCampaign(int campaignId);
    }
}
