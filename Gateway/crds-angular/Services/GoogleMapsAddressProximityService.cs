using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using GoogleMapsAPI.NET.API.Client.Interfaces;

namespace crds_angular.Services
{
    public class GoogleMapsAddressProximityService : IAddressProximityService
    {
        private const double MetersPerMile = 1609.344;

        private readonly IMapsAPIClient _mapsApiClient;
        public GoogleMapsAddressProximityService(IMapsAPIClient mapsApiClient)
        {
            _mapsApiClient = mapsApiClient;
        }

        public GeoCoordinate GetGeoCoordinates(AddressDTO address)
        {
            return GetGeoCoordinates(address.ToString());
        }

        public GeoCoordinate GetGeoCoordinates(string address)
        {
            var geocode = _mapsApiClient.Geocoding.Geocode(address);
            if (geocode.NoResultsFound)
            {
                throw new InvalidAddressException(geocode.ErrorMessage);
            }

            var coords = new GeoCoordinate
            {
                Latitude = geocode.Results[0].Geometry.Location.Latitude,
                Longitude = geocode.Results[0].Geometry.Location.Longitude
            };

            return coords;
        }

        public List<decimal?> GetProximity(string originAddress, List<AddressDTO> destinationAddresses)
        {
            var originCoords = GetGeoCoordinates(originAddress);

            return destinationAddresses.Select(a => CalculateProximity(originCoords, a)).ToList();
        }

        private static decimal? CalculateProximity(GeoCoordinate origin, AddressDTO destination)
        {
            if (origin == null || destination.Longitude == null || destination.Latitude == null)
            {
                return null;
            }

            return ((decimal)Math.Round(origin.GetDistanceTo(new GeoCoordinate(destination.Latitude.Value, destination.Longitude.Value))/MetersPerMile*10))/10;
        }
    }
}