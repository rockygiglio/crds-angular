using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IGroupToolRepository
    {
        List<MpInvitation> GetInvitations(int sourceId, int invitationTypeId, string token);
        List<MpInquiry> GetInquiries(int groupId, string token);
    }
}