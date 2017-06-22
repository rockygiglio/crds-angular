﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Models.Lookups;

namespace crds_angular.Services
{
    public class GatewayLookupService : IGatewayLookupService
    {
        private readonly ILookupRepository _lookupService;
        private readonly IApiUserRepository _mpApiUserService;

        public GatewayLookupService(ILookupRepository lookupService, IApiUserRepository apiUserService)
        {
            _lookupService = lookupService;
            _mpApiUserService = apiUserService;
        }

        public List<OtherOrganization> GetOtherOrgs(string token)
        {
            var current_token = token ?? _mpApiUserService.GetToken();
            var orgList = _lookupService.GetList<MpOtherOrganization>(current_token);
            return orgList.Select(org => new OtherOrganization(org.OtherOrganizationID, org.OtherOrganization)).ToList();
        }
    }
}