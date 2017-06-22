﻿using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Payments
{
    [MpRestApiTable(Name = "Payment_Detail")]
    public class MpPaymentDetail
    {
        public int PaymentId { get; set; }

        [JsonProperty(PropertyName = "Payment_Detail_ID")]
        public int PaymentDetailId { get; set; }
       
        [JsonProperty(PropertyName = "Payment_ID")]
        public MpPayment Payment { get; set; }
        
        [JsonProperty(PropertyName = "Payment_Amount")]
        public decimal PaymentAmount { get; set; }
    
        [JsonProperty(PropertyName = "Invoice_Detail_ID")]
        public int InvoiceDetailId { get; set; }

        [JsonProperty(PropertyName = "Congregation_ID")]
        public int CongregationId { get; set; }
    }
}
