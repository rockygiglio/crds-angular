﻿using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models.GoCincinnati;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class GroupConnector
    {
        [JsonProperty(PropertyName = "absoluteMaximumVolunteers")]
        public int AbsoluteMaximumVolunteers { get; set; }

        [JsonProperty(PropertyName = "groupConnectorId")]
        public int GroupConnectorId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "preferredLaunchSite")]
        public string PreferredLaunchSite { get; set; }

        [JsonProperty(PropertyName = "primaryContactRegistrationId")]
        public int PrimaryRegistraionContactId { get; set; }

        [JsonProperty(PropertyName = "projectMaximumVolunteers")]
        public int ProjectMaximumVolunteers { get; set; }

        [JsonProperty(PropertyName = "projectMinimumAge")]
        public int ProjectMinimumAge { get; set; }

        [JsonProperty(PropertyName = "projectType")]
        public string ProjectType { get; set; }

        [JsonProperty(PropertyName = "volunteerCount")]
        public int VolunteerCount { get; set; }

        public List<GroupConnector> FromMpGroupConnectorList(List<MpGroupConnector> mpGroupConnectors)
        {
            return mpGroupConnectors.Select(r => FromMpGroupConnector(r)).ToList();
        }

        public GroupConnector FromMpGroupConnector(MpGroupConnector mpGroupConnectors)
        {
            var r = mpGroupConnectors;
            return new GroupConnector
            {
                GroupConnectorId = r.Id,
                Name = r.Name,
                PrimaryRegistraionContactId = r.PrimaryRegistrationID,
                ProjectMaximumVolunteers = r.ProjectMaximumVolunteers,
                AbsoluteMaximumVolunteers = r.AbsoluteMaximumVolunteers,
                ProjectMinimumAge = r.ProjectMinimumAge,
                ProjectType = r.ProjectType,
                PreferredLaunchSite = r.PreferredLaunchSite,
                VolunteerCount = r.VolunteerCount
            };
        }
    }
}