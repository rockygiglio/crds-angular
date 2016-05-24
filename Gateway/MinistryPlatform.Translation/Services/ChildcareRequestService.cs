using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
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

        public ChildcareRequestService(IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService)
        {
            _ministryPlatformService = ministryPlatformService;
            _apiUserService = apiUserService;
            _childcareRequestPageId = configurationWrapper.GetConfigIntValue("ChildcareRequestPageId");
            _childcareRequestStatusPending = configurationWrapper.GetConfigIntValue("ChildcareRequestPending");
        }

        public void CreateChildcareRequest(ChildcareRequest request)
        {
            var apiToken = _apiUserService.GetToken();
            
            var requestDict = new Dictionary<string, object>
            {
                {"Requester_ID", request.RequesterId},
                {"Congregation_ID", request.LocationId },
                {"Ministry_ID", request.MinistryId },
                {"Group_ID", request.GroupId },
                {"Childcare_Start_Date", request.StartDate },
                {"Childcare_End_Date", request.EndDate },
                {"Frequency", request.Frequency },
                {"Time_Frame", request.PreferredTime },
                {"No_of_Children_Attending", request.EstimatedChildren },
                {"Notes", request.Notes },
                {"Request_Status_ID", _childcareRequestStatusPending }
            };
            _ministryPlatformService.CreateRecord(_childcareRequestPageId, requestDict, apiToken);
        }
    }
}
