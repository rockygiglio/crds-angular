using System;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GroupLeader;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupLeaderService
    {
        Task SaveProfile(string token, GroupLeaderProfileDTO leader);
        IObservable<int> SaveSpiritualGrowth(string token, SpiritualGrowthDTO spiritualGrowth);
    }
    
}