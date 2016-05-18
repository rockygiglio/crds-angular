using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareRequestDto
    {
        [JsonProperty(PropertyName = "requester")]
        public int RequesterId { get; set; }

        [JsonProperty(PropertyName = "site")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "ministry")]
        public int MinistryId { get; set; }

        [JsonProperty(PropertyName = "group")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "frequency")]
        public string Frequency { get; set; }

        [JsonProperty(PropertyName = "timeframe")]
        public int PreferredTime { get; set; }

        [JsonProperty(PropertyName = "estimatedChildren")]
        public int EstimatedChildren { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}