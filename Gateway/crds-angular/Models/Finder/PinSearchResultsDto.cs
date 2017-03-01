using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class PinSearchResultsDto
    {
        [JsonProperty("centerLocation")]
        public GeoCoordinates CenterLocation { get; set; }

        [JsonProperty("pinSearchResults")]
        public PinDto[] PinSearchResults { get; set; }

        public PinSearchResultsDto(GeoCoordinates centerLocation, PinDto[] pins)
        {
            this.CenterLocation = centerLocation;
            this.PinSearchResults = pins; 
        }
    }
}