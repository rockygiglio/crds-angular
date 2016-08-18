using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using GoogleMapsAPI.NET.API.Client.Interfaces;
using GoogleMapsAPI.NET.API.Geocoding.Components;
using GoogleMapsAPI.NET.API.Places.Enums;
using Microsoft.Owin.Security.Provider;

namespace crds_angular.Services
{
    public class GoogleMapsAddressGeocodingService : IAddressGeocodingService
    {
        private readonly IMapsAPIClient _mapsApiClient;
        public GoogleMapsAddressGeocodingService(IMapsAPIClient mapsApiClient)
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

        public AddressDTO ValidateAddress(AddressDTO address)
        {
            return ValidateAddress(address.ToString());
        }

        public AddressDTO ValidateAddress(string address)
        {
            var geocode = _mapsApiClient.Geocoding.Geocode(address);
            if (geocode.NoResultsFound)
            {
                throw new InvalidAddressException(geocode.ErrorMessage);
            }

            var addressComponents = geocode.Results[0].AddressComponents;
            var response = new AddressDTO
            {
                AddressLine1 = $"{GetAddressComponent(addressComponents, PlaceResultTypeEnum.StreetNumber)} {GetAddressComponent(addressComponents, PlaceResultTypeEnum.Route)}".Trim(),
                AddressLine2 = GetAddressComponent(addressComponents, PlaceResultTypeEnum.Subpremise),
                City = GetAddressComponent(addressComponents, PlaceResultTypeEnum.Locality),
                State = GetAddressComponent(addressComponents, PlaceResultTypeEnum.AdministrativeAreaLevel1),
                PostalCode = GetAddressComponent(addressComponents, PlaceResultTypeEnum.PostalCode),
                Latitude = geocode.Results[0].Geometry.Location.Latitude,
                Longitude = geocode.Results[0].Geometry.Location.Longitude,
            };

            return response;
        }

        private static string GetAddressComponent(List<Address> components, PlaceResultTypeEnum type)
        {
            return components.Find(a => a.Types.Contains(type))?.ShortName;
        }
    }
}