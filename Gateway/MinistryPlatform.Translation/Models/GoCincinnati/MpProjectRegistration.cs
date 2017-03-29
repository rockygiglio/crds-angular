using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.GoCincinnati
{
    [MpRestApiTable(Name = "cr_Group_Connector_Registrations")]
    public class MpProjectRegistration
    {
        [JsonProperty(PropertyName = "Project_ID")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Mobile_Phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "Email_Address")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "Spouse_Participation")]
        public bool SpouseParticipating { get; set; }

        [JsonProperty(PropertyName = "_Family_Count")]
        public int FamilyCount { get; set; }

    }
}
