﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Finder;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Finder;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Repositories.Interfaces;
using AutoMapper;
using crds_angular.Models.Crossroads.Groups;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Rhino.Mocks;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Exceptions;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Services.Analytics;
using Crossroads.Web.Common.Security;
using MvcContrib.TestHelper;


namespace crds_angular.test.Services
{
    [TestFixture]
    public class FinderServiceTest
    {
        private FinderService _fixture;
        private Mock<IAddressGeocodingService> _addressGeocodingService;
        private Mock<IFinderRepository> _mpFinderRepository;
        private Mock<IContactRepository> _mpContactRepository;
        private Mock<IAddressService> _addressService;
        private Mock<IParticipantRepository> _mpParticipantRepository;
        private Mock<IConfigurationWrapper> _mpConfigurationWrapper;
        private Mock<IGroupToolService> _mpGroupToolService;
        private Mock<IGroupService> _groupService;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IAddressProximityService> _addressProximityService;
        private Mock<IInvitationService> _invitationService;
        private Mock<IGroupRepository> _mpGroupRepository;
        private Mock<IAwsCloudsearchService> _awsCloudsearchService;
        private Mock<IFinderService> _mpFinderServiceMock;
        private Mock<IAuthenticationRepository> _authenticationRepository;
        private Mock<ICommunicationRepository> _communicationRepository;
      
        private Mock<IAccountService> _accountService;
        private Mock<ILookupService> _lookupService;
        private Mock<IAnalyticsService> _analyticsService;

        private int _memberRoleId = 16;
        private int _anywhereGatheringInvitationTypeId = 3;
        private int _groupInvitationTypeId = 1;

