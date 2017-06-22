﻿using System.Collections.Generic;
using System.Device.Location;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Finder;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Security;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class FinderControllerTest
    {
        private FinderController _fixture;

        private Mock<IAddressService> _addressService;
        private Mock<IAddressGeocodingService> _addressGeocodingService;
        private Mock<IFinderService> _finderService;
        private Mock<IUserImpersonationService> _userImpersonationService;
        private Mock<IAuthenticationRepository> _authenticationRepository;
        private Mock<IAwsCloudsearchService> _awsCloudsearchService;
        private string _authToken;
        private string _authType;

        [SetUp]
        public void SetUp()
        {
            _finderService = new Mock<IFinderService>();
            _userImpersonationService = new Mock<IUserImpersonationService>();
            _authenticationRepository = new Mock<IAuthenticationRepository>();
            _awsCloudsearchService = new Mock<IAwsCloudsearchService>();

            _authType = "authType";
            _authToken = "authToken";

            _fixture = new FinderController(_finderService.Object,
                                            _userImpersonationService.Object,
                                            _authenticationRepository.Object,
                                            _awsCloudsearchService.Object)
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext()
            };

            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(_authType, _authToken);
        }

        [Test]
        public void TestObjectInstantiates()
        {
            Assert.IsNotNull(_fixture);
        }

        [Test]
        public void TestGetMyPinsByContactIdWithResults()
        {
            var fakeQueryParams = new PinSearchQueryParams();
            fakeQueryParams.CenterGeoCoords = new GeoCoordinates(39.123, -84.456);
            fakeQueryParams.ContactId = 12345;
            fakeQueryParams.FinderType = "CONNECT";
            var geoCoordinate = new GeoCoordinate(39.123, -84.456);
            var listPinDto = GetListOfPinDto();
            var address = new AddressDTO("123 Main st","","Independence","KY","41051",32,-84);

            _finderService.Setup(m => m.GetGeoCoordsFromAddressOrLatLang(It.IsAny<string>(), It.IsAny<GeoCoordinates>())).Returns(geoCoordinate);
            _finderService.Setup(m => m.GetMyPins(It.IsAny<string>(), It.IsAny<GeoCoordinate>(), It.IsAny<int>(), It.IsAny<string>())).Returns(listPinDto);
            _finderService.Setup(m => m.RandomizeLatLong(It.IsAny<AddressDTO>())).Returns(address);

            var response = _fixture.GetMyPinsByContactId(fakeQueryParams);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<PinSearchResultsDto>>(response);
        }

        [Test]
        public void TestGetMyPinsByContactIdReturnsNothing()
        {

            var fakeQueryParams = new PinSearchQueryParams();
            fakeQueryParams.CenterGeoCoords = new GeoCoordinates(39.123, -84.456);
            fakeQueryParams.ContactId = 12345;
            fakeQueryParams.FinderType = "CONNECT";
            var geoCoordinate = new GeoCoordinate(39.123, -84.456);

            _finderService.Setup(m => m.GetGeoCoordsFromAddressOrLatLang(It.IsAny<string>(), It.IsAny<GeoCoordinates>())).Returns(geoCoordinate);
            _finderService.Setup(m => m.GetMyPins(It.IsAny<string>(), It.IsAny<GeoCoordinate>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new List<PinDto>());

            var response = _fixture.GetMyPinsByContactId(fakeQueryParams) as OkNegotiatedContentResult<PinSearchResultsDto>;
            Assert.That(response != null && response.Content.PinSearchResults.Count == 0);
        }

        [Test]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestNotAuthorized()
        {
            _authenticationRepository.Setup(mocked => mocked.GetContactId("abc")).Returns(123456);
            _fixture.EditGatheringPin(GetListOfPinDto()[0]);
        }

        private static List<PinDto> GetListOfPinDto()
        {
            var list = new List<PinDto>();

            var addr1 = new AddressDTO
            {
                Latitude = 30.1,
                Longitude = -80.1
            };
            var pin1 = new PinDto
            {
                Contact_ID = 1,
                EmailAddress = "pin1@fake.com",
                FirstName = "pinhead",
                LastName = "One",
                Address = addr1
            };

            var addr2 = new AddressDTO
            {
                Latitude = 30.2,
                Longitude = -80.2
            };
            var pin2 = new PinDto
            {
                Contact_ID = 2,
                EmailAddress = "pin2@fake.com",
                FirstName = "pinhead",
                LastName = "Two",
                Address = addr2
            };
            list.Add(pin1);
            list.Add(pin2);
            return list;
        }

    }
}
