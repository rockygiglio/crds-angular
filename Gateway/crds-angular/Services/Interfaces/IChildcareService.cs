using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Serve;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Childcare;

namespace crds_angular.Services.Interfaces
{
    public interface IChildcareService

    {
        void SendRequestForRsvp();
        List<FamilyMember> MyChildren(string token);
        void SaveRsvp(ChildcareRsvpDto saveRsvp);
        void CreateChildcareRequest(ChildcareRequestDto request, String token);
        void ApproveChildcareRequest(int childcareRequestId, ChildcareRequestDto childcareRequest, string token);
        MpChildcareRequest GetChildcareRequestForReview(int childcareRequestId, string token);
        void RejectChildcareRequest(int requestId, ChildcareRequestDto childcareRequest, string token);
        ChildcareDashboardDto GetChildcareDashboard(int contactId);
        void CancelRsvp(ChildcareRsvpDto cancelRsvp);
        Tuple<List<MpHouseholdMember>, List<MpHouseholdMember>> GetHeadsOfHousehold(int contactId, int householdId);
    }
}
