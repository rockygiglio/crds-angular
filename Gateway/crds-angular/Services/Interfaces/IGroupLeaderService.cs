using System;
using System.Collections.Generic;
using System.Reactive;
using crds_angular.Models.Crossroads.GroupLeader;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupLeaderService
    {
        IObservable<IList<Unit>> SaveProfile(string token, GroupLeaderProfileDTO leader);
        IObservable<int> SaveReferences(GroupLeaderProfileDTO leader);
        void SetInterested(string token);
        IObservable<int> SetApplied(string token );
        IObservable<int> SaveSpiritualGrowth(SpiritualGrowthDTO spiritualGrowth);
        IObservable<bool> SendReferenceEmail(int contactId);
    }
    
}
