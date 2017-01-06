using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class Room
    {
        [JsonProperty(PropertyName = "banquetCapacity")]
        public int BanquetCapacity { get; set; }

        [JsonProperty(PropertyName = "buildingId")]
        public int BuildingId { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "locationId")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "theaterCapacity")]
        public int TheaterCapacity { get; set; }

        [JsonProperty(PropertyName = "roomStatus")]
        public bool? RoomStatus { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "reservationStart")]
        public DateTime? ReservationStart { get; set; }

        [JsonProperty(PropertyName = "reservationEnd")]
        public DateTime? ReservationEnd { get; set; }

        [JsonProperty(PropertyName = "reservationId")]
        public int? ReservationId { get; set; }

    }
}