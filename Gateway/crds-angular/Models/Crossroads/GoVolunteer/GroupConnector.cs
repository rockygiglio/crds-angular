using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models.GoCincinnati;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class GroupConnector
    {
        [JsonProperty(PropertyName = "organizationId")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "preferredLaunchSite")]
        public string PreferredLaunchSite { get; set; }

        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }


        public List<GroupConnector> FromMpGroupConnectorList(List<MpGroupConnector> mpGroupConnectors)
        {
            return mpGroupConnectors.Select(r => new GroupConnector
            {
                Id = r.Id,
                Name = r.Name,
                ProjectName = r.ProjectName,
                PreferredLaunchSite = r.PreferredLaunchSite
            }).ToList();
        }
    }
}