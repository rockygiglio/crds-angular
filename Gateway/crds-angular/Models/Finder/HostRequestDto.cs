using crds_angular.Models.Crossroads;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class HostRequestDto
    {
        [JsonProperty("contactId")] 
        public int ContactId { get; set; }

        [JsonProperty("address")]
        public AddressDTO Address { get; set; }

        [JsonProperty("isHomeAddress")]
        public bool IsHomeAddress { get; set; }

        [JsonProperty("contactNumber")]
        public string ContactNumber { get; set; }

        [JsonProperty("groupDescription")]
        public string GroupDescription { get; set; }
    }
}