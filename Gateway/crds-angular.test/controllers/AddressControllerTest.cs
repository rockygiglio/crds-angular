using System.Net;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class AddressControllerTest
    {
        private AddressController _fixture;
        private Mock<IAddressGeocodingService> _addressGeocodingService;
        [SetUp]
        public void SetUp()
        {
            _addressGeocodingService = new Mock<IAddressGeocodingService>(MockBehavior.Strict);
            _fixture = new AddressController(_addressGeocodingService.Object);
        }

        [Test]
        public void TestValidateAddressString()
        {
            const string address = "Valid address";
            var addressDto = new AddressDTO();
            _addressGeocodingService.Setup(mocked => mocked.ValidateAddress(address)).Returns(addressDto);

            var result = _fixture.ValidateAddress(address);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<AddressDTO>>(result);
            var okResult = (OkNegotiatedContentResult<AddressDTO>)result;
            Assert.AreSame(addressDto, okResult.Content);
        }

        [Test]
        public void TestValidateAddressStringInvalidAddress()
        {
            const string address = "Invalid address";
            var ex = new InvalidAddressException();
            _addressGeocodingService.Setup(mocked => mocked.ValidateAddress(address)).Throws(ex);

            var result = _fixture.ValidateAddress(address);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResponseMessageResult>(result);
            var responseMessageResult = (ResponseMessageResult)result;
            Assert.AreEqual(HttpStatusCode.NotFound, responseMessageResult.Response.StatusCode);
        }

        [Test]
        public void TestValidateAddressObject()
        {
            var address = new AddressDTO
            {
                AddressLine1 = "Valid address",
                PostalCode = "12345"
            };
            var addressDto = new AddressDTO();
            _addressGeocodingService.Setup(mocked => mocked.ValidateAddress(address.ToString())).Returns(addressDto);

            var result = _fixture.ValidateAddress(address);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<AddressDTO>>(result);
            var okResult = (OkNegotiatedContentResult<AddressDTO>)result;
            Assert.AreSame(addressDto, okResult.Content);
        }

        [Test]
        public void TestValidateAddressObjectInvalidAddress()
        {
            var address = new AddressDTO
            {
                AddressLine1 = "Invalid address",
                PostalCode = "12345"
            };
            var ex = new InvalidAddressException();
            _addressGeocodingService.Setup(mocked => mocked.ValidateAddress(address.ToString())).Throws(ex);

            var result = _fixture.ValidateAddress(address);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResponseMessageResult>(result);
            var responseMessageResult = (ResponseMessageResult)result;
            Assert.AreEqual(HttpStatusCode.NotFound, responseMessageResult.Response.StatusCode);
        }

    }
}
