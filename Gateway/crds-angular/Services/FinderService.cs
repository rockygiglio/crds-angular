using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Finder;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.Finder;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services
{
    public class FinderService : MinistryPlatformBaseService, IFinderService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IFinderRepository _finderRepository;
        private readonly IAddressService _addressService;

        public FinderService(IFinderRepository finderRepository, IContactRepository contactRepository, IAddressService addressService)
        {
            _finderRepository = finderRepository;
            _contactRepository = contactRepository;
            _addressService = addressService;
        }

        public PinDto GetPinDetails(int participantId)
        {
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));

            //TODO get group details
            return pinDetails;
        }

        public void EnablePin(int participantId)
        {
            _finderRepository.EnablePin(participantId);
        }

        public void UpdateHouseholdAddress(PinDto pin)
        {
            _addressService.SetGeoCoordinates(pin.Address);
          
            var householdDictionary = new Dictionary<string, object> { { "Household_ID", pin.Household_ID } };
            var address = Mapper.Map<MpAddress>(pin.Address);
            var addressDictionary = getDictionary(address);
            addressDictionary.Add("State/Region", addressDictionary["State"]);
            _contactRepository.UpdateHouseholdAddress(pin.Contact_ID, householdDictionary, addressDictionary);
        }

    }
}