        [SetUp]
        public void SetUp()
        {
            _addressGeocodingService = new Mock<IAddressGeocodingService>();
            _mpFinderRepository = new Mock<IFinderRepository>();
            _mpContactRepository = new Mock<IContactRepository>();
            _addressService = new Mock<IAddressService>();
            _mpParticipantRepository = new Mock<IParticipantRepository>();
            _mpGroupToolService = new Mock<IGroupToolService>();
            _mpConfigurationWrapper = new Mock<IConfigurationWrapper>();
            _addressProximityService = new Mock<IAddressProximityService>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _groupService = new Mock<IGroupService>();
            _invitationService = new Mock<IInvitationService>();
            _mpGroupRepository = new Mock<IGroupRepository>();
            _awsCloudsearchService = new Mock<IAwsCloudsearchService>();
            _authenticationRepository = new Mock<IAuthenticationRepository>();
            _communicationRepository = new Mock<ICommunicationRepository>();
            _accountService = new Mock<IAccountService>();
            _analyticsService = new Mock<IAnalyticsService>();
            

            _mpFinderServiceMock = new Mock<IFinderService>(MockBehavior.Strict);
            _lookupService = new Mock<ILookupService>();

            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("GroupRoleLeader")).Returns(22);
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("ApprovedHostStatus")).Returns(3);
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("AnywhereGroupTypeId")).Returns(30);
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("SmallGroupTypeId")).Returns(1);
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigValue("FinderConnectFlag")).Returns("CONNECT");
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("Group_Role_Default_ID")).Returns(_memberRoleId);
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("AnywhereGatheringInvitationType")).Returns(_anywhereGatheringInvitationTypeId);
            _mpConfigurationWrapper.Setup(mocked => mocked.GetConfigIntValue("GroupInvitationType")).Returns(_groupInvitationTypeId);

            _fixture = new FinderService(_addressGeocodingService.Object,
                                         _mpFinderRepository.Object,
                                         _mpContactRepository.Object,
                                         _addressService.Object,
                                         _mpParticipantRepository.Object,
                                         _mpGroupRepository.Object,
                                         _groupService.Object,
                                         _mpGroupToolService.Object,
                                         _apiUserRepository.Object,
                                         _mpConfigurationWrapper.Object,
                                         _invitationService.Object,
                                         _awsCloudsearchService.Object,
                                         _authenticationRepository.Object,
                                         _communicationRepository.Object,
                                         _accountService.Object,
                                         _lookupService.Object,
                                         _analyticsService.Object);

            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User does not have access to requested address")]
        public void ShouldThrowExceptionIfParticipantIdsDontMatch()
        {
            const int participantId = 42;
            const string token = "ABC";
            const int addressParticipantId = 99;

            _mpParticipantRepository.Setup(mock => mock.GetParticipantRecord(token)).Returns(new MpParticipant()
            {
                ParticipantId = participantId
            });

            _fixture.GetPersonAddress(token, addressParticipantId, true);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User address not found")]
        public void GetPersonShouldThrowWhenAddressNotFound()
        {
            const int participantId = 42;
            const string token = "ABC";


            _mpParticipantRepository.Setup(mock => mock.GetParticipantRecord(token)).Returns(new MpParticipant()
            {
                ParticipantId = participantId
            });

            _mpFinderRepository.Setup(mock => mock.GetPinAddress(participantId)).Returns((MpAddress) null);

            _fixture.GetPersonAddress(token, participantId, true);
        }

        [Test]
        public void ShouldGetFullPersonAddress()
        {
            const int participantId = 42;
            const string token = "ABC";
            _mpParticipantRepository.Setup(mock => mock.GetParticipantRecord(token)).Returns(new MpParticipant()
            {
                ParticipantId = participantId
            });

            _mpFinderRepository.Setup(mock => mock.GetPinAddress(participantId)).Returns(this.getAMpAddress());

            var result = _fixture.GetPersonAddress(token, participantId, true);
            Assert.AreEqual(result.AddressID, 1);
            Assert.AreEqual(result.AddressLine1, "1 Street");

        }

        [Test]
        public void ShouldCallAnalyticsWhenAcceptingAnInvite()
        {
            var hostPin = GetAPin(2);
            var invitedContact = new MpMyContact()
            {
                Contact_ID = 42,
                Email_Address = "invited@email.com"
            };
            const int groupId = 42;
            const string invitationGuid = "ILIKEICECREAM";
            const string token = "IAMAUTHENTICATEDMAN";

            _mpGroupToolService.Setup(m => m.AcceptDenyGroupInvitation(
                                          It.Is<string>(toke => toke.Equals(token)),
                                          It.Is<int>(GroupID => GroupID == groupId),
                                          It.Is<string>(invite => invite.Equals(invitationGuid)),
                                          It.Is<Boolean>(t => t == true)
                                      ));

            _groupService.Setup(m => m.GetPrimaryContactParticipantId(It.Is<int>(GroupID => GroupID == groupId))).Returns(42);
            _mpFinderRepository.Setup(m => m.GetPinDetails(It.Is<int>(partId => partId == 42))).Returns(convertPinDtoToFinderPinDto(hostPin));
            _mpContactRepository.Setup(m => m.GetContactById(It.Is<int>(id => id == invitedContact.Contact_ID))).Returns(invitedContact);
            _authenticationRepository.Setup(m => m.GetContactId(It.Is<string>(Token => Token.Equals(token)))).Returns(invitedContact.Contact_ID);
            _mpFinderRepository.Setup(m => m.RecordConnection(It.IsAny<MpConnectCommunication>()));
            _communicationRepository.Setup(m => m.GetTemplate(It.IsAny<int>())).Returns(new MpMessageTemplate());
            _communicationRepository.Setup(m => m.SendMessage(It.IsAny<MinistryPlatform.Translation.Models.MpCommunication>(), false));
            _analyticsService.Setup(m => m.Track(
                                        It.Is<string>(hostId => hostId.Equals(hostPin.Contact_ID.ToString())),
                                        It.Is<string>(eventName => eventName.Equals("HostInvitationAccepted")),
                                        It.Is<EventProperties>(props => props["InvitationTo"].Equals(invitedContact.Contact_ID)
                                                                        && props["InvitationToEmail"].Equals(invitedContact.Email_Address))
                                    ));

            _analyticsService.Setup(m => m.Track(
                                        It.Is<string>(invitedId => invitedId.Equals(invitedContact.Contact_ID.ToString())),
                                        It.Is<string>(eventName => eventName.Equals("InviteeAcceptedInvitation")),
                                        It.Is<EventProperties>(props => props["InvitationFrom"].Equals(hostPin.Contact_ID)
                                                                        && props["InvitationFromEmail"].Equals(hostPin.EmailAddress))
                                    ));

            _fixture.AcceptDenyGroupInvitation(token, groupId, invitationGuid, true);
            _analyticsService.VerifyAll();
            _groupService.VerifyAll();
        }

        [Test]
        public void ShouldGetPartialPersonAddress()
        {
            const int participantId = 42;
            const string token = "ABC";
            _mpParticipantRepository.Setup(mock => mock.GetParticipantRecord(token)).Returns(new MpParticipant()
            {
                ParticipantId = participantId
            });

            _mpFinderRepository.Setup(mock => mock.GetPinAddress(participantId)).Returns(this.getAMpAddress());

            var result = _fixture.GetPersonAddress(token, participantId, false);
            Assert.AreEqual(result.AddressID, 1);
            Assert.AreEqual(result.AddressLine1, null);
            Assert.AreEqual(result.AddressLine2, null);
            Assert.AreEqual(result.City, "City!");

        }

        [Test]
        public void ShouldGetPartialAddressDifferentParticipantId()
        {
            const int participantId = 42;
            const int addressParticipantId = 33;
            const string token = "ABC";
            _mpParticipantRepository.Setup(mock => mock.GetParticipantRecord(token)).Returns(new MpParticipant()
            {
                ParticipantId = participantId
            });

            _mpFinderRepository.Setup(mock => mock.GetPinAddress(addressParticipantId)).Returns(this.getAMpAddress());

            var result = _fixture.GetPersonAddress(token, addressParticipantId, false);
            Assert.AreEqual(result.AddressID, 1);
            Assert.AreEqual(result.AddressLine1, null);
            Assert.AreEqual(result.AddressLine2, null);
            Assert.AreEqual(result.City, "City!");

        }

        [Test]
        public void ShouldGetPersonPinDetails()
        {
            _apiUserRepository.Setup(ar => ar.GetToken()).Returns("abc123");
            _mpFinderRepository.Setup(m => m.GetPinDetails(123))
                .Returns(new FinderPinDto
                {
                    LastName = "Ker",
                    FirstName = "Joe",
                    Address = new MpAddress {Address_ID = 12, Postal_Code = "1234", Address_Line_1 = "123 street", City = "City", State = "OH"},
                    Participant_ID = 123,
                    EmailAddress = "joeker@gmail.com",
                    Contact_ID = 22,
                    Household_ID = 13,
                    Host_Status_ID = 3
                });

            var result = _fixture.GetPinDetailsForPerson(123);

            _mpFinderRepository.VerifyAll();

            Assert.AreEqual(result.LastName, "Ker");
            Assert.AreEqual(result.Address.AddressID, 12);
            Assert.AreEqual(result.PinType, PinType.PERSON);
        }


        [Test]
        public void ShouldGetGroupPinDetailsAnywhere()
        {

            var searchresults = new SearchResponse();
            searchresults.Hits = new Hits();
            searchresults.Hits.Found = 1;
            searchresults.Hits.Start = 0;
            searchresults.Hits.Hit = new List<Hit>();
            var hit = new Hit();
            var fields = new Dictionary<string, List<string>>();
            fields.Add("firstname", new List<string>() {"Sara"});
            fields.Add("lastname", new List<string>() {"Smith"});
            fields.Add("pintype", new List<string>() {"2"});
            fields.Add("latlong", new List<string>() {"38.94526,-84.661275"});
            fields.Add("groupid", new List<string>() {"121212"});
            fields.Add("city", new List<string>() {"Union"});
            fields.Add("zip", new List<string>() {"41091"});
            fields.Add("contactid", new List<string>() {"111111"});

            hit.Fields = fields;
            searchresults.Hits.Hit.Add(hit);

            _awsCloudsearchService.Setup(
                mocked => mocked.SearchByGroupId(It.IsAny<string>())).Returns(searchresults);

            var result = _fixture.GetPinDetailsForGroup(121212, new GeoCoordinate(38.94526, -84.661275));

            Assert.IsInstanceOf<PinDto>(result);

            Assert.AreEqual(result.FirstName, "Sara");
            Assert.AreEqual(result.Gathering.GroupId, 121212);
            Assert.AreEqual(result.PinType, PinType.GATHERING);
        }

        [Test]
        public void ShouldGetGroupPinDetailsSmallGroup()
        {
            var searchresults = new SearchResponse();
            searchresults.Hits = new Hits();
            searchresults.Hits.Found = 1;
            searchresults.Hits.Start = 0;
            searchresults.Hits.Hit = new List<Hit>();
            var hit = new Hit();
            var fields = new Dictionary<string, List<string>>();
            fields.Add("firstname", new List<string>() {"Sara"});
            fields.Add("lastname", new List<string>() {"Smith"});
            fields.Add("pintype", new List<string>() {"4"});
            fields.Add("latlong", new List<string>() {"38.94526,-84.661275"});
            fields.Add("groupid", new List<string>() {"121212"});
            fields.Add("groupname", new List<string>() {"Sara S."});
            fields.Add("city", new List<string>() {"Union"});
            fields.Add("zip", new List<string>() {"41091"});
            fields.Add("contactid", new List<string>() {"111111"});

            hit.Fields = fields;
            searchresults.Hits.Hit.Add(hit);

            _awsCloudsearchService.Setup(
                mocked => mocked.SearchByGroupId(It.IsAny<string>())).Returns(searchresults);

            var result = _fixture.GetPinDetailsForGroup(121212, new GeoCoordinate(38.94526, -84.661275));

            Assert.IsInstanceOf<PinDto>(result);

            Assert.AreEqual(result.FirstName, "Sara");
            Assert.AreEqual(result.Gathering.GroupId, 121212);
            Assert.AreEqual(result.PinType, PinType.SMALL_GROUP);
        }

        [Test]
        public void ShouldEnablePin()
        {
            _mpFinderRepository.Setup(m => m.EnablePin(123));
            _fixture.EnablePin(123);
            _mpFinderRepository.VerifyAll();
        }

        [Test]
        public void ShouldDisablePin()
        {
            _mpFinderRepository.Setup(m => m.EnablePin(123));
            _fixture.EnablePin(123);
            _fixture.DisablePin(123);
            _mpFinderRepository.VerifyAll();
        }

        [Test]
        public void ShouldGetGeoCoordinatesFromLatLang()
        {
            const string address = "123 Main Street, Walton, KY";

            var mockCoords = new GeoCoordinate()
            {
                Latitude = 39.2844738,
                Longitude = -84.319614
            };

            _addressGeocodingService.Setup(mocked => mocked.GetGeoCoordinates(address)).Returns(mockCoords);

            GeoCoordinate geoCoords = _fixture.GetGeoCoordsFromAddressOrLatLang(address, new GeoCoordinates(39.2844738, -84.319614));
            Assert.AreEqual(mockCoords, geoCoords);
        }

        [Test]
        public void ShouldGetGeoCoordinatesFromAddress()
        {
            const string address = "123 Main Street, Walton, KY";

            var mockCoords = new GeoCoordinate()
            {
                Latitude = 39.2844738,
                Longitude = -84.319614
            };

            _addressGeocodingService.Setup(mocked => mocked.GetGeoCoordinates(address)).Returns(mockCoords);

            GeoCoordinate geoCoords = _fixture.GetGeoCoordsFromAddressOrLatLang(address, new GeoCoordinates(0, 0));
            Assert.AreEqual(mockCoords, geoCoords);
        }

        [Test]
        public void ShouldReturnAListOfPinsWhenSearching()
        {
            const string address = "123 Main Street, Walton, KY";
            var originCoords = new GeoCoordinate()
            {
                Latitude = 39.2844738,
                Longitude = -84.319614
            };

            var searchresults = new SearchResponse();
            searchresults.Hits = new Hits();
            searchresults.Hits.Found = 1;
            searchresults.Hits.Start = 0;
            searchresults.Hits.Hit = new List<Hit>();
            var hit = new Hit();
            var fields = new Dictionary<string, List<string>>();
            fields.Add("city", new List<string>() {"Union"});
            fields.Add("zip", new List<string>() {"41091"});
            fields.Add("firstname", new List<string>() {"Robert"});
            fields.Add("lastname", new List<string>() {"Smith"});
            fields.Add("latlong", new List<string>() {"38.94526,-84.661275"});
            hit.Fields = fields;
            searchresults.Hits.Hit.Add(hit);

            _awsCloudsearchService.Setup(
                    mocked => mocked.SearchConnectAwsCloudsearch(It.IsAny<string>(), "_all_fields", It.IsAny<int>(), It.IsAny<GeoCoordinate>(), It.IsAny<AwsBoundingBox>()))
                .Returns(searchresults);

            _mpGroupToolService.Setup(m => m.SearchGroups(It.IsAny<int[]>(), null, It.IsAny<string>(), null, originCoords)).Returns(new List<GroupDTO>());
            _mpFinderRepository.Setup(mocked => mocked.GetPinsInRadius(originCoords)).Returns(new List<SpPinDto>());
            _addressGeocodingService.Setup(mocked => mocked.GetGeoCoordinates(address)).Returns(originCoords);
            _addressProximityService.Setup(mocked => mocked.GetProximity(address, new List<AddressDTO>(), originCoords)).Returns(new List<decimal?>());
            _addressProximityService.Setup(mocked => mocked.GetProximity(address, new List<string>(), originCoords)).Returns(new List<decimal?>());


            var boundingBox = new AwsBoundingBox
            {
                UpperLeftCoordinates = new GeoCoordinates(61.21, -149.9),
                BottomRightCoordinates = new GeoCoordinates(21.52, -77.78)
            };

            List<PinDto> pins = _fixture.GetPinsInBoundingBox(originCoords, address, boundingBox, "CONNECT", 0, "filterstring");

            Assert.IsInstanceOf<List<PinDto>>(pins);
        }

        public void ShouldRandomizeThePosition()
        {
            const double originalLatitude = 59.6378639;
            const double originalLongitude = -151.5068732;

            var address = new AddressDTO
            {
                AddressID = 222,
                AddressLine1 = "1393 Bay Avenue",
                City = "Homer",
                State = "AK",
                PostalCode = "99603",
                Latitude = originalLatitude,
                Longitude = originalLongitude
            };

            var result = _fixture.RandomizeLatLong(address);
            Assert.AreNotEqual(result.Longitude, originalLongitude);
            Assert.AreNotEqual(result.Latitude, originalLatitude);
        }

        [Test]
        public void ShouldUpdateHouseholdAddress()
        {
            var pin = new PinDto
            {
                Address = new AddressDTO
                {
                    AddressID = 741,
                    AddressLine1 = "123 Main Street",
                    City = "Cincinnati",
                    State = "OH",
                    PostalCode = "45249"
                },
                Contact_ID = 123,
                Participant_ID = 456,
                Household_ID = 789,
                EmailAddress = "",
                FirstName = "",
                LastName = "",
                Gathering = null,
                Host_Status_ID = 0
            };

            var geoCodes = new GeoCoordinate() {Altitude = 0, Course = 0, HorizontalAccuracy = 0, Latitude = 10, Longitude = 20, Speed = 0, VerticalAccuracy = 0};

            var addressDictionary = new Dictionary<string, object>
            {
                {"AddressID", pin.Address.AddressID},
                {"AddressLine1", pin.Address.AddressID},
                {"City", pin.Address.AddressID},
                {"State/Region", pin.Address.AddressID},
                {"PostCode", pin.Address.AddressID}
            };
            var householdDictionary = new Dictionary<string, object> {{"Household_ID", pin.Household_ID}};

            _addressGeocodingService.Setup(mocked => mocked.GetGeoCoordinates(It.IsAny<AddressDTO>())).Returns(geoCodes);
            _addressService.Setup(m => m.SetGeoCoordinates(pin.Address));
            _mpContactRepository.Setup(m => m.UpdateHouseholdAddress((int) pin.Household_ID, householdDictionary, addressDictionary));
            _addressService.Setup(m => m.GetGeoLocationCascading(It.IsAny<AddressDTO>())).Returns(new GeoCoordinate(39, -84));

            _fixture.UpdateHouseholdAddress(pin);
            _mpFinderRepository.VerifyAll();
        }

        [Test]
        public void TestRequestToBeHost()
        {
            var token = "faketoken";
            var hostRequestDto = new HostRequestDto
            {
                ContactId = 123,
                GroupDescription = "fake group description",
                IsHomeAddress = false,
                ContactNumber = "555-123-4567",
                Address = new AddressDTO
                {
                    AddressID = 1,
                    AddressLine1 = "123 Main St",
                    City = "Cincinnati",
                    State = "OH",
                    PostalCode = "45249"
                }
            };

            var contact = new MpMyContact
            {
                Contact_ID = 123,
                Email_Address = "bob@bob.com",
                Nickname = "Bob",
                Last_Name = "Bobert"
            };
            var participant = new MpParticipant {ParticipantId = 999};

            var group = new GroupDTO();
            group.GroupId = 555;


            _mpContactRepository.Setup(m => m.GetContactById(It.IsAny<int>())).Returns(contact);
            _mpParticipantRepository.Setup(m => m.GetParticipant(It.IsAny<int>())).Returns(participant);
            _groupService.Setup(m => m.CreateGroup(It.IsAny<GroupDTO>())).Returns(group);
            _groupService.Setup(m => m.addParticipantToGroupNoEvents(It.IsAny<int>(), It.IsAny<ParticipantSignup>()));
            _addressService.Setup(m => m.CreateAddress(It.IsAny<AddressDTO>())).Returns(57);

            _mpGroupRepository.Setup(m => m.GetGroupsByGroupType(It.IsAny<int>())).Returns(new List<MpGroup>());

            _fixture.RequestToBeHost(token, hostRequestDto);

            _groupService.Verify(x => x.addParticipantToGroupNoEvents(It.IsAny<int>(), It.IsAny<ParticipantSignup>()), Times.Once);
            _mpContactRepository.Verify(x => x.SetHouseholdAddress(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void RequestToBeHostShouldThrow()
        {
            var token = "faketoken";
            var hostRequestDto = new HostRequestDto
            {
                ContactId = 123,
                GroupDescription = "fake group description",
                IsHomeAddress = false,
                ContactNumber = "555-123-4567",
                Address = new AddressDTO
                {
                    AddressLine1 = "123 Main St",
                    City = "Cincinnati",
                    State = "OH",
                    PostalCode = "45249"
                }
            };

            var searchResult1 = new MpGroup
            {
                ContactId = 456,
                PrimaryContact = "456",
                Address = new MpAddress()
                {
                    Address_ID = 1,
                    Address_Line_1 = "42 Elm St",
                    City = "Florence",
                    State = "KY",
                    Postal_Code = "45202"
                }
            };
            var searchResult2 = new MpGroup
            {
                ContactId = 123,
                PrimaryContact = "123",
                Address = new MpAddress()
                {
                    Address_ID = 2,
                    Address_Line_1 = "123 Main St",
                    City = "Cincinnati",
                    State = "OH",
                    Postal_Code = "45249"
                }
            };

            var searchResult3 = new MpGroup
            {
                ContactId = 123,
                PrimaryContact = "123",
                Address = new MpAddress()
                {
                    Address_ID = 2,
                    Address_Line_1 = "99 SomewhereElse Ave",
                    City = "Cincinnati",
                    State = "OH",
                    Postal_Code = "45249"
                }
            };
            var searchResults = new List<MpGroup> {searchResult1, searchResult2, searchResult3};
            _mpGroupRepository.Setup(m => m.GetGroupsByGroupType(It.IsAny<int>())).Returns(searchResults);

            Assert.That(() => _fixture.RequestToBeHost(token, hostRequestDto),
                        Throws.Exception
                            .TypeOf<GatheringException>());
        }

        [Test]
        public void ShouldUpdateGathering()
        {
            var pin = this.GetAPin();
            _addressService.Setup(mocked => mocked.GetGeoLocationCascading(pin.Gathering.Address))
                .Returns(new GeoCoordinate() {Altitude = 0, Course = 0, HorizontalAccuracy = 0, Latitude = 10, Longitude = 20, Speed = 0, VerticalAccuracy = 0});

            var expectedPin = this.GetAPin();
            expectedPin.Gathering.Address.Longitude = 20;
            expectedPin.Gathering.Address.Latitude = 10;

            var expectedFinderGathering = Mapper.Map<FinderGatheringDto>(expectedPin.Gathering);

            _mpFinderRepository.Setup(mocked => mocked.UpdateGathering(It.IsAny<FinderGatheringDto>())).Returns(expectedFinderGathering);

            var result = _fixture.UpdateGathering(pin);
            _addressService.VerifyAll();
            _mpFinderRepository.VerifyAll();
            result.ShouldBe(pin);
        }

        [Test]
        public void ShouldUpdateGatheringAndUpdateHouseholdAddress()
        {
            var geoCodes = new GeoCoordinate() {Altitude = 0, Course = 0, HorizontalAccuracy = 0, Latitude = 10, Longitude = 20, Speed = 0, VerticalAccuracy = 0};
            var pin = this.GetAPin();
            var updatedAddress = new AddressDTO()
            {
                AddressID = pin.Address.AddressID,
                AddressLine1 = pin.Gathering.Address.AddressLine1,
                AddressLine2 = pin.Gathering.Address.AddressLine2,
                Longitude = pin.Gathering.Address.Longitude,
                Latitude = pin.Gathering.Address.Latitude,
                City = pin.Gathering.Address.City,
                County = pin.Gathering.Address.County,
                ForeignCountry = pin.Gathering.Address.ForeignCountry,
                PostalCode = pin.Gathering.Address.PostalCode,
                State = pin.Gathering.Address.State
            };

            var expectedPin = this.GetAPin();
            expectedPin.Gathering.Address.Longitude = 20;
            expectedPin.Gathering.Address.Latitude = 10;
            expectedPin.ShouldUpdateHomeAddress = true;
            expectedPin.Address = updatedAddress;

            var expectedFinderGathering = Mapper.Map<FinderGatheringDto>(expectedPin.Gathering);

            pin.ShouldUpdateHomeAddress = true;

            _addressGeocodingService.Setup(mocked => mocked.GetGeoCoordinates(It.IsAny<AddressDTO>())).Returns(geoCodes);
            _addressService.Setup(mocked => mocked.GetGeoLocationCascading(It.IsAny<AddressDTO>())).Returns(geoCodes);
            _mpContactRepository.Setup(mocked => mocked.UpdateHouseholdAddress(pin.Contact_ID.Value, null, It.IsAny<Dictionary<string, object>>()));
            _mpFinderRepository.Setup(mocked => mocked.UpdateGathering(It.IsAny<FinderGatheringDto>())).Returns(expectedFinderGathering);
            _awsCloudsearchService.Setup(mocked => mocked.UploadNewPinToAws(It.IsAny<PinDto>()));

            var result = _fixture.UpdateGathering(pin);
            _addressService.Verify(ver => ver.GetGeoLocationCascading(It.IsAny<AddressDTO>()), Times.Exactly(1));
            _mpFinderRepository.VerifyAll();
            _mpContactRepository.VerifyAll();
            _awsCloudsearchService.VerifyAll();
            Assert.AreEqual(result.Address.AddressLine1, expectedPin.Address.AddressLine1);
            Assert.AreEqual(result.Address.AddressID, expectedPin.Address.AddressID);
        }

        [Test]
        public void ShouldSayHi()
        {
            _mpConfigurationWrapper.Setup(x => x.GetConfigIntValue(It.IsAny<string>())).Returns(1);
            _mpFinderRepository.Setup(mocked => mocked.RecordConnection(It.IsAny<MpConnectCommunication>()));

            _fixture.SayHi(123, 456);
            _mpFinderRepository.Verify(m => m.RecordConnection(It.IsAny<MpConnectCommunication>()), Times.Once);
        }

        [Test]
        public void ShouldInviteToGathering()
        {
            string token = "abc";
            int gatheringId = 12345;
            User person = new User()
            {
                firstName = "doug",
                lastName = "shannon",
                email = "a@b.com",
            };

            Invitation expectedInvitation = new Invitation()
            {
                RecipientName = person.firstName,
                EmailAddress = person.email,
                SourceId = gatheringId,
                GroupRoleId = _memberRoleId,
                InvitationType = _anywhereGatheringInvitationTypeId,
                CommunicationId = 7
            };

            _invitationService.Setup(i => i.ValidateInvitation(It.Is<Invitation>(
                                                                   (inv) => inv.RecipientName == expectedInvitation.RecipientName
                                                                            && inv.EmailAddress == expectedInvitation.EmailAddress
                                                                            && inv.SourceId == expectedInvitation.SourceId
                                                                            && inv.GroupRoleId == expectedInvitation.GroupRoleId
                                                                            && inv.InvitationType == expectedInvitation.InvitationType),
                                                               It.Is<string>((s) => s == token)));

            _invitationService.Setup(i => i.CreateInvitation(It.Is<Invitation>(
                                                                 (inv) => inv.RecipientName == expectedInvitation.RecipientName
                                                                          && inv.EmailAddress == expectedInvitation.EmailAddress
                                                                          && inv.SourceId == expectedInvitation.SourceId
                                                                          && inv.GroupRoleId == expectedInvitation.GroupRoleId
                                                                          && inv.InvitationType == expectedInvitation.InvitationType),
                                                             It.Is<string>((s) => s == token))).Returns(expectedInvitation);
            _mpFinderRepository.Setup(x => x.RecordConnection(It.IsAny<MpConnectCommunication>()));
            _mpConfigurationWrapper.Setup(x => x.GetConfigIntValue(It.IsAny<string>())).Returns(1);
            _mpContactRepository.Setup(x => x.GetContactIdByEmail(It.IsAny<string>())).Returns(2);
            _mpContactRepository.Setup(x => x.GetContactId(It.IsAny<string>())).Returns(3);
            _fixture.InviteToGroup(token, gatheringId, person, "CONNECT");
            _invitationService.VerifyAll();
        }

        [Test]
        public void ShouldInviteToSmallGroup()
        {
            string token = "abc";
            int gatheringId = 12345;
            User person = new User()
            {
                firstName = "doug",
                lastName = "shannon",
                email = "a@b.com",
            };

            Invitation expectedInvitation = new Invitation()
            {
                RecipientName = person.firstName,
                EmailAddress = person.email,
                SourceId = gatheringId,
                GroupRoleId = _memberRoleId,
                InvitationType = _groupInvitationTypeId,
                CommunicationId = 7
            };

            _invitationService.Setup(i => i.ValidateInvitation(It.Is<Invitation>(
                                                                   (inv) => inv.RecipientName == expectedInvitation.RecipientName
                                                                            && inv.EmailAddress == expectedInvitation.EmailAddress
                                                                            && inv.SourceId == expectedInvitation.SourceId
                                                                            && inv.GroupRoleId == expectedInvitation.GroupRoleId
                                                                            && inv.InvitationType == expectedInvitation.InvitationType),
                                                               It.Is<string>((s) => s == token)));

            _invitationService.Setup(i => i.CreateInvitation(It.Is<Invitation>(
                                                                 (inv) => inv.RecipientName == expectedInvitation.RecipientName
                                                                          && inv.EmailAddress == expectedInvitation.EmailAddress
                                                                          && inv.SourceId == expectedInvitation.SourceId
                                                                          && inv.GroupRoleId == expectedInvitation.GroupRoleId
                                                                          && inv.InvitationType == expectedInvitation.InvitationType),
                                                             It.Is<string>((s) => s == token))).Returns(expectedInvitation);
            _mpFinderRepository.Setup(x => x.RecordConnection(It.IsAny<MpConnectCommunication>()));
            _mpConfigurationWrapper.Setup(x => x.GetConfigIntValue(It.IsAny<string>())).Returns(1);
            _mpContactRepository.Setup(x => x.GetContactIdByEmail(It.IsAny<string>())).Returns(2);
            _mpContactRepository.Setup(x => x.GetContactId(It.IsAny<string>())).Returns(3);
            _fixture.InviteToGroup(token, gatheringId, person, "SMALL_GROUP");
            _invitationService.VerifyAll();
        }

        private FinderPinDto convertPinDtoToFinderPinDto(PinDto pinDto)
        {
            return Mapper.Map<FinderPinDto>(pinDto);
        }

        private PinDto GetAPin(int designator = 1)
        {
            return new PinDto()
            {
                Gathering = new FinderGroupDto
                {
                    GroupId = designator * 10,
                    Address = this.getAnAddress(designator * 10),
                    ContactId = designator,
                    AttributeTypes = null,
                    ChildCareAvailable = false,
                    AvailableOnline = true,
                    Congregation = "CongWoot",
                    CongregationId = designator,
                    EndDate = null,
                    StartDate = DateTime.Now,
                    GroupName = $"Group {designator}",
                    PrimaryContactEmail = $"{designator}Guy@compuserv.net",
                    Events = null,
                    GroupDescription = "Best gathering",
                    GroupFullInd = false,
                    GroupRoleId = 26,
                    GroupTypeId = 30,
                    GroupTypeName = "Anywhere Gathering",
                    KidsWelcome = false,
                    MaximumAge = 0,
                    MeetingDay = null,
                    MeetingDayId = null,
                    MeetingFrequency = null,
                    MeetingFrequencyID = null,
                    MeetingTime = null,
                    MinimumParticipants = 0,
                    MinistryId = 1,
                    MinorAgeGroupsAdded = false,
                    OnlineRsvpMinimumAge = 0,
                    ParticipantId = null,
                    Participants = null,
                    PrimaryContactName = "Dudeman",
                    Proximity = null,
                    ReasonEndedId = null,
                    RemainingCapacity = 0,
                    SignUpFamilyMembers = null
                },
                Contact_ID = designator,
                Address = this.getAnAddress(designator),
                Proximity = null,
                EmailAddress = $"{designator}1Guy@compuserve.net",
                FirstName = $"{designator}Guy",
                LastName = "Lastname",
                Host_Status_ID = 3,
                Household_ID = null,
                Participant_ID = designator,
                PinType = PinType.GATHERING,
                ShowOnMap = true,
                SiteName = "Anywheres",
                ShouldUpdateHomeAddress = false
            };
        }

        private MpAddress getAMpAddress(int designator = 1)
        {
            return new MpAddress()
            {
                Address_ID = designator,
                Address_Line_1 = $"{designator} Street",
                Address_Line_2 = $"Apt {designator}",
                City = "City!",
                County = "County!",
                Foreign_Country = "USA",
                Latitude = 0,
                Longitude = 0,
                Postal_Code = "12345",
                State = "Ohio"
            };
        }

        private AddressDTO getAnAddress(int designator = 1)
        {
            return new AddressDTO()
            {
                AddressID = designator,
                AddressLine1 = $"{designator} street",
                AddressLine2 = null,
                City = "City!",
                County = "County",
                ForeignCountry = "USA",
                Latitude = 0,
                Longitude = 0,
                PostalCode = "12345",
                State = "Ohio"
            };
        }

        [Test]
        public void ShouldSendEmailToAddedUser()
        {
        var person = new User()
            {
                firstName = "albert",
                lastName = "einstein",
                email = "ae@g.com"
            };

            var token = "abc";
            var gatheringId = 12345;
            var emailTemplate = new MpMessageTemplate
            {
                FromContactId = 234,
                FromEmailAddress = "ae@g.com",
                ReplyToContactId = 456,
                ReplyToEmailAddress = "ss@g.com"
            };

            var leaderContact = new MpMyContact
            {
                Email_Address = "in@g.com",
                First_Name = "albert",
                Last_Name = "Einstein"
            };

            var groupAddress = new AddressDTO();
            groupAddress.AddressLine1 = "333";
            groupAddress.AddressLine2 = "vine";
            groupAddress.City = "Cin";
            groupAddress.State = "OH";
            groupAddress.PostalCode = "3455";

            var group = new GroupDTO();
            group.GroupName = "Physics";
            group.ContactId = 1;
            group.MeetingDayId = 1;
            group.MeetingFrequencyID = 1;
            group.MeetingTime = "0001-01-01T05:25:00.000Z";
            group.Address = groupAddress;

            var mpParticpant = new MpParticipant();
            mpParticpant.ContactId = 1;
            mpParticpant.ParticipantId = 3;

            var groupList = new List<GroupDTO>();
            groupList.Add(group);

            _mpConfigurationWrapper.Setup(x => x.GetConfigIntValue("GroupsAddParticipantEmailNotificationTemplateId")).Returns(1);
            _communicationRepository.Setup(x => x.GetTemplate(1)).Returns(emailTemplate);
            _mpContactRepository.Setup(x => x.GetContactId(It.IsAny<string>())).Returns(3);
            _mpContactRepository.Setup(x => x.GetContactById(3)).Returns(leaderContact);
            _mpContactRepository.Setup(x => x.GetContactIdByEmail(It.IsAny<string>())).Returns(2);
            _groupService.Setup(x => x.GetGroupDetails(It.IsAny<int>())).Returns(group);
            _lookupService.Setup(x => x.GetMeetingDayFromId(It.IsAny<int>())).Returns("Friday");
            _mpParticipantRepository.Setup(x => x.GetParticipantRecord(It.IsAny<string>())).Returns(mpParticpant);
            _groupService.Setup(x => x.GetGroupsByTypeOrId(It.IsAny<string>(), It.IsAny<int>(), null, It.IsAny<int>(),true, false)).Returns(groupList);
            _fixture.SendEmailToAddedUser( token, person, gatheringId );
            _communicationRepository.Verify(x => x.SendMessage(It.IsAny<MinistryPlatform.Translation.Models.MpCommunication>(), false), Times.Once);
            
        }
    }
}