using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Product
{
    [MpRestApiTable(Name = "Products")]
    public class MpProduct
    {
        [JsonProperty(PropertyName = "Product_ID")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "Product_Name")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "Base_Price")]
        public double BasePrice { get; set; }

        [JsonProperty(PropertyName = "Deposit_Price")]
        public double DepositPrice { get; set; }
    }
}
