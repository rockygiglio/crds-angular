﻿using System;
using System.Linq;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services
{
    public class AddressService : IAddressService
    {
        private readonly MinistryPlatform.Translation.Repositories.Interfaces.IAddressRepository _mpAddressService;
        private readonly IAddressGeocodingService _addressGeocodingService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (AddressService));

        public AddressService(MinistryPlatform.Translation.Repositories.Interfaces.IAddressRepository mpAddressService, IAddressGeocodingService addressGeocodingService)
        {
            _mpAddressService = mpAddressService;
            _addressGeocodingService = addressGeocodingService;
        }

        public void FindOrCreateAddress(AddressDTO address, bool updateGeoCoordinates = false)
        {
            var mpAddress = AutoMapper.Mapper.Map<MpAddress>(address);
            var found = FindExistingAddress(address, mpAddress);
            if (found)
            {
                mpAddress.Address_ID = address.AddressID;
                if (!updateGeoCoordinates || address.HasGeoCoordinates())
                {
                    return;
                }
            }

            if (updateGeoCoordinates && !address.HasGeoCoordinates())
            {
                SetGeoCoordinates(address);
                mpAddress.Longitude = address.Longitude;
                mpAddress.Latitude = address.Latitude;
            }

            address.AddressID = found ? UpdateAddress(mpAddress) : CreateAddress(mpAddress);
        }

        public void SetGeoCoordinates(AddressDTO address)
        {
            try
            {
                var coords = _addressGeocodingService.GetGeoCoordinates(address);
                address.Longitude = coords.Longitude;
                address.Latitude = coords.Latitude;
                var mpAddress = AutoMapper.Mapper.Map<MpAddress>(address);
                UpdateAddress(mpAddress);
            }
            catch (InvalidAddressException e)
            {
                _logger.Info($"Can't get GeoCoordinates for address '{address}', address is invalid", e);
            }
            catch (Exception e)
            {
                _logger.Error($"Error getting GeoCoordinates for address '{address}'", e);
            }
        }

        private int CreateAddress(MpAddress address)
        {
            return _mpAddressService.Create(address);
        }

        private int UpdateAddress(MpAddress address)
        {
            return _mpAddressService.Update(address);
        }

        private bool FindExistingAddress(AddressDTO address, MpAddress mpAddress)
        {
            var result = _mpAddressService.FindMatches(mpAddress);
            if (result.Count > 0)
            {
                var found = result.First(x => x.Address_ID.HasValue);
                if (found == null)
                {
                    return false;
                }
                address.AddressID = found.Address_ID;
                address.Latitude = found.Latitude;
                address.Longitude = found.Longitude;
                return true;
            }

            return false;
        }
    }
}