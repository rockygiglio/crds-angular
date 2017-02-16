using System;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Finder;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.Finder;

namespace crds_angular.Services
{
    public class FinderService : MinistryPlatformBaseService, IFinderService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IFinderRepository _finderRepository;

        public FinderService(IFinderRepository finderRepository, IContactRepository contactRepository)
        {
            _finderRepository = finderRepository;
            _contactRepository = contactRepository;
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
            var householdDictionary = getDictionary(pin.Household_ID);
            var addressDictionary = getDictionary(pin.Address);
            addressDictionary.Add("State/Region", addressDictionary["State"]);
            _contactRepository.UpdateHouseholdAddress(pin.Contact_ID, householdDictionary, addressDictionary);
        }

    }
}