using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class CampRepository : ICampRepository
    {             
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CampRepository));

        public CampRepository(IConfigurationWrapper configurationWrapper,
                              IMinistryPlatformRestRepository ministryPlatformRest,
                              IApiUserRepository apiUserRepository)
                              
        {            
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public MpCamp GetCampEventDetails(int eventId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var campType = _configurationWrapper.GetConfigIntValue("CampEventType");
            var gradeGroupId = _configurationWrapper.GetConfigIntValue("AgeorGradeGroupType");
            var campGrades = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpEventGroup>($"Event_ID_Table.[Event_ID] = {eventId} AND Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID] = {gradeGroupId}", "Group_ID_Table.Group_ID, Group_ID_Table.Group_Name").ToList();           
            var campData = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpCamp>($"Event_ID = {eventId}").ToList();
            campData = campData.Where((camp) => camp.EventType == campType).ToList();
            var campEvent = campData.FirstOrDefault();
            if (campEvent != null)
            {
                campEvent.CampGradesList = campGrades;
                return campEvent;
            }
            throw new Exception("No Camp found");
        }
    }
}
