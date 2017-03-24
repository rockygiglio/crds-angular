using System;
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
using Crossroads.Web.Common.MinistryPlatform;
using System.Linq;
using System.Device.Location;
using crds_angular.Models.Crossroads.Groups;
using MinistryPlatform.Translation.Models.Finder;
using Crossroads.Web.Common.Configuration;

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
        private readonly IAddressGeocodingService _addressGeocodingService;
        private readonly IContactRepository _contactRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddressService));
        private readonly IFinderRepository _finderRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IAddressService _addressService;
        private readonly IGroupToolService _groupToolService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IGroupService _groupService;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IInvitationService _invitationService;
        private readonly IAwsCloudsearchService _awsCloudsearchService;
        private readonly int _approvedHost;
        private readonly int _anywhereGroupType;
        private readonly int _leaderRoleId;
        private readonly int _memberRoleId;
        private readonly int _anywhereGatheringInvitationTypeId;

        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        public FinderService(
                            IAddressGeocodingService addressGeocodingService, 
                            IFinderRepository finderRepository,
                            IContactRepository contactRepository, 
                            IAddressService addressService, 
                            IParticipantRepository participantRepository, 
                            IGroupService groupService,
                            IGroupToolService groupToolService,
                            IApiUserRepository apiUserRepository,
                            IConfigurationWrapper configurationWrapper,
                            IInvitationService invitationService,
                            IAwsCloudsearchService awsCloudsearchService
                            )
        {
            _addressGeocodingService = addressGeocodingService;
            _finderRepository = finderRepository;
            _contactRepository = contactRepository;
            _addressService = addressService;
            _participantRepository = participantRepository;
            _groupService = groupService;
            _apiUserRepository = apiUserRepository;
            _approvedHost = configurationWrapper.GetConfigIntValue("ApprovedHostStatus");
            _anywhereGroupType = configurationWrapper.GetConfigIntValue("AnywhereGroupTypeId");
            _leaderRoleId = configurationWrapper.GetConfigIntValue("GroupRoleLeader");
            _memberRoleId = configurationWrapper.GetConfigIntValue("Group_Role_Default_ID");
            _anywhereGatheringInvitationTypeId = configurationWrapper.GetConfigIntValue("AnywhereGatheringInvitationType");
            _groupToolService = groupToolService;
            _configurationWrapper = configurationWrapper;
            _invitationService = invitationService;
            _awsCloudsearchService = awsCloudsearchService;
        }


        public PinDto GetPinDetails(int participantId)
        {
            var token = _apiUserRepository.GetToken();
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));

            //get group details
            if (pinDetails.Host_Status_ID == _approvedHost)
            {
                var groups = _groupService.GetGroupsByTypeOrId(token, participantId, new int[] {_anywhereGroupType});
                foreach (GroupDTO group in groups)
                {
                    var leader = group.Participants.Where(p => p.GroupRoleId == _leaderRoleId && p.ParticipantId == participantId).FirstOrDefault();

                    if (leader != null)
                    {
                        pinDetails.Gathering = group;
                        break;
                    }
                }
            }
            
            //make sure we have a lat/long
            if (pinDetails.Address.Latitude == null || pinDetails.Address.Longitude == null)
            {
                _addressService.SetGeoCoordinates(pinDetails.Address);
            }
            // randomize the location
                pinDetails.Address = RandomizeLatLong(pinDetails.Address);
            
            //TODO get group details
            return pinDetails;
        }

        public void EnablePin(int participantId)
        {
            _finderRepository.EnablePin(participantId);
        }

        public void UpdateHouseholdAddress(PinDto pin)
        {
            if (pin.isFormDirty || (!pin.isFormDirty && !pin.Address.HasGeoCoordinates()))
            {
                _addressService.SetGeoCoordinates(pin.Address);
            }

            var householdDictionary = new Dictionary<string, object> {{"Household_ID", pin.Household_ID}};
            var address = Mapper.Map<MpAddress>(pin.Address);
            var addressDictionary = getDictionary(address);
            addressDictionary.Add("State/Region", addressDictionary["State"]);
            _contactRepository.UpdateHouseholdAddress((int) pin.Contact_ID, householdDictionary, addressDictionary);
        }

        public void GatheringJoinRequest(string token, int gatheringId)
        {
            _groupToolService.SubmitInquiry(token, gatheringId);
        }

        public AddressDTO GetAddressForIp(string ip)
        {
            var address = new AddressDTO();
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

        public List<PinDto> GetPinsInRadius(GeoCoordinate originCoords, string address)
        {
            var pins = new List<PinDto>();

            List<PinDto> groupPins = GetGroupPinsinRadius(originCoords, address);
            List<PinDto> participantAndBuildingPins = GetParticipantAndBuildingPinsInRadius(originCoords);

            pins.AddRange(participantAndBuildingPins);
            pins.AddRange(groupPins);

            foreach (var pin in pins)
            {
                //calculate proximity for all pins to origin
                if (pin.Address.Latitude == null) continue;
                if (pin.Address.Longitude != null) pin.Proximity = GetProximity(originCoords, new GeoCoordinate(pin.Address.Latitude.Value, pin.Address.Longitude.Value));
            }

            return pins;
        }

        private static decimal GetProximity(GeoCoordinate originCoords, GeoCoordinate pinCoords)
        {
            return (decimal) Proximity(originCoords.Latitude, originCoords.Longitude, pinCoords.Latitude,pinCoords.Longitude);
        }

        private static double Proximity(double lat1, double lon1, double lat2, double lon2)
        {
            var theta = lon1 - lon2;
            var dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
           
            return (dist);
        }

        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private List<PinDto> GetParticipantAndBuildingPinsInRadius(GeoCoordinate originCoords)
        {
            List<SpPinDto> participantPinsFromSp = _finderRepository.GetPinsInRadius(originCoords);
            List<PinDto> participantAndBuildingPins = new List<PinDto>();

            foreach (var piFromSP in participantPinsFromSp)
            {
                var pin = Mapper.Map<PinDto>(piFromSP);
                participantAndBuildingPins.Add(pin);
            }

            return participantAndBuildingPins;
        }


        public GeoCoordinate GetGeoCoordsFromAddressOrLatLang(string address, string lat, string lng)
        {
            double latitude = Convert.ToDouble(lat.Replace("$", "."));
            double longitude = Convert.ToDouble(lng.Replace("$", "."));

            bool geoCoordsPassedIn = latitude != 0 && longitude != 0;

            GeoCoordinate originCoordsFromGoogle = geoCoordsPassedIn ? null : _addressGeocodingService.GetGeoCoordinates(address);

            GeoCoordinate originCoordsFromClient = new GeoCoordinate(latitude, longitude);

            GeoCoordinate originCoordinates = geoCoordsPassedIn ? originCoordsFromClient : originCoordsFromGoogle;

            return originCoordinates;
        }

        private List<PinDto> GetGroupPinsinRadius(GeoCoordinate originCoords, string address)
        {
            // ignoring originCoords at this time
            var pins = new List<PinDto>();

            // get group for anywhere gathering
            var anywhereGroupTypeId = _configurationWrapper.GetConfigIntValue("AnywhereGroupTypeId");
            var groups = _groupToolService.SearchGroups(new int[] {anywhereGroupTypeId}, null, address, null, originCoords);

            foreach (var group in groups)
            {
                var pin = Mapper.Map<PinDto>(group);
                pin.Gathering = group;
                if (pin.Contact_ID != null)
                {
                    var contact = _contactRepository.GetContactById((int)pin.Contact_ID);
                    pin.FirstName = contact.First_Name;
                    pin.LastName = contact.Last_Name;
                }
               
                pins.Add(pin);
            }

            return pins;
        }

        public AddressDTO RandomizeLatLong(AddressDTO address)
        {
            if (!address.HasGeoCoordinates()) return address;
            var distance = _random.Next(75, 300); // up to a quarter mile
            var angle = _random.Next(0, 359);
            const int earthRadius = 6371000; // in meters

            var distanceNorth = Math.Sin(angle)*distance;
            var distanceEast = Math.Cos(angle)*distance;

            var newLat = (double) (address.Latitude + (distanceNorth/earthRadius)*180/Math.PI);
            var newLong = (double) (address.Longitude + (distanceEast/(earthRadius*Math.Cos(newLat*180/Math.PI)))*180/Math.PI);
            address.Latitude = newLat;
            address.Longitude = newLong;

            return address;
        }


        public Invitation InviteToGathering(string token, int gatheringId, User person)
        {
            var invitation = new Invitation();

            invitation.RecipientName = person.firstName;
            invitation.EmailAddress = person.email;
            invitation.SourceId = gatheringId;
            invitation.GroupRoleId = _memberRoleId;
            invitation.InvitationType = _anywhereGatheringInvitationTypeId;
            invitation.RequestDate = DateTime.Now;

            _invitationService.ValidateInvitation(invitation, token);
            return _invitationService.CreateInvitation(invitation, token);
        }
    }
}