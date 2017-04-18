using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GroupLeader;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupLeaderService
    {
        IObservable<IList<Unit>> SaveProfile(string token, GroupLeaderProfileDTO leader);
        IObservable<int> SaveReferences(GroupLeaderProfileDTO leader);
    }
    
}