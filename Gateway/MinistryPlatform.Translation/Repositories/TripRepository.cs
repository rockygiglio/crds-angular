using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TripRepository));

        private readonly IMinistryPlatformRestRepository _ministryPlatformRestRepository;
        private readonly IConfigurationWrapper _configurationWrapper;

        public TripRepository(IMinistryPlatformRestRepository ministryPlatformRestRepository, IConfigurationWrapper configurationWrapper)
        {
            _ministryPlatformRestRepository = ministryPlatformRestRepository;
            _configurationWrapper = configurationWrapper;
        }

        public bool AddAsTripParticipant(int ContactId, int PledgeCampaignID, string token)
        {
            var storedProc = _configurationWrapper.GetConfigValue("TripParticipantStoredProc");
            try
            {
                var fields = new Dictionary<string, object>
                {
                    {"@PledgeCampaignID", PledgeCampaignID},
                    {"@ContactID", ContactId}
                };
                var result = _ministryPlatformRestRepository.UsingAuthenticationToken(token).PostStoredProc(storedProc, fields);
                if (result == 200)
                {
                    return true;
                }
                _logger.Debug($"Got a status back of ${result} instead of 200");             
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to call stored procedure #{storedProc}");
                _logger.Error(e.Message);                
            }
            return false;
        }
    }
}
