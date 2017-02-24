using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.GoCincinnati
{
    [MpRestApiTable(Name = "cr_Group_Connectors")]
    public class MpGroupConnector
    {
        [JsonProperty(PropertyName = "Group_Connector_ID")]
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonProperty(PropertyName = "Primary_Registration")]
        public int PrimaryRegistrationID { get; set; }

        public string OrganizationName { get; set; }
        public string PreferredLaunchSite { get; set; }
        public int ProjectMaximumVolunteers { get; set; }
        public int AbsoluteMaximumVolunteers { get; set; }
        public int ProjectMinimumAge { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public int VolunteerCount { get; set; }
        public int PreferredLaunchSiteId { get; set; }

        [JsonProperty(PropertyName = "Contact_ID")]
        public int PrimaryContactId { get; set; }

        [JsonProperty(PropertyName = "First_Name")]
        public string PrimaryContactFirstName { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string PrimaryContactLastName { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string PrimaryContactNickname { get; set; }

        [JsonProperty(PropertyName = "Email_Address")]
        public string PrimaryContactEmail { get; set; }

    }
}