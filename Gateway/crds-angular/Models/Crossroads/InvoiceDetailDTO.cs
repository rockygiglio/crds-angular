using crds_angular.Models.Crossroads.Camp;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class InvoiceDetailDTO
    {
        [JsonProperty(PropertyName = "invoiceDetailID")]
        public int InvoiceDetailId { get; set; }

        [JsonProperty(PropertyName = "invoiceStatusID")]
        public int InvoiceStatusId { get; set; }

        [JsonProperty(PropertyName = "invoiceID")]
        public int InvoiceId { get; set; }

        [JsonProperty(PropertyName = "recipientContactID")]
        public int RecipientContactId { get; set; }

        [JsonProperty(PropertyName = "eventParticipantID")]
        public int? EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "itemQuantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "lineTotal")]
        public double LineTotal { get; set; }

        [JsonProperty(PropertyName = "productID")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "product")]
        public ProductDTO Product { get; set; }

        [JsonProperty(PropertyName = "productOptionPriceID")]
        public int? ProductOptionPriceId { get; set; }

        [JsonProperty(PropertyName = "itemNote")]
        public string ItemNote { get; set; }

        [JsonProperty(PropertyName = "recipientName")]
        public string RecipientName { get; set; }

        [JsonProperty(PropertyName = "recipientAddress")]
        public string RecipientAddress { get; set; }

        [JsonProperty(PropertyName = "recipientEmail")]
        public string RecipientEmail { get; set; }

        [JsonProperty(PropertyName = "recipientPhone")]
        public string RecipientPhone { get; set; }
    }
}