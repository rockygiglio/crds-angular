using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
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

        public List<Location> GetLocationsForOrganization(int orgId, string token)
        {
            var search = string.Format("{0},", orgId);
            var result = _ministryPlatformService.GetPageViewRecords(_configurationWrapper.GetConfigIntValue("LocationsByOrg"), token, search);
            return result.Select(record => new Location
            {
                LocationId = record.ToInt("dp_RecordID"),
                LocationName = record.ToString("Location Name"),
                LocationTypeId = record.ToInt("Location Type ID"),
                OrganizationId = record.ToInt("Organization ID"),
                OrganizationName = record.ToString("Name"),
                Address = record.ToString("Address Line 1"),
                City = record.ToString("City"),
                State = record.ToString("State/Region"),
                Zip = record.ToString("Postal Code"),
                ImageUrl = record.ToString("Image_URL")
            }).ToList();
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
