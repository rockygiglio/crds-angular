using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public  class CampProductDTO
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "financialAssistance")]
        public bool FinancialAssistance { get; set; }
    }
}