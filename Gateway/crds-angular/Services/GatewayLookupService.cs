using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Translation.Models.Lookups;

namespace crds_angular.Services
{
    public class GatewayLookupService : IGatewayLookupService
    {
        private readonly ILookupService _lookupService;
        private readonly IApiUserService _mpApiUserService;

        public GatewayLookupService(ILookupService lookupService, IApiUserService apiUserService)
        {
            _lookupService = lookupService;
            _mpApiUserService = apiUserService;
        }

        public List<OtherOrganization> GetOtherOrgs(string token)
        {
            var current_token = token ?? _mpApiUserService.GetToken();
            var orgList = _lookupService.GetList<MPOtherOrganization>(current_token);
            return orgList.Select(org => new OtherOrganization(org.OtherOrganizationID, org.OtherOrganization)).ToList();
        }
    }
}