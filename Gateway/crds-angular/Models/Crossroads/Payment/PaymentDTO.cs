using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Payment
{
    public class PaymentDTO
    {
        [JsonProperty(PropertyName = "invoiceId")]
        public int InvoiceId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "stripeTransactionId")]
        public string StripeTransactionId { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }
        [JsonProperty(PropertyName = "paymentTypeId")]
        public int PaymentTypeId { get; set; }
    }
}