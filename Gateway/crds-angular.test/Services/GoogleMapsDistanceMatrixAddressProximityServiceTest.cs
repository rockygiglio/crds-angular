using System.Collections.Generic;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using GoogleMapsAPI.NET.API.Client;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    /// <summary>
    /// Integration tests for the GoogleMapsDistanceMatrixAddressProximityService.  It is not currently possible to write true unit tests against this service, as the Google Maps .NET API is not "mockable".
    /// </summary>
    [Category("IntegrationTests")]
    public class GoogleMapsDistanceMatrixAddressProximityServiceTest
    {
        private GoogleMapsDistanceMatrixAddressProximityService _fixture;
        private IAddressGeocodingService _addressGeocodingService; // TODO: Replace with mock when updating these tests
        private MapsAPIClient _mapsApiClient;

        [SetUp]
        public void SetUp()
        {
            _mapsApiClient = new MapsAPIClient(new ConfigurationWrapper().GetEnvironmentVarAsString("GOOGLE_API_SECRET_KEY"));

            //_fixture = new GoogleMapsDistanceMatrixAddressProximityService(_mapsApiClient);
            _fixture = new GoogleMapsDistanceMatrixAddressProximityService(_mapsApiClient, _addressGeocodingService);
        }

        [Test]
        [Category("IntegrationTests")]
        public void TestGetGeoCoordinatesStringAddress()
        {
            const double expectedLatitude = 39.15946159999999;
            const double expectedLongitude = -84.42336390000003;
            const string addressString = "3500 Madison Rd, Cincinnati, OH 45209, USA";
            var result = _fixture.GetGeoCoordinates(addressString);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLatitude, result.Latitude, .009);
            Assert.AreEqual(expectedLongitude, result.Longitude, .009);
        }

        [Test]
        [Category("IntegrationTests")]
        public void TestGetGeoCoordinatesAddressObject()
        {
            const double expectedLatitude = 39.15946159999999;
            const double expectedLongitude = -84.42336390000003;
            var address = new AddressDTO
            {
                AddressLine1 = "3500 Madison Rd",
                City = "Cincinnati",
                State = "OH",
                PostalCode = "45209"
            };
            var result = _fixture.GetGeoCoordinates(address);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLatitude, result.Latitude, .009);
            Assert.AreEqual(expectedLongitude, result.Longitude, .009);
        }

        [Test]
        [Category("IntegrationTests")]
        public void TestGetProximityNoDestinationCoords()
        {
            var address = new AddressDTO
            {
                AddressLine1 = "3500 Madison Rd",
                City = "Cincinnati",
                State = "OH",
                PostalCode = "45209"
            };
            var result = _fixture.GetProximity("990 Reading Rd, Mason, OH 45040", new List<AddressDTO> { address });
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsNull(result[0]);
        }

        [Test]
        [Category("IntegrationTests")]
        [ExpectedException(typeof(InvalidAddressException))]
        public void TestGetProximityBadOrigin()
        {
            var address = new AddressDTO
            {
                AddressLine1 = "3500 Madison Rd",
                City = "Cincinnati",
                State = "OH",
                PostalCode = "45209",
                Latitude = 39.15946159999999,
                Longitude = -84.42336390000003
            };
            var result = _fixture.GetProximity("1234 test st, testy mctestface, te, 99999-9999, xyz", new List<AddressDTO> { address });
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsNull(result[0]);
        }

        [Test]
        [Category("IntegrationTests")]
        public void TestGetProximity()
        {
            var address = new AddressDTO
            {
                AddressLine1 = "3500 Madison Rd",
                City = "Cincinnati",
                State = "OH",
                PostalCode = "45209",
                Latitude = 39.15946159999999,
                Longitude = -84.42336390000003
            };
            var result = _fixture.GetProximity("990 Reading Rd, Mason, OH 45040", new List<AddressDTO> { address });
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
            Assert.AreEqual(13.3, (double)result[0], .009);
        }

        //[Test]
        //[Category("IntegrationTests")]
        //public void TestValidateAddressString()
        //{
        //    const string address = "3500 Madison Rd, Cincinnati, OH 45209, USA";

        //    var result = _fixture.ValidateAddress(address);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(39.15946159999999, result.Latitude, .009);
        //    Assert.AreEqual(-84.42336390000003, result.Longitude, .009);
        //}

        //[Test]
        //[Category("IntegrationTests")]
        //[ExpectedException(typeof(InvalidAddressException))]
        //public void TestValidateAddressStringInvalidAddress()
        //{
        //    const string address = "1234 testy mctestface, test, yy 99999-9999";

        //    _fixture.ValidateAddress(address);
        //}

        //[Test]
        //[Category("IntegrationTests")]
        //public void TestValidateAddressObject()
        //{
        //    var address = new AddressDTO
        //    {
        //        AddressLine1 = "3500 Madison Rd",
        //        City = "Cincinnati",
        //        State = "OH",
        //        PostalCode = "45209"
        //    };

        //    var result = _fixture.ValidateAddress(address);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(39.15946159999999, result.Latitude, .009);
        //    Assert.AreEqual(-84.42336390000003, result.Longitude, .009);
        //}

        //[Test]
        //[Category("IntegrationTests")]
        //[ExpectedException(typeof(InvalidAddressException))]
        //public void TestValidateAddressObjectInvalidAddress()
        //{
        //    var address = new AddressDTO
        //    {
        //        AddressLine1 = "1234 testy mctestface",
        //        City = "test",
        //        State = "yy",
        //        PostalCode = "99999-9999"
        //    };

        //    _fixture.ValidateAddress(address);
        //}
    }
}
