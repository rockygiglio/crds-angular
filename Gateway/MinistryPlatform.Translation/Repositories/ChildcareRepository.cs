using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class ChildcareRepository : IChildcareRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IApiUserRepository _apiUserRepository;

        public ChildcareRepository(IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public List<ChildcareDashboard> GetChildcareDashboard(int contactId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var parms = new Dictionary<string, object> {{"Contact_ID", contactId}, {"Domain_ID", 1} };
            var dashboardData = _ministryPlatformRest.UsingAuthenticationToken(apiToken).GetFromStoredProc<ChildcareDashboard>(_configurationWrapper.GetConfigValue("ChildcareDashboardStoredProc"), parms);
            return dashboardData.FirstOrDefault();
        }
    }
}