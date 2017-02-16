﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using AutoMapper;
using crds_angular.Exceptions;
using crds_angular.Models.Finder;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.Finder;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class FinderService : IFinderService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IFinderRepository _finderRepository;
        private readonly IParticipantRepository _participantRepository;

        private class RemoteAddress
        {
            public string Ip { get; set; }
            public string region_code { get; set; }
            public string city { get; set; }
            public string zip_code { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public FinderService(IFinderRepository finderRepository, IParticipantRepository participantRepository)
        {
            _finderRepository = finderRepository;
            _participantRepository = participantRepository;
        }

        public PinDto GetPinDetails(int participantId)
        {
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));

            //TODO get group details
            return pinDetails;
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