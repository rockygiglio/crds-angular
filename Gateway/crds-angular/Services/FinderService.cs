using System.Net;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using crds_angular.Models.Finder;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using Newtonsoft.Json;

namespace crds_angular.Services
{

    public class RemoteAddress
    {
        public string Ip { get; set; }
        public string region_code { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class FinderService : MinistryPlatformBaseService, IFinderService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IFinderRepository _finderRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IAddressService _addressService;


        public FinderService(IFinderRepository finderRepository, IContactRepository contactRepository, IAddressService addressService, IParticipantRepository participantRepository)
        {
            _finderRepository = finderRepository;
            _contactRepository = contactRepository;
            _addressService = addressService;
            _participantRepository = participantRepository;
        }


        public PinDto GetPinDetails(int participantId)
        {
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));

            //make sure we have a lat/long
            if (pinDetails.Address.Latitude == null || pinDetails.Address.Longitude == null)
            {
                _addressService.SetGeoCoordinates(pinDetails.Address);
            }

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



        public AddressDTO GetAddressForIp()
        {
            var address = new AddressDTO();
            var ip = _finderRepository.GetIpForRemoteUser();
            var request = WebRequest.Create("http://freegeoip.net/json/" + ip);
            using (var response = request.GetResponse())
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var responseString = stream.ReadToEnd();
                var s = JsonConvert.DeserializeObject<RemoteAddress>(responseString);
                address.City = s.city;
                address.State = s.region_code;
                address.PostalCode = s.zip_code;
                address.Latitude = s.latitude;
                address.Longitude = s.longitude;
            }
            return address;
        }

        public int GetParticipantIdFromContact(int contactId)
        {
            var participant = _participantRepository.GetParticipant(contactId);
            return participant.ParticipantId;
        }
    }
}