using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Connect
{
    public class AddressDto
    {
        [JsonProperty(PropertyName = "addressId")]
        public int? Address_ID { get; set; }

        [JsonProperty(PropertyName = "addressLine1")]
        public string Address_Line_1 { get; set; }

        [JsonProperty(PropertyName = "addressLine2")]
        public string Address_Line_2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string Postal_Code { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Foreign_Country { get; set; }

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
            return $"{Address_Line_1} {Address_Line_2}, {City}, {State}, {Postal_Code}";
        }
    }
}