using crds_angular.Models.Crossroads.Stewardship;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Payment
{
    public class PaymentDTO
    {
        [JsonProperty(PropertyName = "paymentId")]
        public int PaymentId { get; set; }

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

        [JsonProperty(PropertyName = "batchId")]
        public int? BatchId { get; set; }

        [JsonProperty(PropertyName = "processorFee")]
        public decimal ProcessorFee { get; set; }

        [JsonProperty(PropertyName = "paymentStatus")]
        public DonationStatus Status { get; set; }
    }
}