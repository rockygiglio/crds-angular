using System;
using crds_angular.Models.Crossroads.Payment;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class MyCampDTO
    {
        [JsonProperty(PropertyName = "contactId")]
        public int CamperContactId { get; set; }

        [JsonProperty(PropertyName = "nickName")]
        public string CamperNickName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string CamperLastName { get; set; }

        [JsonProperty(PropertyName = "eventTitle")]
        public string CampName { get; set; }

        [JsonProperty(PropertyName = "eventPrimaryContact")]
        public string CampPrimaryContactEmail { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime CampStartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime CampEndDate { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "camperInvoice")]
        public PaymentDetailDTO CamperInvoice { get; set; }
    }
}