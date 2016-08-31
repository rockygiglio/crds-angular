using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ITripRepository
    {
        Result<MpPledge> AddAsTripParticipant(int ContactId, int PledgeCampaignID, string token);
    }
}