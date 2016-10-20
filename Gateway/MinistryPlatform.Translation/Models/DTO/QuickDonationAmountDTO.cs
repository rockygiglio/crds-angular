using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinistryPlatform.Translation.Models.DTO
{
    public class QuickDonationAmountDTO
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

}
