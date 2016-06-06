using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ChildcareRequestService :  IChildcareRequestService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IApiUserService _apiUserService;
        private readonly int _childcareRequestPageId;
        private readonly int _childcareRequestStatusPending;
        private readonly int _childcareRequestStatusApproved;
        private readonly int _childcareEmailPageViewId;

        
        public ChildcareRequestService(IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            _ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
            _childcareRequestPageId = configurationWrapper.GetConfigIntValue("ChildcareRequestPageId");
            _childcareEmailPageViewId = configurationWrapper.GetConfigIntValue("ChildcareEmailPageView");
            _childcareRequestStatusPending = configurationWrapper.GetConfigIntValue("ChildcareRequestPending");
            _childcareRequestStatusApproved = configurationWrapper.GetConfigIntValue("ChildcareRequestApproved");
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
                {"Notes", request.Notes },
                {"Request_Status_ID", _childcareRequestStatusPending }
            };
            var childcareRequestId = _ministryPlatformService.CreateRecord(_childcareRequestPageId, requestDict, apiToken);

            return childcareRequestId;
        }

        public ChildcareRequestEmail GetChildcareRequest(int childcareRequestId, string token)
        {
            var searchString = string.Format("{0},", childcareRequestId);
            var r = _ministryPlatformService.GetPageViewRecords(_childcareEmailPageViewId, token,searchString);
            if (r.Count == 1)
            {
                var record = r[0];

                var c = new ChildcareRequestEmail
                {
                    RequestId = record.ToInt("Childcare_Request_ID"),
                    RequesterId = record.ToInt("Contact_ID"),
                    RequesterEmail = record.ToString("Email_Address"),
                    RequesterNickname = record.ToString("Nickname"),
                    RequesterLastName = record.ToString("Last_Name"),
                    GroupName = record.ToString("Group_Name"),
                    MinistryName = record.ToString("Ministry_Name"),
                    StartDate = record.ToDate("Start_Date"),
                    EndDate = record.ToDate("End_Date"),
                    ChildcareSession = record.ToString("Childcare_Session"),
                    Requester = record.ToString("Display_Name"),
                    ChildcareContactEmail = record.ToString("Childcare_Contact_Email_Address"),
                    ChildcareContactId = record.ToInt("Childcare_Contact_ID"),
                    CongregationName = record.ToString("Congregation_Name")
                };

                return c;

            }
            if (r.Count == 0)
            {
                return null;
            }
            throw new ApplicationException(string.Format("Duplicate Childcare Request ID detected: {0}", childcareRequestId));
        }

        public void ApproveChildcareRequest(int childcareRequestId)
        {
            var apiToken = _apiUserService.GetToken();

            var searchString = string.Format("{0},", childcareRequestId);
            var record = _ministryPlatformService.GetRecordDict(_childcareRequestPageId, childcareRequestId, apiToken);

            if (record == null)
            {
                throw new ApplicationException(string.Format("Childcare Request ID not found: {0}", childcareRequestId));
            }

            var requestDict = new Dictionary<string, object>
            {
                {"Childcare_Request_ID", childcareRequestId },
                {"Requester_ID",record.ToInt("Requester_ID")},
                {"Congregation_ID", record.ToInt("Congregation_ID")},
                {"Ministry_ID", record.ToInt("Ministry_ID") },
                {"Group_ID", record.ToInt("Group_ID") },
                {"Start_Date", record.ToDate("Start_Date") },
                {"End_Date", record.ToDate("End_Date") },
                {"Frequency", record.ToString("Frequency") },
                {"Childcare_Session", record.ToString("Childcare_Session") },
                {"Notes", record.ToString("Notes") },
                {"Request_Status_ID", _childcareRequestStatusApproved }
            };

            _ministryPlatformService.UpdateRecord(_childcareRequestPageId, requestDict, apiToken);
        }


        public ChildcareRequest GetChildcareRequestForReview(int childcareRequestId)
        {
            var apiToken = _apiUserService.GetToken();

            var searchString = string.Format("{0},", childcareRequestId);
            var record = _ministryPlatformService.GetRecordDict(_childcareRequestPageId, childcareRequestId, apiToken);

            if (record == null)
            {
                return null;
            }
            var c = new ChildcareRequest
            {
                RequesterId = record.ToInt("Requester_ID"),
                LocationId = record.ToInt("Congregation_ID"),
                MinistryId = record.ToInt("Ministry_ID"),
                GroupId = record.ToInt("Group_ID"),
                GroupName = record.ToString("Group_ID_Text"),
                StartDate = record.ToDate("Start_Date"),
                EndDate = record.ToDate("End_Date"),
                Frequency = record.ToString("Frequency"),
                PreferredTime = record.ToString("Childcare_Session"),
                Notes = record.ToString("Notes")
            };

            return c;
        }
    }


}
