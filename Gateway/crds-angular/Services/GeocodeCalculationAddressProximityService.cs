using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;

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

        public List<decimal?> GetProximity(string originAddress, List<string> destinationAddresses)
        {
            // This is not implemented mainly because it is an expensive call.  We'd have to geocode each 
            // destination address as a separate call to the geocode service, then calculate distances.
            throw new NotImplementedException("Getting geocode distance for a destination string is not supported");
        }

        public List<decimal?> GetProximity(string originAddress, List<AddressDTO> destinationAddresses)
        {
            var originCoords = _addressGeocodingService.GetGeoCoordinates(originAddress);
            return destinationAddresses.Select(a => CalculateProximity(originCoords, a)).ToList();
        }

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