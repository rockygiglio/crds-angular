using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampDTO
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "eventTitle")]
        public String EventTitle { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public int EventType { get; set; }

        [JsonProperty(PropertyName = "programId")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "productId")]
        public int OnlineProductId { get; set; }

        [JsonProperty(PropertyName = "registrationStartDate")]
        public DateTime RegistrationStartDate { get; set; }

        [JsonProperty(PropertyName = "registrationEndDate")]
        public DateTime RegistrationEndDate { get; set; }

    }
}
