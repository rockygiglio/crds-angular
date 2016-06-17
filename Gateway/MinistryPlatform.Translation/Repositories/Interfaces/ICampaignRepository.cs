using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICampaignRepository
    {
        MpPledgeCampaign GetPledgeCampaign(int campaignId);
    }
}
