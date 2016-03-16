using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class OrganizationService : BaseService,IOrganizationService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public OrganizationService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public MPOrganization GetOrganization(string name, string token)
        {
            var search = string.Format("{0},", name);
            var result = _ministryPlatformService.GetRecordsDict(_configurationWrapper.GetConfigIntValue("OrganizationsPage"), token, search);            
            return MapOrganizations(result).SingleOrDefault();
        }

        public List<MPOrganization> GetOrganizations(string token)
        {
            var result = _ministryPlatformService.GetRecordsDict(_configurationWrapper.GetConfigIntValue("OrganizationsPage"), token);
            return MapOrganizations(result);
        }

        private static List<MPOrganization> MapOrganizations(IEnumerable<Dictionary<string, object>> records )
        {
            return records.Select(record => new MPOrganization
            {
                ContactId = record.ToInt("Primary_Contact"),
                EndDate = record.ToDate("End_Date"),
                StartDate = record.ToDate("Start_Date"),
                Name = record.ToString("Name"),
                OpenSignup = record.ToBool("Open_Signup"),
                OrganizationId = record.ToInt("dp_RecordID")
            }).ToList();
        }
    }
}
