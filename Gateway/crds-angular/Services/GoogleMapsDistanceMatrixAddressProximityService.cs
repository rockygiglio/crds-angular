using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using GoogleMapsAPI.NET.API.Client.Interfaces;
using GoogleMapsAPI.NET.API.Common.Components.Locations;
using GoogleMapsAPI.NET.API.Common.Components.Locations.Interfaces.Combined;
using GoogleMapsAPI.NET.API.DistanceMatrix.Responses;
using GoogleMapsAPI.NET.API.Geocoding.Components;
using GoogleMapsAPI.NET.API.Places.Enums;
using GoogleMapsAPI.NET.Requests;
using GoogleMapsAPI.NET.Utils;
using GoogleMapsAPI.NET.Web.Extensions;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class GoogleMapsDistanceMatrixAddressProximityService : IAddressProximityService
    {
        private const double MetersPerMile = 1609.344;
        private const int MaxDestinationsPerRequest = 101;

        private readonly IMapsAPIClient _mapsApiClient;
        private readonly IAddressGeocodingService _addressGeocodingService;

        public GoogleMapsDistanceMatrixAddressProximityService(IMapsAPIClient mapsApiClient, IAddressGeocodingService addressGeocodingService)
        {
            _mapsApiClient = mapsApiClient;
            _addressGeocodingService = addressGeocodingService;
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

        //public AddressDTO ValidateAddress(AddressDTO address)
        //{
        //    return ValidateAddress(address.ToString());
        //}

        //public AddressDTO ValidateAddress(string address)
        //{
        //    var geocode = _mapsApiClient.Geocoding.Geocode(address);
        //    if (geocode.NoResultsFound)
        //    {
        //        throw new InvalidAddressException(geocode.ErrorMessage);
        //    }

        //    var addressComponents = geocode.Results[0].AddressComponents;
        //    var response = new AddressDTO
        //    {
        //        AddressLine1 = $"{GetAddressComponent(addressComponents, PlaceResultTypeEnum.StreetNumber)} {GetAddressComponent(addressComponents, PlaceResultTypeEnum.Route)}".Trim(),
        //        AddressLine2 = GetAddressComponent(addressComponents, PlaceResultTypeEnum.Subpremise),
        //        City = GetAddressComponent(addressComponents, PlaceResultTypeEnum.Locality),
        //        State = GetAddressComponent(addressComponents, PlaceResultTypeEnum.AdministrativeAreaLevel1),
        //        PostalCode = GetAddressComponent(addressComponents, PlaceResultTypeEnum.PostalCode),
        //        Latitude = geocode.Results[0].Geometry.Location.Latitude,
        //        Longitude = geocode.Results[0].Geometry.Location.Longitude,
        //    };

        //    return response;
        //}

        //private static string GetAddressComponent(List<Address> components, PlaceResultTypeEnum type)
        //{
        //    return components.Find(a => a.Types.Contains(type))?.ShortName;
        //}

        public List<decimal?> GetProximity(string originAddress, List<AddressDTO> destinationAddresses)
        {
            //var originCoords = GetGeoCoordinates(originAddress);

            //return destinationAddresses.Select(a => CalculateProximity(originCoords, a)).ToList();
            return GetProximity(originAddress, destinationAddresses.Select(a => a.ToString()).ToList());
        }

        private static decimal? CalculateProximity(GeoCoordinate origin, AddressDTO destination)
        {
            if (origin == null || destination.Longitude == null || destination.Latitude == null)
            {
                return null;
            }

            return ((decimal)Math.Round(origin.GetDistanceTo(new GeoCoordinate(destination.Latitude.Value, destination.Longitude.Value))/MetersPerMile*10))/10;
        }

        public List<decimal?> GetProximity(string originAddress, List<string> destinationAddresses)
        {
            var originCoords = _addressGeocodingService.GetGeoCoordinates(originAddress);
            
            var numRequests = (destinationAddresses.Count / MaxDestinationsPerRequest) + 1;
            
            var results = new List<decimal?>(destinationAddresses.Count);

            for (var i = 0; i < numRequests; i++)
            {
                var next = GetDistanceMatrix(originCoords, destinationAddresses.Skip(i * MaxDestinationsPerRequest).Take(MaxDestinationsPerRequest));
                results.AddRange(next.Rows[0].Elements.Select(e => e == null || e.NoResultsFound || e.Distance == null ? (decimal?)null : (decimal)Math.Round(e.Distance.Value / MetersPerMile * 10) / 10).ToList());
            }

            return results;
        }

        private GetDistanceMatrixResponse GetDistanceMatrix(GeoCoordinate origin, IEnumerable<string> destinations)
        {
            var queryParams = new QueryParams
            {
                ["origins"] = Converter.Location(new List<IAddressOrGeoCoordinatesLocation>
                {
                    new GeoCoordinatesLocation(origin.Latitude, origin.Longitude)
                }),
                ["destinations"] = Converter.Location(destinations.Select(a => new AddressLocation(a)))
            };

            return _mapsApiClient.APIGet("/maps/api/distancematrix/json", queryParams, ExtractResponseBody, new DateTime?());
        }

        private static GetDistanceMatrixResponse ExtractResponseBody(HttpWebResponse webResponse)
        {
            var response = JsonConvert.DeserializeObject<GetDistanceMatrixResponse>(webResponse.GetResponseContent());

            if (response.IsOverQueryLimit && response.HasErrorMessage && response.ErrorMessage.ToLower().Contains("daily request quota"))
            {
                throw new Exception("Over query limit");
            }
            return response;
        }
    }
}