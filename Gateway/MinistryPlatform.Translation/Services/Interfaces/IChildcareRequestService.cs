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

        void ApproveChildcareRequest(int childcareRequestId);
    }
}
