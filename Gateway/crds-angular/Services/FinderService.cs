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
using System.Web.Services.Description;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Models.Crossroads.Profile;
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
        private readonly IGroupRepository _groupRepository;
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
                            IGroupRepository groupRepository,
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
            _groupRepository = groupRepository;
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

        public PinDto GetPinDetailsForGroup(int groupId)
        {
            //get the groups Primary contact
            var participantId = GetParticipantIdFromGroup(groupId);
            //get the pin details for the primary contact
            var pin = GetPinDetailsForPerson(participantId);

            var token = _apiUserRepository.GetToken();

            //get group details for the primary pin
            pin.Gathering = _groupService.GetGroupsByTypeOrId(token, participantId, null, groupId, false, false).FirstOrDefault();
            pin.PinType = PinType.GATHERING;
            if (pin.Gathering != null)
            {
                pin.Gathering.Address.AddressLine1 = "";
                pin.Gathering.Address.AddressLine2 = "";
                pin.Address = pin.Gathering.Address;

            }
            return pin;
        }

        public PinDto GetPinDetailsForPerson(int participantId)
        {
            //first get pin details
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));

            //make sure we have a lat/long
            if (pinDetails != null && pinDetails.Address.Latitude != null && pinDetails.Address.Longitude != null)
            {
                _addressService.SetGeoCoordinates(pinDetails.Address);                
                pinDetails.Address = RandomizeLatLong(pinDetails.Address);
            }

            // randomize the location
            pinDetails.Address = RandomizeLatLong(pinDetails.Address);
            pinDetails.Address.AddressLine1 = "";
            pinDetails.Address.AddressLine2 = "";
            pinDetails.PinType = PinType.PERSON;
            return pinDetails;
        }

        public void EnablePin(int participantId)
        {
            _finderRepository.EnablePin(participantId);
        }

        public void UpdateHouseholdAddress(PinDto pin)
        {
            // TODO is this supposed to be gone?? merge conflicts
            // _addressService.SetGeoCoordinates(pin.Address);

            var coordinates = _addressService.GetGeoLocationCascading(pin.Address);
            pin.Address.Latitude = coordinates.Latitude;
            pin.Address.Longitude = coordinates.Longitude;
            var householdDictionary = new Dictionary<string, object> {{"Household_ID", pin.Household_ID}};
            var address = Mapper.Map<MpAddress>(pin.Address);
            var addressDictionary = getDictionary(address);
            addressDictionary.Add("State/Region", addressDictionary["State"]);
            _contactRepository.UpdateHouseholdAddress((int)pin.Contact_ID, householdDictionary, addressDictionary);
        }

        public List<GroupParticipantDTO> GetParticipantsForGroup(int groupId)
        {
            return _groupService.GetGroupParticipantsWithoutAttributes(groupId);
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

        public int GetParticipantIdFromGroup(int groupId)
        {
            var participantId = _groupService.GetPrimaryContactParticipantId(groupId);
            return participantId;
        }

        public List<PinDto> GetPinsInBoundingBox(GeoCoordinate originCoords, string address, AwsBoundingBox boundingBox)
        {
            var cloudReturn = _awsCloudsearchService.SearchConnectAwsCloudsearch("matchall", "_all_fields", _configurationWrapper.GetConfigIntValue("ConnectDefaultNumberOfPins"), originCoords, boundingBox);
            var pins = ConvertFromAwsSearchResponse(cloudReturn);

            foreach (var pin in pins)
            {
                //calculate proximity for all pins to origin
                if (pin.Address.Latitude == null) continue;
                if (pin.Address.Longitude != null) pin.Proximity = GetProximity(originCoords, new GeoCoordinate(pin.Address.Latitude.Value, pin.Address.Longitude.Value));
            }

            return pins;
        }

        private static List<PinDto> ConvertFromAwsSearchResponse(SearchResponse response)
        {
            var pins = new List<PinDto>();

            foreach (var hit in response.Hits.Hit)
            {
                var pin = new PinDto();
                pin.Proximity = hit.Fields.ContainsKey("proximity") ? Convert.ToDecimal(hit.Fields["proximity"].FirstOrDefault()) : (decimal?)null;
                pin.PinType = hit.Fields.ContainsKey("pintype") ? (PinType)Convert.ToInt32(hit.Fields["pintype"].FirstOrDefault()) : PinType.PERSON;
                pin.FirstName = hit.Fields.ContainsKey("firstname") ? hit.Fields["firstname"].FirstOrDefault() : null;
                pin.LastName = hit.Fields.ContainsKey("lastname") ? hit.Fields["lastname"].FirstOrDefault() : null;
                pin.SiteName = hit.Fields.ContainsKey("sitename") ? hit.Fields["sitename"].FirstOrDefault() : null;
                pin.EmailAddress = hit.Fields.ContainsKey("emailaddress") ? hit.Fields["emailaddress"].FirstOrDefault() : null;
                pin.Contact_ID = hit.Fields.ContainsKey("contactid") ? Convert.ToInt32(hit.Fields["contactid"].FirstOrDefault()) : (int?)null;
                pin.Participant_ID = hit.Fields.ContainsKey("participantid") ? Convert.ToInt32(hit.Fields["participantid"].FirstOrDefault()) : (int?)null;
                pin.Host_Status_ID = hit.Fields.ContainsKey("hoststatus") ? Convert.ToInt32(hit.Fields["hoststatus"].FirstOrDefault()) : (int?)null;
                pin.Household_ID = hit.Fields.ContainsKey("householdid") ? Convert.ToInt32(hit.Fields["householdid"].FirstOrDefault()) : (int?)null;
                pin.Address = new AddressDTO
                {
                    AddressID = hit.Fields.ContainsKey("addressid") ? Convert.ToInt32(hit.Fields["addressid"].FirstOrDefault()) : (int?)null,
                    City = hit.Fields.ContainsKey("city") ? hit.Fields["city"].FirstOrDefault() : null,
                    State = hit.Fields.ContainsKey("state") ? hit.Fields["state"].FirstOrDefault() : null,
                    PostalCode = hit.Fields.ContainsKey("zip") ? hit.Fields["zip"].FirstOrDefault() : null,
                    Latitude = hit.Fields.ContainsKey("latitude") ? Convert.ToDouble(hit.Fields["latitude"].FirstOrDefault()) : (double?)null,
                    Longitude = hit.Fields.ContainsKey("longitude") ? Convert.ToDouble(hit.Fields["longitude"].FirstOrDefault()) : (double?)null,
                };
                if (hit.Fields.ContainsKey("latlong"))
                {
                    var locationstring = hit.Fields["latlong"].FirstOrDefault() ?? "";
                    var coordinates = locationstring.Split(',');
                    pin.Address.Latitude = Convert.ToDouble(coordinates[0]);
                    pin.Address.Longitude = Convert.ToDouble(coordinates[1]);
                }
                if (pin.PinType == PinType.GATHERING)
                {
                    pin.Gathering = new GroupDTO
                    {
                        GroupId = hit.Fields.ContainsKey("groupid") ? Convert.ToInt32(hit.Fields["groupid"].FirstOrDefault()) : 0,
                        GroupName = hit.Fields.ContainsKey("groupname") ? hit.Fields["groupname"].FirstOrDefault() : null,
                        GroupDescription = hit.Fields.ContainsKey("groupdescription") ? hit.Fields["groupdescription"].FirstOrDefault() : null,
                        PrimaryContactEmail = hit.Fields.ContainsKey("primarycontactemail") ? hit.Fields["primarycontactemail"].FirstOrDefault() : null,
                        ContactId = hit.Fields.ContainsKey("countactid") ? Convert.ToInt32(hit.Fields["countactid"].FirstOrDefault()) : 0,
                        Address = pin.Address
                    };
                }
                pins.Add(pin);
            }

            return pins;
        }

        private static decimal GetProximity(GeoCoordinate originCoords, GeoCoordinate pinCoords)
        {
            return (decimal)Proximity(originCoords.Latitude, originCoords.Longitude, pinCoords.Latitude, pinCoords.Longitude);
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


        public List<PinDto> GetMyPins(string token, GeoCoordinate originCoords, int contactId)
        {
            var pins = new List<PinDto>();
            var participantId = GetParticipantIdFromContact(contactId);

            List<PinDto> groupPins = GetMyGroupPins(token, new int[] { _anywhereGroupType }, participantId);
            PinDto personPin = GetPinDetailsForPerson(participantId);

            pins.AddRange(groupPins);
            if (personPin != null && personPin.ShowOnMap)
            {
                pins.Add(personPin);
            }            

            foreach (var pin in pins)
            {
                //calculate proximity for all pins to origin
                if (pin.Address.Latitude == null) continue;
                if (pin.Address.Longitude != null) pin.Proximity = GetProximity(originCoords, new GeoCoordinate(pin.Address.Latitude.Value, pin.Address.Longitude.Value));
            }

            return pins;
        }

        public List<PinDto> GetMyGroupPins(string token, int[] groupTypeIds, int participantId)
        {
            var groupsByType = _groupRepository.GetGroupsForParticipantByTypeOrID(participantId, null, groupTypeIds);

            if (groupsByType == null)
            {
                return null;
            }

            var groupDTOs = groupsByType.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();

            // TODO pull this out - 2x in class - just return what comes out of private method            
            var pins = new List<PinDto>();

            foreach (var group in groupDTOs)
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

        public GeoCoordinate GetGeoCoordsFromLatLong(string lat, string lng)
        {
            double latitude = Convert.ToDouble(lat.Replace("$", "."));
            double longitude = Convert.ToDouble(lng.Replace("$", "."));

            return new GeoCoordinate(latitude, longitude);
        }

        private List<PinDto> GetGroupPinsinRadius(GeoCoordinate originCoords, string address)
        {
            // ignoring originCoords at this time
            var pins = new List<PinDto>();

            // get group for anywhere gathering
            var anywhereGroupTypeId = _configurationWrapper.GetConfigIntValue("AnywhereGroupTypeId");
            var groups = _groupToolService.SearchGroups(new int[] { anywhereGroupTypeId }, null, address, null, originCoords);

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

            var distanceNorth = Math.Sin(angle) * distance;
            var distanceEast = Math.Cos(angle) * distance;

            var newLat = (double)(address.Latitude + (distanceNorth / earthRadius) * 180 / Math.PI);
            var newLong = (double)(address.Longitude + (distanceEast / (earthRadius * Math.Cos(newLat * 180 / Math.PI))) * 180 / Math.PI);
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

        public AddressDTO GetGroupAddress(string token, int groupId)
        {
            var user = _participantRepository.GetParticipantRecord(token);
            var group = _groupService.GetGroupsByTypeOrId(token, user.ParticipantId, null, groupId, true, false).FirstOrDefault();

            if (user.ContactId == group.ContactId || group.Participants.Any(p => p.ParticipantId == user.ParticipantId))
            {
                return group.Address;
            }
            else
            {
                throw new Exception("User does not have acces to requested address");
            }
        }

        public AddressDTO GetPersonAddress(string token, int participantId)
        {
            var user = _participantRepository.GetParticipantRecord(token);

            if (user.ParticipantId == participantId)
            {
                var address = _finderRepository.GetPinAddress(participantId);

                if (address != null)
                {
                    return Mapper.Map<AddressDTO>(address);
                }
                else
                {
                    throw new Exception("User address not found");
                }
            }
            else
            {
                throw new Exception("User does not have acces to requested address");
            }

        }
    }
}