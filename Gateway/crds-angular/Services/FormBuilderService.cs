using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;
using IAttributeService = MinistryPlatform.Translation.Services.Interfaces.IAttributeService;
using IObjectAttributeService = crds_angular.Services.Interfaces.IObjectAttributeService;

namespace crds_angular.Services
{
    public class FormBuilderService : crds_angular.Services.Interfaces.IFormBuilderService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (GroupService));

        private IMinistryPlatformService _ministryPlatformService;
        private readonly IFormBuilderService _mpFormBuilderService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IParticipantService _participantService;
        private readonly IObjectAttributeService _objectAttributeService;
        private readonly IApiUserService _apiUserService;
        private readonly IAttributeService _attributeService;

        public FormBuilderService(IFormBuilderService mpFormBuilderService,
                            IMinistryPlatformService ministryPlatformService,
                            IConfigurationWrapper configurationWrapper,
                            IParticipantService participantService,
                            IObjectAttributeService objectAttributeService, 
                            IApiUserService apiUserService, 
                            IAttributeService attributeService)
        {
            _mpFormBuilderService = mpFormBuilderService;
            _configurationWrapper = configurationWrapper;
            _participantService = participantService;
            _objectAttributeService = objectAttributeService;
            _apiUserService = apiUserService;
            _attributeService = attributeService;
        }

        public List<Dictionary<string, object>> GetPageViewRecords(int pageView)
        {
            var pageViewRecords = _ministryPlatformService.GetPageViewRecords(pageView, token);
            try
            {
                if (pageViewRecords.Count == 0)
                {
                    throw new ApplicationException();
                }
            }
            catch (Exception e)
            {
                var message = String.Format("Could not retrieve page view details {0}: {1}", pageView, e.Message);
                logger.Error(message, e);
                throw (new ApplicationException(message, e));
            }

            return pageViewRecords;
        }

    }
}
