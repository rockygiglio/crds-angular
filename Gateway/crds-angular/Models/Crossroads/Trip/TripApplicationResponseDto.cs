using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicationResponseDto
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("exception")]
        public ApplicationException Exception { get; set; }

        [JsonProperty("pledgeId")]
        public int PledgeId { get; set; }

        [JsonProperty("depositAmount")]
        public int DepositAmount { get; set; }

        [JsonProperty(PropertyName = "donorId")]
        public int DonorId { get; set; }

        [JsonProperty(PropertyName = "programId")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "programName")]
        public string ProgramName { get; set; }
    }
}