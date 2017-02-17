using System.Collections.Generic;
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

namespace crds_angular.test.Services
{
    [TestFixture]
    public class FinderServiceTest
    {
        private FinderService _fixture;
        private Mock<IFinderRepository> _mpFinderRepository;
        private Mock<IContactRepository> _mpContactRepository;
        private Mock<IAddressService>_addressService;
        private Mock<IParticipantRepository> _mpParticipantRepository;
        private Mock<IAddressService> _mpAddressService;

        [SetUp]
        public void SetUp()
        {
            _mpFinderRepository = new Mock<IFinderRepository>();
            _mpContactRepository = new Mock<IContactRepository>();
            _addressService = new Mock<IAddressService>();
            _mpParticipantRepository = new Mock<IParticipantRepository>();
            _mpAddressService = new Mock<IAddressService>();

            _fixture = new FinderService(_mpFinderRepository.Object, _mpContactRepository.Object, _addressService.Object, _mpParticipantRepository.Object);

            _fixture = new FinderService(_mpFinderRepository.Object, _mpParticipantRepository.Object, _mpAddressService.Object);
            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void ShouldGetPinDetails()
        {
            _mpFinderRepository.Setup(m => m.GetPinDetails(123))
                .Returns(new FinderPinDto
                         {
                             LastName = "Ker",
                             FirstName = "Joe",
                             Address = new MpAddress {Address_ID = 12, Postal_Code = "1234", Address_Line_1 = "123 street", City = "City", State = "OH"},
                             Participant_ID = 123,
                             EmailAddress = "joeker@gmail.com",
                             Contact_ID = 22,
                             Household_ID = 13
                         });

            var result = _fixture.GetPinDetails(123);

            _mpFinderRepository.VerifyAll();

            Assert.AreEqual(result.LastName, "Ker");
            Assert.AreEqual(result.Address.AddressID, 12);
        }

        [Test]
        public void ShouldEnablePin()
        {
            _mpFinderRepository.Setup(m => m.EnablePin(123));
            _fixture.EnablePin(123);
            _mpFinderRepository.VerifyAll();
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
                Host_Status = 0
            };

            var address = Mapper.Map<MpAddress>(pin.Address);            
            var addressDictionary = new Dictionary<string, object>
            {
                { "AddressID", pin.Address.AddressID },
                { "AddressLine1", pin.Address.AddressID },
                { "City", pin.Address.AddressID },
                { "State/Region", pin.Address.AddressID },
                { "PostCode", pin.Address.AddressID }
            };
            var householdDictionary = new Dictionary<string, object> { { "Household_ID", pin.Household_ID} };

            _addressService.Setup(m => m.SetGeoCoordinates(pin.Address));
            _mpContactRepository.Setup(m => m.UpdateHouseholdAddress(pin.Household_ID, householdDictionary, addressDictionary));
            _fixture.UpdateHouseholdAddress(pin);
            _mpFinderRepository.VerifyAll();
        }
    }
}
