using System;
using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Group_Participants")]
    public class MpGroupParticipant
    {
        [JsonProperty("Group_Participant_ID")]
        public int GroupParticipantId { get; set; }
        [JsonProperty("Participant_ID")]
        public int ParticipantId { get; set; }
        [JsonProperty("Group_ID")]
        public int GroupId { get; set; }
        [JsonProperty("Group_Role_ID")]
        public int GroupRoleId { get; set; }
        public int ContactId { get; set; }
        public string NickName { get; set; }
        public string LastName { get; set; }
        public string GroupRoleTitle { get; set; }
        public string Email { get; set; }
        public DateTime? StartDate { get; set; }
        public string Congregation { get; set; }
        public bool IsApprovedSmallGroupLeader { get; set; }
    }
}
