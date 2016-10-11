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

        public List<MpCampEvent> GetCampEventDetails(int eventId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var parms = new Dictionary<string, object> { { "Event_ID", eventId }, { "Domain_ID", 1 } };
            var campEventData = _ministryPlatformRest.UsingAuthenticationToken(apiToken).GetFromStoredProc<MpCampEvent>(_configurationWrapper.GetConfigValue("CampEventStoredProc"), parms);
            var campEvent = campEventData.FirstOrDefault() ?? new List<MpCampEvent>();
            return campEvent;
        }

        public List<MpMinorContact> CreateMinorContact(MpMinorContact minorContact)
        {
            var storedProc = _configurationWrapper.GetConfigValue("CreateContactStoredProc");
            var apiToken = _apiUserRepository.GetToken();
            var fields = new Dictionary<String, Object>
              {
                {"@FirstName", minorContact.FirstName},
                {"@LastName", minorContact.LastName},
                {"@MiddleName", minorContact.MiddleName },
                {"@PreferredName", minorContact.PreferredName },
                {"@Birthdate", minorContact.BirthDate },
                {"@Gender", minorContact.Gender },
                {"@SchoolAttending", minorContact.SchoolAttending },
                {"@HouseholdId", minorContact.HouseholdId },
                {"@HouseholdPosition", minorContact.HouseholdPositionId }
             };
             var result = _ministryPlatformRest.UsingAuthenticationToken(apiToken).GetFromStoredProc<MpMinorContact>(storedProc, fields);
             var newMinorContact = result.FirstOrDefault() ?? new List<MpMinorContact>();
             return newMinorContact;
        }

        public Result<int> AddAsCampParticipant(int contactId, int eventId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var storedProc = _configurationWrapper.GetConfigValue("CampParticipantStoredProc");
            try
            {
                var fields = new Dictionary<string, object>
                {
                    {"@EventID", eventId},
                    {"@ContactID", contactId}
                };
                var result = _ministryPlatformRest.UsingAuthenticationToken(apiToken).GetFromStoredProc<int>(storedProc, fields);
                if (result.Count > 0 && result[0].Count > 0)
                {
                    return new Result<int>(true, result[0].FirstOrDefault());
                }
                _logger.Debug($"Adding a camp particpant returned no results. The camp is already full.");
                return new Result<int>(false, "Camp is already full");
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
