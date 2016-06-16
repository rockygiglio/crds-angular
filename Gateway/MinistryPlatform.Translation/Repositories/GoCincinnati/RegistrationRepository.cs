using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;

namespace MinistryPlatform.Translation.Repositories.GoCincinnati
{
    public class RegistrationRepository : BaseRepository, IRegistrationRepository
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (RoomRepository));
        private readonly IMinistryPlatformService _ministryPlatformService;

        public RegistrationRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public int AddAgeGroup(int registrationId, int attributeId, int count)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Attribute_ID", attributeId},
                {"Count", count}
            };
            return AddAttribute(registrationId, dictionary, "RegistrationChildrenSubPage");
        }

        public int AddEquipment(int registrationId, int equipmentId, string notes)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Attribute_ID", equipmentId},
                {"Notes", notes}
            };
            return AddAttribute(registrationId, dictionary, "RegistrationEquipmentSubPage");
        }

        public int AddPrepWork(int registrationId, int attributeId, bool spouse)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Attribute_ID", attributeId},
                {"Spouse", spouse}
            };
            return AddAttribute(registrationId, dictionary, "RegistrationPrepWorkSubPage");
        }

        public int AddProjectPreferences(int registrationId, int projectType, int priority)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Project_Type_ID", projectType},
                {"Priority", priority}
            };

            return AddAttribute(registrationId, dictionary, "RegistrationProjectPreferencesSubPage");
        }

        public int CreateRegistration(MpRegistration registration)
        {
            var token = ApiLogin();
            var registrationPageId = _configurationWrapper.GetConfigIntValue("RegistrationPageId");
            var registrationDictionary = new Dictionary<string, object>
            {
                {"Organization_ID", registration.OrganizationId},
                {"Other_Organization_Name", registration.OtherOrganizationName},
                {"Preferred_Launch_Site_ID", registration.PreferredLaunchSiteId},
                {"Participant_ID", registration.ParticipantId},
                {"Initiative_ID", registration.InitiativeId},
                {"Spouse_Participation", registration.SpouseParticipation},
                {"Additional_Information", registration.AdditionalInformation},
                {"Role_Id", registration.RoleId}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(registrationPageId, registrationDictionary, token, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Go Cincinnati Registration, registration: {0}", registration);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        private int AddAttribute(int registrationId, Dictionary<string, object> dictionary, string pageKey)
        {
            var token = ApiLogin();
            try
            {
                return _ministryPlatformService.CreateSubRecord(pageKey, registrationId, dictionary, token, true);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error Go Cincinnati AddAttribute, registration: {0}, pageKey {1}", registrationId, pageKey);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }
    }
}