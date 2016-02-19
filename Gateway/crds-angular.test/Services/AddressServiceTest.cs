using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using MPServices = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    class AddressServiceTest
    {

        private AddressService _fixture;
        private Mock<MPServices.IAddressService> _mpAddressServiceMock;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _mpAddressServiceMock = new Mock<MPServices.IAddressService>(MockBehavior.Strict);

            _fixture = new AddressService(_mpAddressServiceMock.Object);            
        }

        [Test]
        public void Given_An_Address_That_Exists_It_Should_Return_The_Existing_Address()
        {
            var address = new AddressDTO()
            {
                AddressLine1 = "123 Sesame St",
                AddressLine2 = "",
                City = "South Side",
                State = "OH",               
                PostalCode = "12312"
            };

            var addressResults = new List<Address>()
            {
                new Address()
                {
                    Address_ID = 12345
                },
                new Address()
                {
                    Address_ID = 232323
                }
            };

            _mpAddressServiceMock.Setup(mocked => mocked.FindMatchingAddresses(It.IsAny<Address>())).Returns(addressResults);

            _fixture.FindOrCreateAddress(address);

            _mpAddressServiceMock.Verify(x => x.FindMatchingAddresses(It.IsAny<Address>()), Times.Once);
            _mpAddressServiceMock.Verify(x => x.Create(It.IsAny<Address>()), Times.Never);
            Assert.AreEqual(address.AddressID, 12345);
        }

        [Test]
        public void Given_An_Address_That_Does_Not_Exist_It_Should_Create_A_New_Address()
        {
            var address = new AddressDTO()
            {
                AddressLine1 = "123 Sesame St",
                AddressLine2 = "",
                City = "South Side",
                State = "OH",
                PostalCode = "12312"
            };

            var addressResults = new List<Address>();

            _mpAddressServiceMock.Setup(mocked => mocked.FindMatchingAddresses(It.IsAny<Address>())).Returns(addressResults);
            _mpAddressServiceMock.Setup(mocked => mocked.Create(It.IsAny<Address>())).Returns(12345);

            _fixture.FindOrCreateAddress(address);

            _mpAddressServiceMock.Verify(x => x.FindMatchingAddresses(It.IsAny<Address>()), Times.Once);
            _mpAddressServiceMock.Verify(x => x.Create(It.IsAny<Address>()), Times.Once);
            
            Assert.AreEqual(address.AddressID, 12345);
        }
    }
}
