using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using Crossroads.Utilities.Interfaces;

using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Finder;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Repositories;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Test.Helpers;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class FinderRepositoryTest
    {
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRestRepository;
        private Mock<IConfigurationWrapper> _config;
        private Mock<IApiUserRepository> _apiUserRepo;

        private FinderRepository _fixture;


        [SetUp]
        public void SetUp()
        {
            _ministryPlatformRestRepository = new Mock<IMinistryPlatformRestRepository>();
            _config = new Mock<IConfigurationWrapper>();
            _apiUserRepo = new Mock<IApiUserRepository>();

            _ministryPlatformRestRepository.Setup(m => m.UsingAuthenticationToken("abc")).Returns(_ministryPlatformRestRepository.Object);


            _fixture = new FinderRepository(_config.Object, _ministryPlatformRestRepository.Object, _apiUserRepo.Object);
        }

        [Test]
        public void getFinderPinDetails()
        {
            var participantId = 123;

            var response = new FinderPinDto
            {
                Address = null,
                LastName = "Kerstanoff",
                Contact_ID = 12,
                EmailAddress = "JoeKer@gmail.com",
                FirstName = "Joe",
                Household_ID = 2,
                Participant_ID = 123
            };
            var addressResponse = new MpAddress {Address_ID = 1, Address_Line_1 = "123 street", City = "City!", Postal_Code = "12345"};
            _apiUserRepo.Setup(m => m.GetToken()).Returns("abc");
            _ministryPlatformRestRepository.Setup(
                mocked =>
                    mocked.Search<FinderPinDto>(
                        It.Is<string>(
                            m =>
                                m.Equals(
                                    $"Participant_Record = {participantId}")),
                        It.Is<string>(
                            m =>
                                m.Equals(
                                    "Email_Address, Nickname as FirstName, Last_Name as LastName, Participant_Record_Table.*, Household_ID")),
                        It.IsAny<string>(),
                        It.IsAny<bool>())).Returns(new List<FinderPinDto> { response });

            _ministryPlatformRestRepository.Setup(
                mocked =>
                    mocked.Search<MpAddress>(
                        It.Is<string>(
                            m =>
                                m.Equals(
                                    $"Participant_Record = {participantId}")),
                        It.Is<string>(
                            m =>
                                m.Equals(
                                    "Household_ID_Table_Address_ID_Table.*")),
                        It.IsAny<string>(),
                        It.IsAny<bool>())).Returns(new List<MpAddress> { addressResponse });

            var value = _fixture.GetPinDetails(123);
            _ministryPlatformRestRepository.VerifyAll();

            Assert.AreEqual(value.LastName, response.LastName);
            Assert.AreEqual(value.Address.Address_ID, response.Address.Address_ID);

        }
    }
}
