﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
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
        private readonly int _addressPageId = 271;
        private readonly int _addressApiPageViewId = 271;


        [SetUp]
        public void Setup()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _apiUserService = new Mock<IApiUserRepository>();
            _apiUserService.Setup(m => m.GetToken()).Returns("useme");
            _configuration = new Mock<IConfigurationWrapper>();
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Addresses")).Returns(_addressPageId);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("AddressesApiPageView")).Returns(_addressApiPageViewId);
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

            _ministryPlatformService.Setup(m => m.CreateRecord(_addressPageId, It.IsAny<Dictionary<string, object>>(), apiToken, false)).Returns(addressId);

            int addrId = _fixture.Create(addr);
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(_addressPageId, values, "useme", false));

            Assert.IsNotNull(addrId);
            Assert.AreEqual(addressId, addrId);
        }

        [Test]
        public void UpdateAddress()
        {
            const string apiToken = "useme";
            const int addressId = 785645;

            var addr = new MpAddress
            {
                Address_ID = addressId,
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

            _ministryPlatformService.Setup(m => m.UpdateRecord(271, It.IsAny<Dictionary<string, object>>(), apiToken));

            var addrId = _fixture.Update(addr);
            _ministryPlatformService.Verify(
                mocked =>
                    mocked.UpdateRecord(_addressPageId,
                                        It.Is<Dictionary<string, object>>(
                                            d =>
                                                d["Address_ID"].Equals(addr.Address_ID) &&
                                                d["Address_Line_1"].Equals(addr.Address_Line_1) &&
                                                d["Address_Line_2"].Equals(addr.Address_Line_2) &&
                                                d["City"].Equals(addr.City) &&
                                                d["State/Region"].Equals(addr.State) &&
                                                d["Postal_Code"].Equals(addr.Postal_Code) &&
                                                d["Foreign_Country"].Equals(addr.Foreign_Country) &&
                                                d["County"].Equals(addr.County) &&
                                                d["Longitude"].Equals(addr.Longitude) &&
                                                d["Latitude"].Equals(addr.Latitude)
                                            ),
                                        "useme"));

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
                    {"County", "Hamilton"},
                    {"Longitude", "123.45"},
                    {"Latitude", "678.90"},
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
                County = "Hamilton",
                Longitude = 123.45,
                Latitude = 678.90
            };

            _ministryPlatformService.Setup(m => m.GetPageViewRecords(_addressApiPageViewId, It.IsAny<string>(), It.IsAny<string>(), string.Empty, 0)).Returns(addrRecords);

            var records = _fixture.FindMatches(addr);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(records);
            Assert.AreEqual(addr.Address_ID , records[0].Address_ID);
            Assert.AreEqual(addr.Address_Line_1, records[0].Address_Line_1);
            Assert.AreEqual(addr.Address_Line_2, records[0].Address_Line_2);
            Assert.AreEqual(addr.City, records[0].City);
            Assert.AreEqual(addr.State, records[0].State);
            Assert.AreEqual(addr.Postal_Code, records[0].Postal_Code);
            Assert.AreEqual(addr.Longitude, records[0].Longitude);
            Assert.AreEqual(addr.Latitude, records[0].Latitude);
        }

    }
}
