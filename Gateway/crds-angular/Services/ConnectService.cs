using System;
using System.Linq;
using AutoMapper;
using crds_angular.Exceptions;
using crds_angular.Models.Connect;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.Connect;

namespace crds_angular.Services
{
    public class ConnectService : IConnectService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IConnectRepository _connectRepository;

        public ConnectService(IConnectRepository connectRepository)
        {
            _connectRepository = connectRepository;
        }

        public PinDto GetPinDetails(int participantId)
        {
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_connectRepository.GetPinDetails(participantId));
            //then get group details
            return pinDetails;
        }
    }
}