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
            var days = _lookupRepository.MeetingDays(_apiUserRepository.GetToken());

            foreach (var day in days)
            {
                if ((int) day["dp_RecordID"] == meetingDayId) ;
                {
                    dayString = day["dp_RecordName"].ToString();
                }
            }
            return dayString;
        }

        public string GetMeetingFrequencyFromId(int meetingFrequencyId)
        {
            throw new NotImplementedException();
        }
    }
}