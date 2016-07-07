using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IGroupToolRepository
    {
        List<MpInvitation> GetInvitations(int SourceId, int InvitationTypeId, string token);

    }
}