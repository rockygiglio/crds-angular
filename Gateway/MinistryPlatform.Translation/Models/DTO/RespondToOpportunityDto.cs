using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.DTO
{
    public class RespondToOpportunityDto
    {
        [JsonProperty(PropertyName = "opportunityId")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "participants")]
        public List<int> Participants { get; set; }
    }
}
