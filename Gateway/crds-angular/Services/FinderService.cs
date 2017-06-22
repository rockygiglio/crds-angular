﻿using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using crds_angular.Models.Finder;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Repositories.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using Newtonsoft.Json;
using Crossroads.Web.Common.MinistryPlatform;
using System.Linq;
using System.Device.Location;
using System.Text;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Exceptions;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Models.Crossroads.Groups;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models.Finder;
using MpCommunication = MinistryPlatform.Translation.Models.MpCommunication;

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
        private readonly ILog _logger = LogManager.GetLogger(typeof(FinderService));
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
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ICommunicationRepository _communicationRepository;
        private readonly IAccountService _accountService;
        private readonly int _approvedHost;
        private readonly int _pendingHost;
        private readonly int _anywhereGroupType;
        private readonly int _leaderRoleId;
        private readonly int _memberRoleId;
        private readonly int _anywhereGatheringInvitationTypeId;
        private readonly int _groupInvitationTypeId;
        private readonly int _domainId;
        private readonly string _finderConnect;
        private readonly string _finderGroupTool;
        private readonly int _inviteAcceptedTemplateId;
        private readonly int _inviteDeclinedTemplateId;
        private readonly int _anywhereCongregationId;
        private readonly int _spritualGrowthMinistryId;
        private readonly string _connectPersonPinUrl;
        private readonly string _connectSitePinUrl;
        private readonly string _connectGatheringPinUrl;
        private readonly string _connectSmallGroupPinUrl;
        private readonly int _smallGroupType;
        private readonly int _connectCommunicationTypeInviteToGathering;
        private readonly int _connectCommunicationTypeInviteToSmallGroup;
        private readonly int _connectCommunicationTypeRequestToJoinSmallGroup;
        private readonly int _connectCommunicationTypeRequestToJoinGathering;

        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private const double MinutesInDegree = 60;
        private const double StatuteMilesInNauticalMile = 1.1515;

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
            IAwsCloudsearchService awsCloudsearchService,
            IAuthenticationRepository authenticationRepository,
            ICommunicationRepository communicationRepository,
            IAccountService accountService
        )
        {
            // services
            _addressGeocodingService = addressGeocodingService;
            _finderRepository = finderRepository;
            _contactRepository = contactRepository;
            _addressService = addressService;
            _participantRepository = participantRepository;
            _groupService = groupService;
            _groupRepository = groupRepository;
            _apiUserRepository = apiUserRepository;
            _authenticationRepository = authenticationRepository;
            _groupToolService = groupToolService;
            _configurationWrapper = configurationWrapper;
            _invitationService = invitationService;
            _awsCloudsearchService = awsCloudsearchService;
            _communicationRepository = communicationRepository;
            _accountService = accountService;
            // constants
            _anywhereCongregationId = _configurationWrapper.GetConfigIntValue("AnywhereCongregationId");
            _approvedHost = configurationWrapper.GetConfigIntValue("ApprovedHostStatus");
            _pendingHost = configurationWrapper.GetConfigIntValue("PendingHostStatus");
            _anywhereGroupType = configurationWrapper.GetConfigIntValue("AnywhereGroupTypeId");
            _leaderRoleId = configurationWrapper.GetConfigIntValue("GroupRoleLeader");
            _memberRoleId = configurationWrapper.GetConfigIntValue("Group_Role_Default_ID");
            _memberRoleId = configurationWrapper.GetConfigIntValue("Group_Role_Default_ID");
            _anywhereGatheringInvitationTypeId = configurationWrapper.GetConfigIntValue("AnywhereGatheringInvitationType");
            _groupInvitationTypeId = configurationWrapper.GetConfigIntValue("GroupInvitationType");
            _domainId = configurationWrapper.GetConfigIntValue("DomainId");
            _finderConnect = configurationWrapper.GetConfigValue("FinderConnectFlag");
            _finderGroupTool = configurationWrapper.GetConfigValue("FinderGroupToolFlag");
            _inviteAcceptedTemplateId = configurationWrapper.GetConfigIntValue("AnywhereGatheringInvitationAcceptedTemplateId");
            _inviteDeclinedTemplateId = configurationWrapper.GetConfigIntValue("AnywhereGatheringInvitationDeclinedTemplateId");
            _domainId = configurationWrapper.GetConfigIntValue("DomainId");
            _spritualGrowthMinistryId = _configurationWrapper.GetConfigIntValue("SpiritualGrowthMinistryId");
            _connectPersonPinUrl = _configurationWrapper.GetConfigValue("ConnectPersonPinUrl");
            _connectSitePinUrl = _configurationWrapper.GetConfigValue("ConnectSitePinUrl");
            _connectGatheringPinUrl = _configurationWrapper.GetConfigValue("ConnectGatheringPinUrl");
            _connectSmallGroupPinUrl = _configurationWrapper.GetConfigValue("ConnectSmallGroupPinUrl");
            _smallGroupType = _configurationWrapper.GetConfigIntValue("SmallGroupTypeId");
            _connectCommunicationTypeInviteToGathering = _configurationWrapper.GetConfigIntValue("ConnectCommunicationTypeInviteToGathering");
            _connectCommunicationTypeInviteToSmallGroup = _configurationWrapper.GetConfigIntValue("ConnectCommunicationTypeInviteToSmallGroup");
        }

        public PinDto GetPinDetailsForGroup(int groupId)
        {
            List<PinDto> pins = null;
            var cloudReturn = _awsCloudsearchService.SearchByGroupId(groupId.ToString());

            pins = ConvertFromAwsSearchResponse(cloudReturn);
            // need originCoords to calculate proximity, but we do not need here
            GeoCoordinate originCoords = null;
            this.AddPinMetaData(pins, originCoords);

            return pins.First();

        }

        public PinDto GetPinDetailsForPerson(int participantId)
        {
            var pinDetails = Mapper.Map<PinDto>(_finderRepository.GetPinDetails(participantId));

            //make sure we have a lat/long
            if (pinDetails != null && pinDetails.Address.Latitude != null && pinDetails.Address.Longitude != null)
            {
                _addressService.SetGeoCoordinates(pinDetails.Address);
                pinDetails.Address.AddressLine1 = "";
                pinDetails.Address.AddressLine2 = "";
            }

            pinDetails.PinType = PinType.PERSON;
            return pinDetails;
        }

        public void EnablePin(int participantId)
        {
            _finderRepository.EnablePin(participantId);
        }

        public void DisablePin(int participantId)
        {
            _finderRepository.DisablePin(participantId);
        }

        public PinDto UpdateGathering(PinDto pin)
        {
            // Update coordinates
            var coordinates = _addressService.GetGeoLocationCascading(pin.Gathering.Address);

            pin.Gathering.Address.Latitude = coordinates.Latitude;
            pin.Gathering.Address.Longitude = coordinates.Longitude;
            pin.Gathering.GroupTypeId = _anywhereGroupType;

            if (pin.ShouldUpdateHomeAddress)
            {
                var pinAddressId = pin.Address.AddressID;
                pin.Address = new AddressDTO
                {
                    AddressID = pinAddressId,
                    AddressLine1 = pin.Gathering.Address.AddressLine1,
                    AddressLine2 = pin.Gathering.Address.AddressLine2,
                    City = pin.Gathering.Address.City,
                    County = pin.Gathering.Address.County,
                    ForeignCountry = pin.Gathering.Address.ForeignCountry,
                    Latitude = pin.Gathering.Address.Latitude,
                    Longitude = pin.Gathering.Address.Longitude,
                    PostalCode = pin.Gathering.Address.PostalCode,
                    State = pin.Gathering.Address.State
                };
                this.UpdateHouseholdAddress(pin);
                pin.PinType = PinType.PERSON;
                _awsCloudsearchService.UploadNewPinToAws(pin);
                pin.PinType = PinType.GATHERING;
            }

            var gathering = Mapper.Map<FinderGatheringDto>(pin.Gathering);

            _finderRepository.UpdateGathering(gathering);

            return pin;
        }

        public void UpdateHouseholdAddress(PinDto pin)
        {
            var coords = _addressGeocodingService.GetGeoCoordinates(pin.Address);
            pin.Address.Longitude = coords.Longitude;
            pin.Address.Latitude = coords.Latitude;

            var householdDictionary = (pin.Address.AddressID == null)
                ? new Dictionary<string, object> {{"Household_ID", pin.Household_ID}}
                : null;
            var address = Mapper.Map<MpAddress>(pin.Address);

            var addressDictionary = getDictionary(address);
            addressDictionary.Add("State/Region", addressDictionary["State"]);
            _contactRepository.UpdateHouseholdAddress((int) pin.Contact_ID, householdDictionary, addressDictionary);
        }

        public List<GroupParticipantDTO> GetParticipantsForGroup(int groupId)
        {
            return _groupService.GetGroupParticipantsWithoutAttributes(groupId);
        }

        private void GatheringValidityCheck(int contactId, AddressDTO address)
        {
            //get the list of anywhere groups

            var groups = _groupRepository.GetGroupsByGroupType(_anywhereGroupType);

            //get groups that this user is the primary contact for at this address
            var matchingGroupsCount = groups.Where(x => x.PrimaryContact == contactId.ToString())
                .Where(x => x.Address.Address_Line_1 == address.AddressLine1)
                .Where(x => x.Address.City == address.City)
                .Where(x => x.Address.State == address.State).Count();

            if (matchingGroupsCount > 0)
            {
                throw new GatheringException(contactId);
            }
        }

        public void RequestToBeHost(string token, HostRequestDto hostRequest)
        {
            //check if they are already a host at this address. If they are then throw
            GatheringValidityCheck(hostRequest.ContactId, hostRequest.Address);

            // get contact data
            var contact = _contactRepository.GetContactById(hostRequest.ContactId);
            var participant = _participantRepository.GetParticipant(hostRequest.ContactId);

            if (participant.HostStatus != _approvedHost)
            {
                participant.HostStatus = _pendingHost;
                _participantRepository.UpdateParticipantHostStatus(participant);
            }

            //update mobile phone number on contact record
            contact.Mobile_Phone = hostRequest.ContactNumber;
            var updateToDictionary = new Dictionary<string, object>
            {
                {"Contact_ID", hostRequest.ContactId},
                {"Mobile_Phone", hostRequest.ContactNumber},
                {"First_Name", contact.First_Name}
            };
            _contactRepository.UpdateContact(hostRequest.ContactId, updateToDictionary);

            // create the address for the group
            var hostAddressId = _addressService.CreateAddress(hostRequest.Address);
            hostRequest.Address.AddressID = hostAddressId;

            // create the group
            var group = new GroupDTO();
            group.GroupName = contact.Nickname + " " + contact.Last_Name[0];
            group.GroupDescription = hostRequest.GroupDescription;
            group.ContactId = hostRequest.ContactId;
            group.PrimaryContactEmail = contact.Email_Address;
            group.PrimaryContactName = contact.Nickname + " " + contact.Last_Name;
            group.Address = hostRequest.Address;
            group.StartDate = DateTime.Now;
            group.AvailableOnline = false;
            group.GroupFullInd = false;
            group.ChildCareAvailable = false;
            group.CongregationId = _anywhereCongregationId;
            group.GroupTypeId = _anywhereGroupType;
            group.MinistryId = _spritualGrowthMinistryId;
            _groupService.CreateGroup(group);

            //add our contact to the group as a leader
            var participantSignup = new ParticipantSignup
            {
                particpantId = participant.ParticipantId,
                groupRoleId = _configurationWrapper.GetConfigIntValue("GroupRoleLeader")
            };
            _groupService.addParticipantToGroupNoEvents(group.GroupId, participantSignup);

            //check if we also need to update the home address
            if (!hostRequest.IsHomeAddress) return;
            var addressId = _addressService.CreateAddress(hostRequest.Address);
            // assign new id to users household
            _contactRepository.SetHouseholdAddress(hostRequest.ContactId, contact.Household_ID, addressId);
        }

        public void GatheringJoinRequest(string token, int gatheringId)
        {
            GroupDTO group = _groupService.GetGroupDetails(gatheringId);

            int commType = group.GroupTypeId == _smallGroupType ? _connectCommunicationTypeRequestToJoinSmallGroup : _connectCommunicationTypeRequestToJoinGathering;

            var connection = new ConnectCommunicationDto
            {
                CommunicationTypeId = commType,
                ToContactId = group.ContactId,
                FromContactId = _contactRepository.GetContactId(token),
                CommunicationStatusId = _configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusUnanswered"),
                GroupId = gatheringId
            };
            RecordCommunication(connection);

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

        public int GetLeaderParticipantIdFromGroup(int groupId)
        {
            var participantId = _groupService.GetPrimaryContactParticipantId(groupId);
            return participantId;
        }

        public List<PinDto> GetPinsInBoundingBox(GeoCoordinate originCoords, string userSearchString, AwsBoundingBox boundingBox, string finderType, int contactId)
        {
            List<PinDto> pins = null;
            var queryString = "matchall";

            // new search string for AWS call based on the findertype, use pintype

            if (finderType.Equals(_finderConnect))
            {
                queryString = "(or pintype:3 pintype:2 pintype:1)";
            }
            else if (finderType.Equals(_finderGroupTool))
            {
                queryString = $"(and pintype:4 (or groupname:'{userSearchString}' groupdescription:'{userSearchString}' groupprimarycontactfirstname:'{userSearchString}' groupprimarycontactlastname:'{userSearchString}'))";
            }
            else
            {     
                throw new Exception("No pin search performed - finder type not found");
            }

            var cloudReturn = _awsCloudsearchService.SearchConnectAwsCloudsearch(queryString,
                                                                                    "_all_fields",
                                                                                    _configurationWrapper.GetConfigIntValue("ConnectDefaultNumberOfPins"),
                                                                                    originCoords/*,
                                                                                    boundingBox*/);
            pins = ConvertFromAwsSearchResponse(cloudReturn);

            this.AddPinMetaData(pins, originCoords, contactId);

            return pins;
        }

        public void AddUserDirectlyToGroup(User user, int groupid)
        {
            //check to see if user exists in MP.
            var contactId = _contactRepository.GetContactIdByEmail(user.email);
            if (contactId == 0)
            {
                user.password = System.Web.Security.Membership.GeneratePassword(25, 10);
                contactId = _accountService.RegisterPersonWithoutUserAccount(user);
            }

            var groupParticipant = _groupService.GetGroupParticipants(groupid, false).FirstOrDefault(p => p.ContactId == contactId);

            // groupParticipant == null then participant not in group
            if (groupParticipant == null)
            {
                _groupService.addContactToGroup(groupid, contactId);
            }
            else
            {
                throw new DuplicateGroupParticipantException($"Participant {groupParticipant.ParticipantId} already in group.");
            }
        }

        private void MakeAllLatLongsUnique(List<PinDto> thePins)
        {
           
                var groupedMatchingLatitude = thePins
                .Where(w => w.Address.Latitude != null && w.Address.Longitude != null)
                    .GroupBy(u => new {u.Address.Latitude, u.Address.Longitude})
                    .Select(grp => grp.ToList())
                    .ToList();

            foreach (var grouping in groupedMatchingLatitude.Where(x => x.Count > 1))
            {
                // each of these groups matches latitude, so we need to create slight differences
                double? newLat = 0.0;
                double? newLong = 0.0;
                foreach (var g in grouping)
                {
                    if (newLat.Equals(0.0))
                    {
                        newLat  = g.Address.Latitude;
                        newLong = g.Address.Longitude;
                    }
                    else
                    {
                        newLat += .0001;
                        newLong -= .0001;

                        g.Address.Latitude = newLat;
                        g.Address.Longitude = newLong;
                    }
                }
            }
        }

        private Boolean isMyPin(PinDto pin, int contactId)
        {
            Boolean isMyPin = pin.Contact_ID == contactId && contactId != 0; 
            return isMyPin; 
        }

        private string isMyPinAsString(PinDto pin, int contactId)
        {
            string isMyPinString = isMyPin(pin, contactId).ToString().ToLower();
            return isMyPinString; 
        }

        private string GetPinTitle(PinDto pin, int contactId = 0)
        {
            string jsonData = "";
            var lastname = string.IsNullOrEmpty(pin.LastName) ? " " : pin.LastName[0].ToString();
            switch (pin.PinType)
            {
                case PinType.SITE:
                    jsonData = $"{{ 'siteName': '{RemoveSpecialCharacters(pin.SiteName)}','isHost':  false,'isMe': false,'pinType': {(int) pin.PinType}}}";
                    break;
                case PinType.GATHERING:
                    jsonData = $"{{ 'firstName': '{RemoveSpecialCharacters(pin.FirstName)}', 'lastInitial': '{RemoveSpecialCharacters(lastname)}','isHost':  true,'isMe': false,'pinType': {(int) pin.PinType}}}";
                    break;
                case PinType.PERSON:
                    jsonData = $"{{ 'firstName': '{RemoveSpecialCharacters(pin.FirstName)}', 'lastInitial': '{RemoveSpecialCharacters(lastname)}','isHost':  false,'isMe': {isMyPinAsString(pin, contactId)},'pinType': {(int) pin.PinType}}}";
                    break;
                case PinType.SMALL_GROUP:
                    var groupName = RemoveSpecialCharacters(pin.Gathering.GroupName).Trim();
                    if (groupName.Length > 22)
                    {
                        groupName = RemoveSpecialCharacters(pin.Gathering.GroupName).Trim().Substring(0, 22);
                    }
                    jsonData = $"{{ 'firstName': '{groupName}', 'lastInitial': '','isHost':  false,'isMe': false,'pinType': {(int)pin.PinType}}}";
                    break;
            }

            return jsonData.Replace("'", "\"");
        }

        private static string RemoveSpecialCharacters(string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private string GetPinUrl(PinType pintype)
        {
            switch (pintype)
            {
                case PinType.GATHERING:
                    return _connectGatheringPinUrl;
                case PinType.SITE:
                    return _connectSitePinUrl;
                case PinType.PERSON:
                    return _connectPersonPinUrl;
                case PinType.SMALL_GROUP:
                    return _connectSmallGroupPinUrl;
                default:
                    return _connectPersonPinUrl;
            }
        }

        private List<PinDto> ConvertFromAwsSearchResponse(SearchResponse response)
        {
            var pins = new List<PinDto>();

            foreach (var hit in response.Hits.Hit)
            {
                var pin = new PinDto
                {
                    Proximity = hit.Fields.ContainsKey("proximity") ? Convert.ToDecimal(hit.Fields["proximity"].FirstOrDefault()) : (decimal?) null,
                    PinType = hit.Fields.ContainsKey("pintype") ? (PinType) Convert.ToInt32(hit.Fields["pintype"].FirstOrDefault()) : PinType.PERSON,
                    FirstName = hit.Fields.ContainsKey("firstname") ? hit.Fields["firstname"].FirstOrDefault() : null,
                    LastName = hit.Fields.ContainsKey("lastname") ? hit.Fields["lastname"].FirstOrDefault() : null,
                    SiteName = hit.Fields.ContainsKey("sitename") ? hit.Fields["sitename"].FirstOrDefault() : null,
                    EmailAddress = hit.Fields.ContainsKey("emailaddress") ? hit.Fields["emailaddress"].FirstOrDefault() : null,
                    Contact_ID = hit.Fields.ContainsKey("contactid") ? Convert.ToInt32(hit.Fields["contactid"].FirstOrDefault()) : (int?) null,
                    Participant_ID = hit.Fields.ContainsKey("participantid") ? Convert.ToInt32(hit.Fields["participantid"].FirstOrDefault()) : (int?) null,
                    Host_Status_ID = hit.Fields.ContainsKey("hoststatus") ? Convert.ToInt32(hit.Fields["hoststatus"].FirstOrDefault()) : (int?) null,
                    Household_ID = hit.Fields.ContainsKey("householdid") ? Convert.ToInt32(hit.Fields["householdid"].FirstOrDefault()) : (int?) null,
                    Address = new AddressDTO
                    {
                        AddressID = hit.Fields.ContainsKey("addressid") ? Convert.ToInt32(hit.Fields["addressid"].FirstOrDefault()) : (int?) null,
                        City = hit.Fields.ContainsKey("city") ? hit.Fields["city"].FirstOrDefault() : null,
                        State = hit.Fields.ContainsKey("state") ? hit.Fields["state"].FirstOrDefault() : null,
                        PostalCode = hit.Fields.ContainsKey("zip") ? hit.Fields["zip"].FirstOrDefault() : null,
                        Latitude = hit.Fields.ContainsKey("latitude") ? Convert.ToDouble(hit.Fields["latitude"].FirstOrDefault()) : (double?) null,
                        Longitude = hit.Fields.ContainsKey("longitude") ? Convert.ToDouble(hit.Fields["longitude"].FirstOrDefault()) : (double?) null,
                    }
                };
                if (hit.Fields.ContainsKey("latlong"))
                {
                    var locationstring = hit.Fields["latlong"].FirstOrDefault() ?? "";
                    var coordinates = locationstring.Split(',');
                    pin.Address.Latitude = Convert.ToDouble(coordinates[0]);
                    pin.Address.Longitude = Convert.ToDouble(coordinates[1]);
                }
                if (pin.PinType == PinType.GATHERING || pin.PinType == PinType.SMALL_GROUP)
                {
                    pin.Gathering = new FinderGroupDto
                    {
                        GroupId = hit.Fields.ContainsKey("groupid") ? Convert.ToInt32(hit.Fields["groupid"].FirstOrDefault()) : 0,
                        GroupName = hit.Fields.ContainsKey("groupname") ? hit.Fields["groupname"].FirstOrDefault() : null,
                        GroupDescription = hit.Fields.ContainsKey("groupdescription") ? hit.Fields["groupdescription"].FirstOrDefault() : null,
                        PrimaryContactEmail = hit.Fields.ContainsKey("primarycontactemail") ? hit.Fields["primarycontactemail"].FirstOrDefault() : null,
                        Address = pin.Address,
                        ContactId = pin.Contact_ID.Value,
                        GroupTypeId = _anywhereGroupType,
                        CongregationId = _anywhereCongregationId,
                        MinistryId = _spritualGrowthMinistryId,
                        KidsWelcome = hit.Fields.ContainsKey("groupkidswelcome") && hit.Fields["groupkidswelcome"].FirstOrDefault() == "1",
                        MeetingDay = hit.Fields.ContainsKey("groupmeetingday") ? hit.Fields["groupmeetingday"].FirstOrDefault() : null,
                        MeetingTime = hit.Fields.ContainsKey("groupmeetingtime") ? hit.Fields["groupmeetingtime"].FirstOrDefault() : null,
                        MeetingFrequency = hit.Fields.ContainsKey("groupmeetingfrequency") ? hit.Fields["groupmeetingfrequency"].FirstOrDefault() : null,
                        GroupType = hit.Fields.ContainsKey("grouptype") ? hit.Fields["grouptype"].FirstOrDefault() : null,
                        VirtualGroup = hit.Fields.ContainsKey("groupvirtual") && hit.Fields["groupvirtual"].FirstOrDefault()== "1",
                        PrimaryContactFirstName = hit.Fields.ContainsKey("groupprimarycontactfirstname") ? hit.Fields["groupprimarycontactfirstname"].FirstOrDefault() : null,
                        PrimaryContactLastName = hit.Fields.ContainsKey("groupprimarycontactlastname") ? hit.Fields["groupprimarycontactlastname"].FirstOrDefault() : null,
                        PrimaryContactCongregation = hit.Fields.ContainsKey("groupprimarycontactcongregation") ? hit.Fields["groupprimarycontactcongregation"].FirstOrDefault() : null,
                        GroupAgesRangeList = hit.Fields.ContainsKey("groupagerange") ? hit.Fields["groupagerange"].ToList() : null,
                        GroupCategoriesList = hit.Fields.ContainsKey("groupcategory") ? hit.Fields["groupcategory"].ToList() : null
                    };

                    if (hit.Fields.ContainsKey("groupstartdate") && !String.IsNullOrWhiteSpace(hit.Fields["groupstartdate"].First()))
                    {
                        DateTime? startDate = null;
                        startDate = Convert.ToDateTime(hit.Fields["groupstartdate"].First());
                        pin.Gathering.StartDate = (DateTime) startDate;
                    }

                }
                pins.Add(pin);
            }

            return pins;
        }

        private static decimal GetProximity(GeoCoordinate originCoords, GeoCoordinate pinCoords)
        {
            return (decimal) Proximity(originCoords.Latitude, originCoords.Longitude, pinCoords.Latitude, pinCoords.Longitude);
        }

        private static double Proximity(double lat1, double lon1, double lat2, double lon2)
        {
            var theta = lon1 - lon2;
            var dist = Math.Sin(Deg2Rad(lat1))*Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1))*Math.Cos(Deg2Rad(lat2))*Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist* MinutesInDegree * StatuteMilesInNauticalMile;

            return (dist);
        }

        private static double Deg2Rad(double deg)
        {
            return (deg*Math.PI/180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad/Math.PI*180.0);
        }

        public List<PinDto> GetMyPins(string token, GeoCoordinate originCoords, int contactId, string finderType)
        {
            var pins = new List<PinDto>();
            var participantId = GetParticipantIdFromContact(contactId);

            int[] groupTypesToFetch = finderType == _finderConnect ? new int[] { _anywhereGroupType } : new int[] { _smallGroupType };

            var groupPins = GetMyGroupPins(token, groupTypesToFetch, participantId, finderType);
            var personPin = GetPinDetailsForPerson(participantId);

            pins.AddRange(groupPins);

            if (personPin != null && personPin.ShowOnMap && finderType == _finderConnect)
            {
                pins.Add(personPin);
            }

            foreach (var pin in pins)
            {
                //calculate proximity for all pins to origin
                if (pin.Address.Latitude == null) continue;
                if (pin.Address.Longitude != null) pin.Proximity = GetProximity(originCoords, new GeoCoordinate(pin.Address.Latitude.Value, pin.Address.Longitude.Value));
            }

            pins = this.AddPinMetaData(pins, originCoords, contactId);

            MakeAllLatLongsUnique(pins);

            return pins;
        }

        public List<PinDto> GetMyGroupPins(string token, int[] groupTypeIds, int participantId, string finderType)
        {
            var groupsByType = _groupRepository.GetGroupsForParticipantByTypeOrID(participantId, null, groupTypeIds);

            if (groupsByType == null)
            {
                return null;
            }

            var groupDTOs = groupsByType.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();

            // TODO when do MY STUFF for Group Tool, will need to account for changing this flag to _finderGroupTool
            var pins = this.TransformGroupDtoToPinDto(groupDTOs, finderType);

            return pins;
        }

        public GeoCoordinate GetGeoCoordsFromAddressOrLatLang(string address, GeoCoordinates centerCoords)
        {

            double latitude = centerCoords.Lat.HasValue ? centerCoords.Lat.Value : 0;
            double longitude = centerCoords.Lng.HasValue ? centerCoords.Lng.Value : 0;

            var geoCoordsPassedIn = latitude != 0 && longitude != 0;

            GeoCoordinate originCoordsFromGoogle = geoCoordsPassedIn ? null : _addressGeocodingService.GetGeoCoordinates(address);

            GeoCoordinate originCoordsFromClient = new GeoCoordinate(latitude, longitude);

            GeoCoordinate originCoordinates = geoCoordsPassedIn ? originCoordsFromClient : originCoordsFromGoogle;

            return originCoordinates;
        }

        public GeoCoordinate GetGeoCoordsFromLatLong(string lat, string lng)
        {
            var latitude = Convert.ToDouble(lat.Replace("$", "."));
            var longitude = Convert.ToDouble(lng.Replace("$", "."));

            return new GeoCoordinate(latitude, longitude);
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


        public Invitation InviteToGroup(string token, int gatheringId, User person, string finderFlag)
        {            
            var inviteType = finderFlag.Equals(_finderConnect) ? _anywhereGatheringInvitationTypeId : _groupInvitationTypeId;

            var invitation = new Invitation
            {
                RecipientName = person.firstName,
                EmailAddress = person.email,
                SourceId = gatheringId,
                GroupRoleId = _memberRoleId,
                InvitationType = inviteType, 
                RequestDate = DateTime.Now
            };

            _invitationService.ValidateInvitation(invitation, token);
            invitation = _invitationService.CreateInvitation(invitation, token);

            // TODO US8247 - Guest giver stuff - see story for info

            //if the invitee does not have a contact then create one
            int toContactId;

            try
            {
                toContactId = _contactRepository.GetContactIdByEmail(person.email);
            }
            catch (Exception e) //go ahead and create additional contact, becuase we don't know which contactId to use
            {
                _logger.Info($"Can't get specific contact_id,  '{person.email}', already has multiple contact records, create another, becuase don't know which one to pick", e);
                toContactId = _contactRepository.CreateContactForGuestGiver(person.email, $"{person.lastName}, {person.firstName}", person.firstName, person.lastName);
            }
            
            if (toContactId == 0)
            {
                toContactId = _contactRepository.CreateContactForGuestGiver(person.email, $"{person.lastName}, {person.firstName}", person.firstName, person.lastName);
            }

            var communicationType = finderFlag.Equals(_finderConnect) ? _connectCommunicationTypeInviteToGathering : _connectCommunicationTypeInviteToSmallGroup;

            var connection = new ConnectCommunicationDto
            {               
                CommunicationTypeId = communicationType,
                ToContactId = toContactId,
                FromContactId = _contactRepository.GetContactId(token),
                CommunicationStatusId = _configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusUnanswered"),
                GroupId = gatheringId
            };
            
            RecordCommunication(connection);
            return invitation;
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
                throw new Exception("User does not have access to requested address");
            }
        }

        public AddressDTO GetPersonAddress(string token, int participantId, bool shouldGetFullAddress = true)
        {
            var user = _participantRepository.GetParticipantRecord(token);

            if ((user.ParticipantId == participantId) || !shouldGetFullAddress)
            {
                var address = _finderRepository.GetPinAddress(participantId);

                if (address != null)
                {
                    if (!shouldGetFullAddress)
                    {
                        address.Address_Line_1 = null;
                        address.Address_Line_2 = null;
                    }
                    return Mapper.Map<AddressDTO>(address);
                }
                else
                {
                    throw new Exception("User address not found");
                }
            }
            else
            {
                throw new Exception("User does not have access to requested address");
            }
        }

        private void RecordCommunication(ConnectCommunicationDto connection)
        {
            _finderRepository.RecordConnection(Mapper.Map<MpConnectCommunication>(connection));
        }

        public void SayHi(int fromContactId, int toContactId)
        {
            var connection = new ConnectCommunicationDto
            {
                FromContactId = fromContactId,
                ToContactId = toContactId,
                CommunicationTypeId = _configurationWrapper.GetConfigIntValue("ConnectCommunicationTypeSayHi"),
                CommunicationStatusId = _configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusNA"),
                GroupId = null
            };
            RecordCommunication(connection);
        }

        public void AcceptDenyGroupInvitation(string token, int groupId, string invitationGuid, bool accept)
        {
            try
            {
                _groupToolService.AcceptDenyGroupInvitation(token, groupId, invitationGuid, accept);

                var host = GetPinDetailsForPerson(GetLeaderParticipantIdFromGroup(groupId));
                var cm = _contactRepository.GetContactById(_authenticationRepository.GetContactId(token));

                var connection = new ConnectCommunicationDto
                {
                    FromContactId = cm.Contact_ID,
                    ToContactId = (int) host.Contact_ID,
                    CommunicationTypeId = _connectCommunicationTypeInviteToGathering,
                    CommunicationStatusId =
                        accept
                            ? _configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusAccepted")
                            : _configurationWrapper.GetConfigIntValue("ConnectCommunicationStatusDeclined"),
                    GroupId = groupId
                };
                RecordCommunication(connection);

                SendGatheringInviteResponseEmail(accept, host, cm);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void SendGatheringInviteResponseEmail(bool inviteAccepted, PinDto host, MpMyContact communityMember)
        {
            try
            {
                // basic merge data here
                var mergeData = new Dictionary<string, object>
                {
                    {"Community_Member", communityMember.Nickname + " " + communityMember.Last_Name},
                    {"Host", host.FirstName},
                };

                int emailTemplateId = inviteAccepted ? _inviteAcceptedTemplateId : _inviteDeclinedTemplateId;

                var emailTemplate = _communicationRepository.GetTemplate(emailTemplateId);
                var fromContact = new MpContact
                {
                    ContactId = emailTemplate.FromContactId,
                    EmailAddress = emailTemplate.FromEmailAddress
                };
                var replyTo = new MpContact
                {
                    ContactId = emailTemplate.ReplyToContactId,
                    EmailAddress = emailTemplate.ReplyToEmailAddress
                };

                var to = new List<MpContact>
                {
                    new MpContact
                    {
                        // Just need a contact ID here, doesn't have to be for the recipient
                        ContactId = host.Contact_ID.Value,
                        EmailAddress = host.EmailAddress
                    }
                };

                var confirmation = new MpCommunication
                {
                    EmailBody = emailTemplate.Body,
                    EmailSubject = emailTemplate.Subject,
                    AuthorUserId = 5,
                    DomainId = _domainId,
                    FromContact = fromContact,
                    ReplyToContact = replyTo,
                    TemplateId = emailTemplateId,
                    ToContacts = to,
                    MergeData = mergeData
                };
                _communicationRepository.SendMessage(confirmation);
            }
            catch (Exception e)
            {
                return;
            }
        }

        private List<PinDto> TransformGroupDtoToPinDto(List<GroupDTO> groupDTOs, string finderType)
        {
            var pins = new List<PinDto>();

            if (finderType.Equals(_finderConnect))
            {
                foreach (var group in groupDTOs)
                {
                    var pin = Mapper.Map<PinDto>(group);
                    pin.Gathering = Mapper.Map<FinderGroupDto>(group);

                    pin.Gathering.ContactId = group.ContactId;
                    pin.Participant_ID = group.ParticipantId;

                    // TODO need to get rid of this call to GetContactById if get name from AWS search instead
                    //var contact = _contactRepository.GetContactById((int)pin.Contact_ID);
                    //pin.FirstName = contact.First_Name;
                    //pin.LastName = contact.Last_Name;
                    pins.Add(pin);
                }
            }
            else if (finderType.Equals(_finderGroupTool))
            {
                foreach (var group in groupDTOs)
                {
                    var pin = Mapper.Map<PinDto>(group);
                    pin.Gathering = Mapper.Map<FinderGroupDto>(group);
                    pin.PinType = PinType.SMALL_GROUP;

                    pin.Gathering.ContactId = group.ContactId;
                    pin.Participant_ID = group.ParticipantId;

                    pins.Add(pin);
                }
            }

            return pins;
        }

        public List<PinDto> AddPinMetaData(List<PinDto> pins, GeoCoordinate originCoords, int contactId = 0)
        {
            foreach (var pin in pins)
            {
                pin.Title = GetPinTitle(pin, contactId);
                pin.IconUrl = GetPinUrl(pin.PinType);

                // Have GROUP address, but no coordinates, get geocordinates and save in MP
                if ((pin.PinType == PinType.GATHERING || pin.PinType == PinType.SMALL_GROUP) && pin.Address.PostalCode != null && pin.Address.Longitude == null)
                {
                    // Everything will go to a state level with bad address - because state is required select control
                    _addressService.SetGroupPinGeoCoordinates(pin);

                    // TODO check error handling here - I did an update on non-existant group and hosed up AWS
                    // TODO uncomment when small groups are in AWS
                    // _awsCloudsearchService.UploadNewPinToAws(pin);
                }

                //calculate proximity for all pins to origin
                if (pin.Address.Latitude == null) continue;
                if (pin.Address.Longitude != null && originCoords != null)
                {
                    pin.Proximity = GetProximity(originCoords, new GeoCoordinate(pin.Address.Latitude.Value, pin.Address.Longitude.Value));
                }

            }
            return pins;
        }

        public Boolean areAllBoundingBoxParamsPresent(MapBoundingBox boundingBox)
        {
            var isUpperLeftLatNull = boundingBox.UpperLeftLat == null;
            var isUpperLeftLngNull = boundingBox.UpperLeftLng == null;
            var isBottomRightLatNull = boundingBox.BottomRightLat == null;
            var isBottomRightLngNull = boundingBox.BottomRightLng == null;
            Boolean areAllBoundingBoxParamsPresent = !isUpperLeftLatNull && !isUpperLeftLngNull && !isBottomRightLatNull && !isBottomRightLngNull;

            return areAllBoundingBoxParamsPresent; 
        }

        public List<PinDto> RandomizeLatLongForNonSitePins(List<PinDto> pins)
        {
            foreach (var pin in pins)
            {
                if (pin.PinType != PinType.SITE)
                {
                    pin.Address = RandomizeLatLong(pin.Address);
                }
            }

            return pins;
        }

        public GeoCoordinate GetMapCenterForResults(string userSearchString, GeoCoordinates frontEndMapCenter, string finderType)
        {
            GeoCoordinate resultMapCenterCoords = new GeoCoordinate();

            if (finderType == _finderConnect)
            {
                resultMapCenterCoords = GetGeoCoordsFromAddressOrLatLang(userSearchString, frontEndMapCenter);
            }
            else
            {
                if (frontEndMapCenter.Lat.HasValue && frontEndMapCenter.Lng.HasValue)
                {
                    resultMapCenterCoords  = new GeoCoordinate(frontEndMapCenter.Lat.Value, frontEndMapCenter.Lng.Value);
                }
                else
                {
                    resultMapCenterCoords = GetGeoCoordsFromAddressOrLatLang(userSearchString, frontEndMapCenter);
                }
            }

            return resultMapCenterCoords;
        }

        public List<User> GetMatches(User user)
        {
            var people = new List<User>();

            var contacts = _contactRepository.GetPotentialMatchesContact(user.firstName, user.lastName, user.email);

            foreach (var contact in contacts)
            {
                var newEntry = new User
                {
                    firstName = contact.First_Name,
                    lastName = contact.Last_Name,
                    email = contact.Email_Address
                };
                people.Add(newEntry);
            }

            return people;
        }
    }
}

