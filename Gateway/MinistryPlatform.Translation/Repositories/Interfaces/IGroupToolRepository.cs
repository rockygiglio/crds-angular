using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IGroupToolRepository
    {
        List<MpInvitation> GetInvitees(int SourceId, string token);

    }
}