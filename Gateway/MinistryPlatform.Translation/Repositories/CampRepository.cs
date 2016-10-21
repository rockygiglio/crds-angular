using System;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class CampRepository : ICampRepository
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CampRepository));
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IApiUserRepository _apiUserRepository;

        public CampRepository(IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public MpCamp GetCampEventDetails(int eventId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var campType = _configurationWrapper.GetConfigIntValue("CampEventType");
            var campData = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpCamp>($"Event_ID = {eventId}").ToList();
            campData = campData.Where((camp) => camp.EventType == campType).ToList();
            if (campData.Count > 0)
            {
                return campData.FirstOrDefault();
            }            
            throw new Exception("No Camp found");
        }
    }
}
