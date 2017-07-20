using System;
using crds_angular.Models.Crossroads.Waivers;

namespace crds_angular.Services.Interfaces
{
    public interface IWaiverService
    {
        IObservable<WaiverDTO> GetWaiver(int waiverId);
    }
}