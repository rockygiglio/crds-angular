using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IInvitationRepository
    {
        IObservable<MpInvitation> CreateInvitationAsync(MpInvitation invite);
        MpInvitation CreateInvitation(MpInvitation dto);
        MpInvitation GetOpenInvitation(string invitationGuid);
        void MarkInvitationAsUsed(string invitationGuid);
    }
}