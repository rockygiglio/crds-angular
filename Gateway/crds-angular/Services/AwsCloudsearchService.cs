using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;

namespace crds_angular.Services
{
    public class AwsCloudsearchService: MinistryPlatformBaseService, IAwsCloudsearchService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        public AwsCloudsearchService(
                           IConfigurationWrapper configurationWrapper)
        {
            _configurationWrapper = configurationWrapper;
        }
    }
}