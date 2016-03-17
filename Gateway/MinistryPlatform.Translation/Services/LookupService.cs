using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.Lookups;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class LookupService : BaseService, ILookupService
    {
        private readonly IMinistryPlatformService _ministryPlatformServiceImpl;

        public LookupService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformServiceImpl)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformServiceImpl = ministryPlatformServiceImpl;
        }

        public Dictionary<string, object> EmailSearch(string email, string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecord(AppSettings("Emails"), email, token);
        }

        public List<Dictionary<string, object>> EventTypes(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("EventTypesLookup"), token);
        }

        public List<Dictionary<string, object>> Genders(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("Genders"), token);
        }

        public List<Dictionary<string, object>> MaritalStatus(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("MaritalStatus"), token);
        }

        public List<Dictionary<string, object>> ServiceProviders(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("ServiceProviders"), token);
        }

        public List<Dictionary<string, object>> States(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("States"), token);
        }

        public List<Dictionary<string, object>> Countries(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("Countries"), token);
        }

        public List<Dictionary<string, object>> CrossroadsLocations(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("CrossroadsLocations"), token);
        }

        public List<Dictionary<string, object>> ReminderDays(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("ReminderDaysLookup"), token);
        }

        public List<Dictionary<string, object>> WorkTeams(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(_configurationWrapper.GetConfigIntValue("WorkTeams"), token);
        }

        public IEnumerable<T> GetList<T>(string token)
        {
            if (typeof (T) == typeof (MPWorkTeams))
            {
                return (IEnumerable<T>) 
                    WorkTeams(token).Select(wt => new MPWorkTeams(wt.ToInt("dp_RecordID"), wt.ToString("dp_RecordName")));
            }
            if (typeof (T) == typeof (MPOtherOrganization))
            {                
                return (IEnumerable<T>)
                    _ministryPlatformServiceImpl.GetLookupRecords(_configurationWrapper.GetConfigIntValue("OtherOrgs"), token)
                    .Select(other => new MPOtherOrganization(other.ToInt("dp_RecordID"), other.ToString("dp_RecordName")));
            }

            return null;
        }

        public T GetObject<T>(string token)
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, object>> MeetingDays(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("MeetingDay"), token);
        }
    }
}