using System.Collections.Generic;
using System.Linq;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;

namespace crds_angular.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly MPInterfaces.IOrganizationService _mpOrganizationService;
        private readonly MPInterfaces.IApiUserService _mpApiUserService;

        public OrganizationService(MPInterfaces.IOrganizationService organizationService, MPInterfaces.IApiUserService apiUserService)
        {
            _mpOrganizationService = organizationService;
            _mpApiUserService = apiUserService;            
        }

        public Organization GetOrganizationByName(string name)
        {
            var apiUserToken = _mpApiUserService.GetToken();
            var org = new Organization();
            var mpOrg = _mpOrganizationService.GetOrganization(name, apiUserToken);
            if (mpOrg != null)
            {
                return org.FromMpOrganization(mpOrg);
            }
            return null;
        }

        public List<Organization> GetOrganizations()
        {
            var apiUserToken = _mpApiUserService.GetToken();
            var mpOrgs = _mpOrganizationService.GetOrganizations(apiUserToken);
            return mpOrgs.Select(o =>
            {
                var org = new Organization();
                return org.FromMpOrganization(o);
            }).ToList();
        }
    }
}