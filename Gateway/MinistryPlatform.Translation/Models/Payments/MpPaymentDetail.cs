using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Payments
{
    [MpRestApiTable(Name = "Payment_Detail")]
    public class MpPaymentDetail
    {
        [JsonProperty(PropertyName = "Payment_Detail_ID")]
        public int PaymentDetailId { get; set; }
       
        [JsonProperty(PropertyName = "Payment_ID")]
        public MpPayment Payment { get; set; }
        
        [JsonProperty(PropertyName = "Payment_Amount")]
        public double PaymentAmount { get; set; }
    
        [JsonProperty(PropertyName = "Invoice_Detail_ID")]
        public int InvoiceDetailId { get; set; }
    }
}
