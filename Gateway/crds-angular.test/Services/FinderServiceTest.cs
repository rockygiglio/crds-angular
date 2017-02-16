using System.Collections.Generic;
using System.Linq;
using crds_angular.App_Start;
using crds_angular.Services;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Finder;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class FinderServiceTest
    {
        private FinderService _fixture;
        private Mock<IFinderRepository> _mpFinderRepository;
        private Mock<IParticipantRepository> _mpParticipantRepository;

        [SetUp]
        public void SetUp()
        {
            _mpFinderRepository = new Mock<IFinderRepository>();
            _mpParticipantRepository = new Mock<IParticipantRepository>();


            _fixture = new FinderService(_mpFinderRepository.Object, _mpParticipantRepository.Object);
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
    }
}
