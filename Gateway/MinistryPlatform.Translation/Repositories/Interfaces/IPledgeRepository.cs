using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IPledgeRepository
    {
        int CreatePledge(int donorId, int pledgeCampaignId, decimal totalPledge);
        bool DonorHasPledge(int pledgeCampaignId, int donorId);
        MpPledge GetPledgeByCampaignAndDonor(int pledgeCampaignId, int donorId);
        MpPledge GetPledgeByCampaignAndContact(int pledgeCampaignId, int contactId);
        List<MpPledge> GetPledgesByCampaign(int pledgeCampaignId, string token);
        int GetDonorForPledge(int pledgeId);
        List<MpPledge> GetPledgesForAuthUser(string userToken, int[] campaignTypeIds = null);
    }
}