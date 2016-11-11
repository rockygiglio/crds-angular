using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class ProductDTO
    {
        [JsonProperty(PropertyName = "productId")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "basePrice")]
        public decimal BasePrice { get; set; }

        [JsonProperty(PropertyName = "basePriceEndDate")]
        public DateTime BasePriceEndDate { get; set; }

        [JsonProperty(PropertyName = "depositPrice")]
        public decimal DepositPrice { get; set; }

        [JsonProperty(PropertyName = "financialAssistance")]
        public bool FinancialAssistance { get; set; }

        [JsonProperty(PropertyName = "options")]
        public List<ProductOptionDTO> Options { get; set; }
    }
}