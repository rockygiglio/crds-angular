using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads.GoVolunteer;
using log4net;

namespace crds_angular.Services
{
    public class GoVolunteerService : MinistryPlatformBaseService

    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(GoVolunteerService));

        public bool CreateRegistration(Registration registration, string token)
        {
            try
            {
                throw new NotImplementedException("Create Registration");

                // Create Registration

                // Create or Add Group Connector

                // Create or Update Contact

                // Create or Update Spouse?

                // Add Children Attributes
                 
                // Prep Work

                // Equipment

                // Project Preferences
            }
            catch (Exception ex)
            {
                var msg = "Go Volunteer Service: CreateRegistration";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
            return true;
        }
    }
}