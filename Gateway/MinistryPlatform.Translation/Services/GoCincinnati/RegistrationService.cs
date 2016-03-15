using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services.GoCincinnati
{
    public interface IRegistrationService
    {
        int CreateRegistration(Registration registration);
    }

    public class RegistrationService : BaseService, IRegistrationService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        

        private readonly ILog _logger = LogManager.GetLogger(typeof(RoomService));

        public RegistrationService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public int CreateRegistration(Registration registration)
        {
            var t = ApiLogin();
            var registrationPageId = _configurationWrapper.GetConfigIntValue("RegistrationPageId");
            var registrationDictionary = new Dictionary<string, object>
            {
                {"Organization_ID", registration.OrganizationId},
                {"Preferred_Launch_Site_ID", registration.PreferredLaunchSiteId},
                {"Participant_ID", registration.ParticipantId},
                {"Initiative_ID", registration.InitiativeId},
                {"Spouse_Participation", registration.SpouseParticipation},
                {"Additional_Information", registration.AdditionalInformation},
                {"Role_Id", registration.RoleId}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(registrationPageId, registrationDictionary, t, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Go Cincinnati Registration, registration: {0}", registration);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }
    }
}
