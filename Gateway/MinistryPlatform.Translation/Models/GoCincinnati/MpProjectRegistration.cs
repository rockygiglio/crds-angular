using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.GoCincinnati
{
    [MpRestApiTable(Name = "cr_Group_Connector_Registrations")]
    public class MpProjectRegistration
    {
        [JsonProperty(PropertyName = "Group_Connector_ID_Table_Project_ID_Table.[Project_ID]")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Nickname]")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name]")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Mobile_Phone]")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Email_Address]")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "Registration_ID_Table.[Spouse_Participation]")]
        public bool SpouseParticipating { get; set; }

        [JsonProperty(PropertyName = "Registration_ID_Table.[_Family_Count]")]
        public int FamilyCount { get; set; }

    }
}
