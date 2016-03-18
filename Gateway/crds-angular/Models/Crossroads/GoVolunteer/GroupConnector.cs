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

        [JsonProperty(PropertyName = "projectMinimumAge")]
        public int ProjectMinimumAge { get; set; }

        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

        [JsonProperty(PropertyName = "projectType")]
        public string ProjectType { get; set; }


        public List<GroupConnector> FromMpGroupConnectorList(List<MpGroupConnector> mpGroupConnectors)
        {
            return mpGroupConnectors.Select(r => new GroupConnector
            {
                Id = r.Id,
                Name = r.Name,
                ProjectMinimumAge = r.ProjectMinimumAge,
                ProjectName = r.ProjectName,
                ProjectType = r.ProjectType,
                PreferredLaunchSite = r.PreferredLaunchSite
            }).ToList();
        }
    }
}