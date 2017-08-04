using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services {
    public class LookupService : ILookupService {

        private readonly ILookupRepository _lookupRepository;
        private readonly IApiUserRepository _apiUserRepository;
        
        public LookupService(ILookupRepository lookupRepository, IApiUserRepository apiUserRepository)
        {
            _lookupRepository = lookupRepository;
            _apiUserRepository = apiUserRepository;
        }

        public string GetMeetingDayFromId(int meetingDayId)
        {
            var dayString = "";
            var token = _apiUserRepository.GetToken();
            var days = _lookupRepository.MeetingDays(token);

            var dictionary = days.FirstOrDefault();

            foreach (KeyValuePair<string, object> pair in dictionary)
                if (meetingDayId.Equals(pair.Value))
                {
                    dayString = pair.Key;
                }

            return dayString;
        }

        public string GetMeetingFrequencyFromId(int meetingFrequencyId)
        {
            throw new NotImplementedException();
        }
    }
}