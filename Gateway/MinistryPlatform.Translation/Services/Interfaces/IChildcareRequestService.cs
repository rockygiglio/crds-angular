using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Childcare;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IChildcareRequestService
    {
        int CreateChildcareRequest(ChildcareRequest request);

        ChildcareRequestEmail GetChildcareRequest(int childcareRequestId, string token);

        ChildcareRequest GetChildcareRequestForReview(int childcareRequestId);

        void DecisionChildcareRequest(int childcareRequestId, int requestStatusId);
        Dictionary<int, int> FindChildcareEvents(int childcareRequestId, List<ChildcareRequestDate> requestedDates);
        List<ChildcareRequestDate> GetChildcareRequestDates(int childcareRequestId);
        void AddGroupToChildcareEvents(int childcareRequestId, int groupId, ChildcareRequestDate childcareDate);
        void DecisionChildcareRequestDate(int childcareRequestDateId, bool decision);
    }
}
