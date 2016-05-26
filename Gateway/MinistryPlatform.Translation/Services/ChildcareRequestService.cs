using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ChildcareRequestService : IChildcareRequestService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserService _apiUserService;
        private readonly int _childcareRequestPageId;
        private readonly int _childcareRequestStatusPending;
        private readonly int _childcareEmailPageViewId;

        
        public ChildcareRequestService(IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            _ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
            _childcareRequestPageId = configurationWrapper.GetConfigIntValue("ChildcareRequestPageId");
            _childcareEmailPageViewId = configurationWrapper.GetConfigIntValue("ChildcareEmailPageView");
            _childcareRequestStatusPending = configurationWrapper.GetConfigIntValue("ChildcareRequestPending");
        }

        public int CreateChildcareRequest(ChildcareRequest request)
        {
            var apiToken = _apiUserService.GetToken();
            
            var requestDict = new Dictionary<string, object>
            {
                {"Requester_ID", request.RequesterId},
                {"Congregation_ID", request.LocationId },
                {"Ministry_ID", request.MinistryId },
                {"Group_ID", request.GroupId },
                {"Start_Date", request.StartDate },
                {"End_Date", request.EndDate },
                {"Frequency", request.Frequency },
                {"Childcare_Session", request.PreferredTime },
                {"Est_No_of_Children", request.EstimatedChildren },
                {"Notes", request.Notes },
                {"Request_Status_ID", _childcareRequestStatusPending }
            };
            var childcareRequestId = _ministryPlatformService.CreateRecord(_childcareRequestPageId, requestDict, apiToken);

            return childcareRequestId;
        }

        public ChildcareRequestEmail GetChildcareRequest(int childcareRequestId, string token)
        {
            var searchString = string.Format("{0}", childcareRequestId);
            var r = _ministryPlatformService.GetPageViewRecords(_childcareEmailPageViewId, token,searchString);
            if (r.Count == 1)
            {
                var record = r[0];

                var c = new ChildcareRequestEmail
                {
                    RequesterId = record.ToInt("Contact_ID"),
                    RequesterEmail = record.ToString("Email_Address"),
                    GroupName = record.ToString("Group_Name"),
                    MinistryName = record.ToString("Ministry_Name"),
                    StartDate = record.ToDate("Start_Date"),
                    EndDate = record.ToDate("End_Date"),
                    ChildcareSession = record.ToString("Childcare_Session"),
                    CongregationId = record.ToInt("Congregation_ID"),
                    Requester = record.ToString("Display_Name")
                };

                return c;

            }
            if (r.Count == 0)
            {
                return null;
            }
            throw new ApplicationException(string.Format("Duplicate Childcare Request ID detected: {0}", childcareRequestId));
        }


    }
}
