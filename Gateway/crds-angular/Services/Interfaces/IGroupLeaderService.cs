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
        IObservable<int> GetGroupLeaderStatus(string token);
        void SetInterested(string token);
        IObservable<int> SetApplied(string token );
        IObservable<int> SaveSpiritualGrowth(SpiritualGrowthDTO spiritualGrowth);
        IObservable<Dictionary<string, object>> GetReferenceData(int contactId); 
        IObservable<int> SendReferenceEmail(Dictionary<string,object> referenceData );
    }
    
}
