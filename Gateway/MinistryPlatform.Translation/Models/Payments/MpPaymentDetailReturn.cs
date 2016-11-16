using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Payments
{
    public class MpPaymentDetailReturn
    {
        [JsonProperty(PropertyName = "Payment_Amount")]
        public decimal PaymentAmount { get; set; }

        [JsonProperty(PropertyName = "Payment_ID")]
        public int PaymentId { get; set; }

        [JsonProperty(PropertyName = "Invoice_Detail_ID")]
        public int InvoiceDetailId { get; set; }

        [JsonProperty(PropertyName = "Payment_Detail_ID")]
        public int PaymentDetailId { get; set; }
    }
}
