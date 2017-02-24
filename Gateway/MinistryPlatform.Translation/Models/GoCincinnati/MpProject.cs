using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.GoCincinnati
{
    [MpRestApiTable(Name = "cr_Projects")]
    public class MpProject
    {
        [JsonProperty(PropertyName = "Project_ID")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "Project_Name")]
        public string ProjectName { get; set; }

        [JsonProperty(PropertyName = "Project_Status_ID")]
        public int ProjectStatusId { get; set; }

        [JsonProperty(PropertyName = "Location_ID")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "Project_Type_ID")]
        public int ProjectTypeId { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string ProjectType { get; set; }

        [JsonProperty(PropertyName = "Organization_ID")]
        public int OrganizationId { get; set; }

        [JsonProperty(PropertyName = "Initiative_ID")]
        public int InitiativeId { get; set; }

        [JsonProperty(PropertyName = "Address_ID")]
        public int AddressId { get; set; }    
        
        [JsonProperty(PropertyName = "State")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }

    }
}