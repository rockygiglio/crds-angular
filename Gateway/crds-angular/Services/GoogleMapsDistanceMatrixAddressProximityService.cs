using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using GoogleMapsAPI.NET.API.Client.Interfaces;
using GoogleMapsAPI.NET.API.Common.Components.Locations;
using GoogleMapsAPI.NET.API.Common.Components.Locations.Interfaces.Combined;
using GoogleMapsAPI.NET.API.DistanceMatrix.Responses;
using GoogleMapsAPI.NET.Requests;
using GoogleMapsAPI.NET.Utils;
using GoogleMapsAPI.NET.Web.Extensions;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class GoogleMapsDistanceMatrixAddressProximityService : IAddressProximityService
    {
        private const double MetersPerMile = 1609.344;
        private const int MaxDestinationsPerRequest = 100;

        private readonly IMapsAPIClient _mapsApiClient;
        private readonly IAddressGeocodingService _addressGeocodingService;

        public GoogleMapsDistanceMatrixAddressProximityService(IMapsAPIClient mapsApiClient, IAddressGeocodingService addressGeocodingService)
        {
            _mapsApiClient = mapsApiClient;
            _addressGeocodingService = addressGeocodingService;
        }

        public List<decimal?> GetProximity(string originAddress, List<AddressDTO> destinationAddresses)
        {
            return GetProximity(originAddress, destinationAddresses.Select(a => a.ToString()).ToList());
        }

        public List<decimal?> GetProximity(string originAddress, List<string> destinationAddresses)
        {
            var originCoords = _addressGeocodingService.GetGeoCoordinates(originAddress);

            var numRequests = Math.Ceiling(destinationAddresses.Count / (decimal)MaxDestinationsPerRequest);

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