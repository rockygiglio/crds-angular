using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;

namespace crds_angular.Services
{
    public class AddressService : IAddressService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IAddressService _mpAddressService;

        public AddressService(MinistryPlatform.Translation.Services.Interfaces.IAddressService mpAddressService)
        {
            _mpAddressService = mpAddressService;
        }

        public void FindOrCreateAddress(AddressDTO address)
        {
            var mpAddress = AutoMapper.Mapper.Map<Address>(address);
            var found = FindExistingAddress(address, mpAddress);
            if (found)
            {
                return;
            } 

            address.AddressID = CreateAddress(mpAddress);
        }

        private int CreateAddress(Address address)
        {
            return _mpAddressService.Create(address);
        }

        private bool FindExistingAddress(AddressDTO address, Address mpAddress)
        {
            var result = _mpAddressService.FindMatchingAddresses(mpAddress);
            if (result.Count > 0)
            {
                var addressId = result.First(x => x.Address_ID.HasValue).Address_ID;
                if (addressId != null)
                {
                    address.AddressID = addressId.Value;
                    return true;
                }
            }

            return false;
        }
    }
}