using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.Services
{
    public class GoEquipmentService : IGoEquipmentService
    {
            private readonly IConfigurationWrapper _configurationWrapper;
        private readonly MinistryPlatform.Translation.Services.Interfaces.IAttributeService _mpAttributeService;

        public GoEquipmentService(MinistryPlatform.Translation.Services.Interfaces.IAttributeService mpAttributeService,IConfigurationWrapper configurationWrapper)
        {
            _mpAttributeService = mpAttributeService;
            _configurationWrapper = configurationWrapper;
        }

        public List<GoEquipment> RetrieveGoEquipment()
        {
            var goCincinnatiEquipmentAttributeTypeId = _configurationWrapper.GetConfigIntValue("GoCincinnatiEquipmentAttributeType");
            var equipment = _mpAttributeService.GetAttributes(goCincinnatiEquipmentAttributeTypeId);
            return new GoEquipment().ToGoEquipment(equipment);
        }
    }
}