using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ResponseService : BaseService, IResponseService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformService _ministryPlatformService;

        private readonly int _signupToServeRemindersId;

        public ResponseService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _authenticationService = authenticationService;
            _configurationWrapper = configurationWrapper;
            _ministryPlatformService = ministryPlatformService;

            _signupToServeRemindersId = _configurationWrapper.GetConfigIntValue("SignupToServeReminders");
        }

        public List<MPServeReminder> GetServeReminders(String token)
        {
            var dict = _ministryPlatformService.GetPageViewRecords(_signupToServeRemindersId, token, "", "", 0);

            return dict.Select(rec =>
            {
                var mp = new MPServeReminder()
                {
                    Event_End_Date = (DateTime)rec["Event_End_Date"],
                    Event_Start_Date = (DateTime)rec["Event_Start_Date"],
                    Event_Title = (String)rec["Event_Title"],
                    Opportunity_Contact_Id = (int)rec["Opportunity_Contact_ID"],
                    Opportunity_Email_Address = (String)rec["Opportunity_Contact_Email_Address"],
                    Opportunity_Title = (String)rec["Opportunity_Title"],
                    Shift_End = rec.ToNullableTimeSpan("Shift_End"),
                    Shift_Start = rec.ToNullableTimeSpan("Shift_Start"),
                    Signedup_Contact_Id = (int)rec["Contact_ID"],
                    Signedup_Email_Address = (String)rec["Email_Address"]
                };
                if (rec["Communication_ID"] != null)
                {
                    mp.Template_Id = (int)rec["Communication_ID"];
                }
                return mp;
                    
            }).ToList();
        }  
    }
}
