using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IPrivateInviteService
    {
        PrivateInvite Create(int pledgeCampaignId, string emailAddress, string recipientName, string token);
        void Email();
        bool PrivateInviteValid(int pledgeCampaignId, string guid);
    }
}