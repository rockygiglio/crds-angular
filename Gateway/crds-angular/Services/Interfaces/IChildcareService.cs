using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Serve;
using MinistryPlatform.Translation.Models.Childcare;

namespace crds_angular.Services.Interfaces
{
    public interface IChildcareService

    {
        void SendRequestForRsvp();
        List<FamilyMember> MyChildren(string token);
        void SaveRsvp(ChildcareRsvpDto saveRsvp, string token);
        void CreateChildcareRequest(ChildcareRequestDto request, String token);
        void ApproveChildcareRequest(int childcareRequestId, ChildcareRequestDto childcareRequest, string token);
        ChildcareRequest GetChildcareRequestForReview(int childcareRequestId, string token);
        void RejectChildcareRequest(int requestId, ChildcareRequestDto childcareRequest, string token);
    }
}
