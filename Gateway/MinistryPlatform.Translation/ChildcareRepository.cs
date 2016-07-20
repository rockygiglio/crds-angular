using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class ChildcareRepository : IChildcareRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IConfigurationWrapper _configurationWrapper;

        public ChildcareRepository(IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest)
        {
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
        }

        public List<object> GetChildcareDashboard(int contactId)
        {
            var parms = new Dictionary<string, object> {{"Contact_ID", contactId}};
            var dashboardData = _ministryPlatformRest.GetFromStoredProc<object>(_configurationWrapper.GetConfigValue("ChildcareDashboardStoredProc"), parms);
            return dashboardData;
        }
    }
}