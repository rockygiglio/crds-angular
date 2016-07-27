using System;
using System.Collections.Generic;
using crds_angular.Models;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Models.Crossroads.Serve;
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
        ChildcareDashboardDto GetChildcareDashboard(Person person, HouseHoldData houseHoldData);
        void CancelRsvp(ChildcareRsvpDto cancelRsvp);
        HouseHoldData GetHeadsOfHousehold(int contactId, int householdId);
        void SendChildcareCancellationNotification();
        void UpdateChildcareRequest(ChildcareRequestDto request, string token);
        List<ChildCareDate> UpdateAvailableChildCareDates(List<ChildCareDate> currentDates, DateTime dateToAdd, bool hasBeenCancelled);
        void SendChildcareReminders();
    }
}
