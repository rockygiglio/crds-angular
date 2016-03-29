using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class GoVolunteerService : IGoVolunteerService
    {
        private readonly MPInterfaces.IGoVolunteerService _mpGoVolunteerService;
        private readonly MPInterfaces.IApiUserService _mpApiUserService;

        public GoVolunteerService(MPInterfaces.IGoVolunteerService goVolunteerService, MPInterfaces.IApiUserService apiUserService)
        {
            _mpGoVolunteerService = goVolunteerService;
            _mpApiUserService = apiUserService;
        }

        public List<ProjectType> GetProjectTypes()
        {
            var apiUserToken = _mpApiUserService.GetToken();
            
            var pTypes = _mpGoVolunteerService.GetProjectTypes(apiUserToken);
            return pTypes.Select(pt =>
            {
                var projType = new ProjectType();
                return projType.FromMpProjectType(pt);
            }).ToList();
        }
    }
}