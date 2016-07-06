using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IInvitationRepository
    {
        bool CreateInvitation(MpInvitation dto, string token);

    }
}