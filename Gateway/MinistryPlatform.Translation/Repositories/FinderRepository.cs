using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Models.Finder;
using System.Device.Location;

namespace MinistryPlatform.Translation.Repositories
{
    public class FinderRepository : BaseRepository, IFinderRepository
    {
        private const int searchRadius = 6380; 

        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CampRepository));

        public FinderRepository(IConfigurationWrapper configuration,
                                IMinistryPlatformRestRepository ministryPlatformRest,
                                IMinistryPlatformService ministryPlatformService,
                                IApiUserRepository apiUserRepository,
                                IAuthenticationRepository authenticationService)
            : base(authenticationService, configuration)
        {
            _configurationWrapper = configuration;
            _ministryPlatformRest = ministryPlatformRest;
            _ministryPlatformService = ministryPlatformService;
            _apiUserRepository = apiUserRepository;
        }

        private class RemoteIp
        {
            public string Ip { get; set; }
        }

        public FinderPinDto GetPinDetails(int participantId)
        {
            const string pinSearch = "Email_Address, Nickname as FirstName, Last_Name as LastName, Participant_Record_Table.*, Household_ID";
            string filter = $"Participant_Record = {participantId}";
            string token = _apiUserRepository.GetToken();

            var pinDetails = _ministryPlatformRest.UsingAuthenticationToken(token).Search<FinderPinDto>(filter, pinSearch)?.First();

            const string addressSearch = "Household_ID_Table_Address_ID_Table.*";
            if (pinDetails != null) pinDetails.Address = _ministryPlatformRest.UsingAuthenticationToken(token).Search<MpAddress>(filter, addressSearch)?.First();


            return pinDetails;
        }

        public void EnablePin(int participantId)
        {
            var dict = new Dictionary<string, object> { { "Participant_ID", participantId }, { "Show_On_Map", true } };

            var update = new List<Dictionary<string, object>> { dict };

            var apiToken = _apiUserRepository.GetToken();
            _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put("Participants", update);
        }

        public List<SpPinDto> GetPinsInRadius(GeoCoordinate originCoords)
        {
            var apiToken = _apiUserRepository.GetToken();

            var parms = new Dictionary<string, object>()
            {
                {"@Latitude", originCoords.Latitude },
                {"@Longitude", originCoords.Longitude },
                {"@RadiusInKilometers", searchRadius }
            };

            string spName = "api_crds_get_Pins_Within_Range"; 

            try
            {
                List<List<SpPinDto>> storedProcReturn = _ministryPlatformRest.UsingAuthenticationToken(apiToken)
                                                                             .GetFromStoredProc<SpPinDto>(spName, parms);
                List<SpPinDto> pinsFromSp = storedProcReturn.FirstOrDefault();

                return pinsFromSp; 
            }
            catch (Exception ex)
            {
                var exception = ex;
                return new List<SpPinDto>();
            }
        }
    }
}
