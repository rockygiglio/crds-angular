using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class ProductOptionDTO
    {
        [JsonProperty(PropertyName = "productOptionPriceId")]
        public int ProductOptionPriceId { get; set; }

        [JsonProperty(PropertyName = "optionTitle")]
        public string OptionTitle { get; set; }

        [JsonProperty(PropertyName = "optionPrice")]
        public double OptionPrice { get; set; }

        [JsonProperty(PropertyName = "daysOutToHide")]
        public int? DaysOutToHide { get; set; }

        [JsonProperty(PropertyName = "totalWithOptionPrice")]
        public double TotalWithOptionPrice { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }
    }
}