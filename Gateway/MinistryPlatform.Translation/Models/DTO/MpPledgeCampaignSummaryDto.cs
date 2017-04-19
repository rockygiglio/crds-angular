using System;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.DTO
{
    public class MpPledgeCampaignSummaryDto
    {
        [JsonProperty(PropertyName = "Pledge_Campaign_Id")]
        public int PledgeCampaignId { get; set; }

        [JsonProperty(PropertyName = "Total_Given")]
        public decimal TotalGiven { get; set; }

        [JsonProperty(PropertyName = "Total_Committed")]
        public decimal TotalCommitted { get; set; }

        [JsonProperty(PropertyName = "Start_Date")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "End_Date")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "Not_Started_Count")]
        public int NotStartedCount { get; set; }

        [JsonProperty(PropertyName = "On_Pace_Count")]
        public int OnPaceCount { get; set; }

        [JsonProperty(PropertyName = "Completed_Count")]
        public int CompletedCount { get; set; }

        [JsonProperty(PropertyName = "Total_Count")]
        public int TotalCount { get; set; }
    }
}
