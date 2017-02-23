using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Device.Location;

namespace crds_angular.Models.Crossroads
{
    public class AddressDTO
    {
        [JsonProperty(PropertyName = "addressId")]
        public int? AddressID { get; set; }

        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "foreignCountry")]
        public string ForeignCountry { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        public bool HasGeoCoordinates()
        {
            return Longitude.HasValue && Latitude.HasValue;
        }

        public override string ToString()
        {
            return $"{AddressLine1} {AddressLine2}, {City}, {State}, {PostalCode}";
        }
    }
}