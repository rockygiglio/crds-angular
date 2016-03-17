using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.GoVolunteer;
using MinistryPlatform.Translation.Models.GoCincinnati;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class GroupConnectorService
    {
        public GroupConnector GetGroupConnectorsByOrganization(Organization organization)
        {
            var groupConnector = new GroupConnector();
            var mpGroupConnector = {};
            if (mpGroupConnector != null)
            {
                return groupConnector.FromMpGroupConnector(mpGroupConnector);
            }
            return null;
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

        public GroupConnector FromMpGroupConnector(MpGroupConnector mpGroupConnector)
        {
            return new GroupConnector()
            {
                Id = mpGroupConnector.Id,
                Name = mpGroupConnector.Name,
                PreferredLaunchSite = mpGroupConnector.PreferredLaunchSite,
                ProjectName = mpGroupConnector.ProjectName,
                OrganizationName = mpGroupConnector.OrganizationName
            };
        }
    }

    
}