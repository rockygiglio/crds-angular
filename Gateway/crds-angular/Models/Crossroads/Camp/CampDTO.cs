using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampDTO
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "eventTitle")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public String EventType { get; set; }

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
