using System;
using System.Linq;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class AddressService : IAddressService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserService _apiUserService;
        private readonly int AddressPageId;

        public AddressService(IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            _ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
            AddressPageId = configurationWrapper.GetConfigIntValue("Addresses");
        }

        public int Create(Address address)
        {
            var apiToken = _apiUserService.GetToken();

            var values = new Dictionary<string, object>()
            {
                {"Address_Line_1", address.Address_Line_1},
                {"Address_Line_2", address.Address_Line_2},
                {"City", address.City},
                {"State/Region", address.State},
                {"Postal_Code", address.Postal_Code},
                {"Foreign_Country", address.Foreign_Country},
                {"County", address.County},
            };

            var addressId = _ministryPlatformService.CreateRecord(AddressPageId, values, apiToken);

            return addressId;
        }

        public List<Address> FindMatchingAddresses(Address address)
        {
            var apiToken = _apiUserService.GetToken();
            var search = string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                                       AddQuotesIfNotEmpty(address.Address_Line_1),
                                       AddQuotesIfNotEmpty(address.Address_Line_2),
                                       AddQuotesIfNotEmpty(address.City),
                                       AddQuotesIfNotEmpty(address.State),
                                       AddQuotesIfNotEmpty(address.Postal_Code),
                                       AddQuotesIfNotEmpty(address.Foreign_Country));

            var records = _ministryPlatformService.GetRecordsDict(AddressPageId, apiToken, search);

            var addresses = records.Select(record => new Address()
            {
                Address_ID = record.ToInt("dp_RecordID"),
                Address_Line_1 = record.ToString("Address_Line_1"),
                Address_Line_2 = record.ToString("Address_Line_2"),
                City = record.ToString("City"),
                State = record.ToString("State/Region"),
                Postal_Code = record.ToString("Postal_Code"),
                Foreign_Country = record.ToString("Foreign_Country"),
            }).ToList();

            return addresses;
        }

        private string AddQuotesIfNotEmpty(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            return string.Format("\"{0}\"", input);
        }
    }
}