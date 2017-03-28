using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Models.Finder;
using crds_angular.Models.Json;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.Security;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class FinderControllerTest
    {
        private FinderController _fixture;

        private Mock<IFinderService> _finderService;
        private Mock<IAddressService> _addressService;
        private Mock<IAddressGeocodingService> _addressGeocodingService;

        private Mock<IConfigurationWrapper> _configurationWrapper;

        private const string AuthType = "abc";
        private const string AuthToken = "123";
        private readonly string _auth = string.Format("{0} {1}", AuthType, AuthToken);

        [SetUp]
        public void SetUp()
        {
            _finderService = new Mock<IFinderService>();
            _addressService = new Mock<IAddressService>();
            _addressGeocodingService = new Mock<IAddressGeocodingService>();
            //_configurationWrapper = new Mock<IConfigurationWrapper>();
            // _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("AnywhereGroupTypeId")).Returns(1);

            _fixture = new FinderController(_addressService.Object,
                                            _addressGeocodingService.Object,
                                            _finderService.Object,
                                            new Mock<IUserImpersonationService>().Object,
                                            new Mock<IAuthenticationRepository>().Object);

            _fixture.SetupAuthorization(AuthType, AuthToken);
        }

        [Test]
        public void TestGetMyPinsByContactIdWithResults()
        {
            int contactId = 123;
            string lat = "80.15";
            string lng = "40.25";

            var pinsForContact = new List<PinDto>
            {
                new PinDto
                {
                    Address = new AddressDTO
                    {
                        AddressID = 741,
                        AddressLine1 = "456 Happy Ave",
                        City = "Cincinnati",
                        State = "OH",
                        PostalCode = "45208",
                        Latitude = 80.15,
                        Longitude = 40.25
                    },
                    Contact_ID = 123,
                    Participant_ID = 456,
                    Household_ID = 789,
                    EmailAddress = "",
                    FirstName = "",
                    LastName = "",
                    Gathering = new GroupDTO(),
                    Host_Status_ID = 0,
                    PinType = PinType.GATHERING
                },
                new PinDto
                {
                    Address = new AddressDTO
                    {
                        AddressID = 741,
                        AddressLine1 = "123 Main Street",
                        City = "Cincinnati",
                        State = "OH",
                        PostalCode = "45249",
                        Latitude = 80.15,
                        Longitude = 40.25
                    },
                    Contact_ID = 123,
                    Participant_ID = 456,
                    Household_ID = 789,
                    EmailAddress = "",
                    FirstName = "",
                    LastName = "",
                    Gathering = null,
                    Host_Status_ID = 0,
                    PinType = PinType.PERSON
                }
            };

            var randomAddress = new AddressDTO
            {
                AddressID = 741,
                AddressLine1 = "456 Happy Ave",
                City = "Cincinnati",
                State = "OH",
                PostalCode = "45208",
                Latitude = 80.15,
                Longitude = 40.25
            };

            _finderService.Setup(mocked => mocked.GetGeoCoordsFromLatLong(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GeoCoordinate(80.15, 40.25));
            _finderService.Setup(mocked => mocked.GetMyPins(_auth, new GeoCoordinate(80.15, 40.25), contactId)).Returns(pinsForContact);
            _finderService.Setup(mocked => mocked.RandomizeLatLong(randomAddress)).Verifiable();

            var pinsForContactSearchResults = new PinSearchResultsDto(new GeoCoordinates(80.15, 40.25), pinsForContact);

            var result = _fixture.GetMyPinsByContactId(contactId, lat, lng);
            var restResult = (OkNegotiatedContentResult<PinSearchResultsDto>)result;

            Assert.IsNotNull(result);
            
            restResult.Content.ShouldBeEquivalentTo(pinsForContactSearchResults); // need to remove becuase fluentassertions is not in crds angular test
            Assert.Equals(pinsForContactSearchResults, restResult.Content);
            // Assert.AreEqual(pinsForContactSearchResults, restResult.Content);
            //Assert.AreSame(pinsForContactSearchResults, restResult.Content);
        }

        [Test]
        public void TestGetMyPinsByContactIdNoResults()
        {

        }

    }
}
