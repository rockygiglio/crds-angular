using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Models;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class AddressServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IApiUserRepository> _apiUserService;
        private Mock<IConfigurationWrapper> _configuration;
        private AddressRepository _fixture;
        private readonly int _addressPageId;


        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _apiUserService = new Mock<IApiUserRepository>();
            _apiUserService.Setup(m => m.GetToken()).Returns("useme");
            _configuration = new Mock<IConfigurationWrapper>();
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Addresses")).Returns(271);
            _fixture = new AddressRepository(_configuration.Object,_ministryPlatformService.Object, _apiUserService.Object);
        }

        [Test]
        public void CreateAddress()
        {
            const string apiToken = "useme";
            const int addressId = 785645;

            var addr = new MpAddress()
            {
                Address_Line_1 = "321 Road Ln",
                Address_Line_2 = "Suite 100",
                City = "Madison",   
                State = "OH",
                Postal_Code = "45454",
                Foreign_Country = "USA",
                County = "Hamilton",
                Longitude = 123.45,
                Latitude = 678.90
            };

            var values = new Dictionary<string, object>
            {
                {"Address_Line_1", "321 Road Ln"},
                {"Address_Line_2", "Suite 100"},
                {"City", "Madison"},
                {"State/Region", "OH"},
                {"Postal_Code", "45454"},
                {"Foreign_Country", "USA"},
                {"County", "Hamilton"},
                {"Longitude", addr.Longitude },
                {"Latitude", addr.Latitude }
            };

            _ministryPlatformService.Setup(m => m.CreateRecord(271, It.IsAny<Dictionary<string, object>>(), apiToken, false)).Returns(addressId);

            int addrId = _fixture.Create(addr);
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(271, values, "useme", false));

            Assert.IsNotNull(addrId);
            Assert.AreEqual(addressId, addrId);
        }

        [Test]
        public void FindMatches()
        {
           var addrRecords = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    {"dp_RecordID", 12345},
                    {"Address_Line_1", "321 Road Ln"},
                    {"Address_Line_2", "Suite 100"},
                    {"City", "Madison"},
                    {"State/Region", "OH"},
                    {"Postal_Code", "45454"},
                    {"Foreign_Country", "USA"},
                    {"County", "Hamilton"}
                }
            };

            var addr = new MpAddress()
            {
                Address_ID = 12345,
                Address_Line_1 = "321 Road Ln",
                Address_Line_2 = "Suite 100",
                City = "Madison",
                State = "OH",
                Postal_Code = "45454",
                Foreign_Country = "USA",
                County = "Hamilton"
            };

            _ministryPlatformService.Setup(m => m.GetRecordsDict(271, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(addrRecords);

            var records = _fixture.FindMatches(addr);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(records);
            Assert.AreEqual(addr.Address_ID , records[0].Address_ID);
            Assert.AreEqual(addr.Address_Line_1, records[0].Address_Line_1);
            Assert.AreEqual(addr.Address_Line_2, records[0].Address_Line_2);
            Assert.AreEqual(addr.City, records[0].City);
            Assert.AreEqual(addr.State, records[0].State);
            Assert.AreEqual(addr.Postal_Code, records[0].Postal_Code);
        }

    }
}
