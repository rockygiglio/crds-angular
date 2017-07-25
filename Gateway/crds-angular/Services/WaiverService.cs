using System;
using System.Reactive.Linq;
using crds_angular.Models.Crossroads.Waivers;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class WaiverService : IWaiverService
    {
        private readonly IWaiverRepository _waiverRepository;

        public WaiverService(IWaiverRepository waiverRepository)
        {
            _waiverRepository = waiverRepository;
        }

        public IObservable<WaiverDTO> GetWaiver(int waiverId)
        {            
            return _waiverRepository.GetWaiver(waiverId).Select<MpWaivers, WaiverDTO>(w => new WaiverDTO
            {
                Accepted = w.Accepted,
                Required = w.Required,
                SigneeContactId = w.SigneeContactId,
                WaiverId = w.WaiverId,
                WaiverName = w.WaiverName,
                WaiverText = w.WaiverText
            });
        }
    }
}