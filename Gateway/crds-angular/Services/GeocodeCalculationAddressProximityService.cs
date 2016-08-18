using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using GoogleMapsAPI.NET.API.Client.Interfaces;

namespace crds_angular.Services
{
    public class GeocodeCalculationAddressProximityService : IAddressProximityService
    {
        private const double MetersPerMile = 1609.344;

        private readonly IAddressGeocodingService _addressGeocodingService;

        public GeocodeCalculationAddressProximityService(IAddressGeocodingService addressGeocodingService)
        {
            _addressGeocodingService = addressGeocodingService;
        }

        public GeoCoordinate GetGeoCoordinates(AddressDTO address)
        {
            throw new NotImplementedException();
        }

        public GeoCoordinate GetGeoCoordinates(string address)
        {
            throw new NotImplementedException();
        }

        public List<decimal?> GetProximity(string originAddress, List<string> destinationAddresses)
        {
            throw new NotImplementedException("Getting geocode distance for a destination string is not supported");
        }

        public List<decimal?> GetProximity(string originAddress, List<AddressDTO> destinationAddresses)
        {
            //throw new NotImplementedException();

            var originCoords = _addressGeocodingService.GetGeoCoordinates(originAddress);
            return destinationAddresses.Select(a => CalculateProximity(originCoords, a)).ToList();
        }

        //public AddressDTO ValidateAddress(AddressDTO address)
        //{
        //    throw new NotImplementedException();
        //}

        //public AddressDTO ValidateAddress(string address)
        //{
        //    //throw new NotImplementedException();
        //    //var address = _addressGeocodingService.
        //}

        private static decimal? CalculateProximity(GeoCoordinate origin, AddressDTO destination)
        {
            if (origin == null || destination.Longitude == null || destination.Latitude == null)
            {
                return null;
            }

            return ((decimal)Math.Round(origin.GetDistanceTo(new GeoCoordinate(destination.Latitude.Value, destination.Longitude.Value)) / MetersPerMile* 10)) / 10;
        }
    }
}