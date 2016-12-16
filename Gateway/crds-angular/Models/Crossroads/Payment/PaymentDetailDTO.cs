using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Payment
{
    public class PaymentDetailDTO
    {
        [JsonProperty(PropertyName = "paymentAmount")]
        public decimal PaymentAmount { get; set; }

        [JsonProperty(PropertyName = "paymentLeft")]
        public decimal TotalToPay { get; set; }

        [JsonProperty(PropertyName = "recipientEmail")]
        public string RecipientEmail { get; set; }
    }
}