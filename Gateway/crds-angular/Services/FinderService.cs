using System;
using System.Linq;
using AutoMapper;
using crds_angular.Exceptions;
using crds_angular.Models.Finder;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.Finder;

namespace crds_angular.Services
{
    public class FinderService : IFinderService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IFinderRepository _finderRepository;

        public FinderService(IFinderRepository finderRepository)
        {
            _finderRepository = finderRepository;
        }

        public PinDto GetPinDetails(int participantId)
        {
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));
            //then get group details
            return pinDetails;
        }
    }
}