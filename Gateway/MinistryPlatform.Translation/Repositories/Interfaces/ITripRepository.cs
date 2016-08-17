namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ITripRepository
    {
        bool AddAsTripParticipant(int ContactId, int PledgeCampaignID, string token);
    }
}