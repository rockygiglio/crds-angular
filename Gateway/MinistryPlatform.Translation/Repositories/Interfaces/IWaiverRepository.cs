using System;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IWaiverRepository
    {
        IObservable<MpWaivers> GetWaiver(int waiverId);
    }
}