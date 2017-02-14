using System;
using System.Linq;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Helpers;
using MinistryPlatform.Translation.Models.Connect;

namespace MinistryPlatform.Translation.Repositories
{
    public class ConnectRepository : IConnectRepository
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CampRepository));

        public ConnectRepository(IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public ConnectPinDto GetPinDetails(int participantId)
        {
            const string pinSearch = "Email_Address, Nickname as FirstName, Last_Name as LastName, Participant_Record_Table.*";
            string filter = $"Participant_Record = {participantId}";
            string token = _apiUserRepository.GetToken();

            var pinDetails = _ministryPlatformRest.UsingAuthenticationToken(token).Search<ConnectPinDto>(filter, pinSearch)?.First();

            const string addressSearch = "Household_ID_Table_Address_ID_Table.*";
            if (pinDetails != null) pinDetails.Address = _ministryPlatformRest.UsingAuthenticationToken(token).Search<ConnectAddressDto>(filter, addressSearch)?.First();


            return pinDetails;
        }


    }
}
