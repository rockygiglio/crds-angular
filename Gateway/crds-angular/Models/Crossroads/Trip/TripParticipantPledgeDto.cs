using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripParticipantPledgeDto
    {
        [JsonProperty(PropertyName = "pledgeAmount")]
        public int PledgeAmount { get; set; }

        [JsonProperty(PropertyName = "deposit")]
        public int Deposit { get; set; }

        [JsonProperty(PropertyName = "donorId")]
        public int DonorId { get; set; }

        [JsonProperty(PropertyName = "programId")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "programName")]
        public string ProgramName { get; set; }

        [JsonProperty(PropertyName = "campaignNickname")]
        public string CampaignNickname { get; set; }

        [JsonProperty(PropertyName = "campaignName")]
        public string CampaignName { get; set; }
    }
}