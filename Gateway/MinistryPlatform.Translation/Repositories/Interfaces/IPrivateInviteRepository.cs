using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IPrivateInviteRepository
    {
        MpPrivateInvite Create(int pledgeCampaignId, string emailAddress, string recipientName, string token);
        bool PrivateInviteValid(int pledgeCampaignId, string guid, string emailAddress);
        void MarkAsUsed(int pledgeCampaignId, string inviteGuid);
    }
}