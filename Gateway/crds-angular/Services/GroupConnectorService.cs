using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.GoVolunteer;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services;
using Newtonsoft.Json;
using IGroupConnectorService = crds_angular.Services.Interfaces.IGroupConnectorService;

namespace crds_angular.Services
{
    public class GroupConnectorService : IGroupConnectorService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.IGroupConnectorService _mpGroupConnectorService;

        public GroupConnectorService(MinistryPlatform.Translation.Services.Interfaces.IGroupConnectorService groupConnectorService)
        {
            _mpGroupConnectorService = groupConnectorService;
        }

        public List<GroupConnector> GetGroupConnectorsByOrganization(int organization, int initiativeId)
        {
            
            var groupConnector = new GroupConnector();
            var mpGroupConnector = _mpGroupConnectorService.GetGroupConnectors(organization, initiativeId);
            return mpGroupConnector != null ? groupConnector.FromMpGroupConnectorList(mpGroupConnector) : null;
        }
    }

    public class GroupConnector
    {
        [JsonProperty(PropertyName = "organizationId")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

        [JsonProperty(PropertyName = "preferredLaunchSite")]
        public string PreferredLaunchSite { get; set; }

        [JsonProperty(PropertyName = "organizationName")]
        public string OrganizationName { get; set; }

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