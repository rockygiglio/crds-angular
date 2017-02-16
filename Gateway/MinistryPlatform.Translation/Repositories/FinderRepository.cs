using System.Linq;
using System.IO;
using System.Net;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Models.Finder;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Repositories
{
    public class FinderRepository : IFinderRepository
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CampRepository));

        private class RemoteIp
        {
            public string Ip { get; set; }
        }

        public FinderRepository(IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
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

        public string GetIpForRemoteUser()
        {
            string ip;

            var request = WebRequest.Create("https://api.ipify.org?format=json");
            using (var response = request.GetResponse())
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var responseString = stream.ReadToEnd();
                var s = JsonConvert.DeserializeObject<RemoteIp>(responseString);
                ip = s.Ip;
            }
            return ip;
        }
    }
}
