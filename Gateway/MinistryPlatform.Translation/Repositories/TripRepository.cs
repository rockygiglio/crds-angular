using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.FunctionalHelpers;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
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

        public Result<MpPledge> AddAsTripParticipant(int ContactId, int PledgeCampaignID, string token)
        {
            var storedProc = _configurationWrapper.GetConfigValue("TripParticipantStoredProc");
            try
            {
                var fields = new Dictionary<string, object>
                {
                    {"@PledgeCampaignID", PledgeCampaignID},
                    {"@ContactID", ContactId}
                };
                var result = _ministryPlatformRestRepository.UsingAuthenticationToken(token).GetFromStoredProc<MpPledge>(storedProc, fields);
                if (result.Count > 0 && result[0].Count > 0)
                {
                    return new Result<MpPledge>(true, result[0].FirstOrDefault());
                }
                _logger.Debug($"Adding a trip particpant returned no results. The trip is already full.");
                return new Result<MpPledge>(false, "Trip is already full");
            }
            catch (Exception e)
            {
                _logger.Error($"Failed to call stored procedure #{storedProc}");
                _logger.Error(e.Message);
                throw;
            }            
        }
    }
}
