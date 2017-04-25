using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Households")]
    public class MpHousehold
    {
        public string Home_Phone { get; set; }
        
        [JsonProperty(PropertyName = "Congregation_ID")]
        public int? Congregation_ID { get; set; }

        [JsonProperty(PropertyName = "Household_ID")]
        public int Household_ID { get; set; }
       
        public int Address_ID { get; set; }
    }
}
