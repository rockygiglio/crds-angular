using System;
using crds_angular.Models.Crossroads.GroupLeader;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupLeaderService
    {
        void SaveProfile(string token, GroupLeaderProfileDTO leader);
    }
    
}