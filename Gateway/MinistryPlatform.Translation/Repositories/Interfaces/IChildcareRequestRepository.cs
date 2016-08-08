using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Childcare;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IChildcareRequestRepository
    {
        int CreateChildcareRequest(MpChildcareRequest request);

        MpChildcareRequestEmail GetChildcareRequest(int childcareRequestId, string token);

        MpChildcareRequest GetChildcareRequestForReview(int childcareRequestId);

        void DecisionChildcareRequest(int childcareRequestId, int requestStatusId, MpChildcareRequest childcareRequest);
        void CreateChildcareRequestDates(int childcareRequestId, MpChildcareRequest mpRequest, string token);       
        List<MpChildcareRequestDate> GetChildcareRequestDates(int childcareRequestId);
        List<MpChildcareRequestDate> GetChildcareRequestDatesForReview(int childcareRequestId);
        List<MpEvent> FindChildcareEvents(int childcareRequestId, List<MpChildcareRequestDate> requestedDates, MpChildcareRequest request);
        void DecisionChildcareRequestDate(int childcareRequestDateId, bool decision);
        MpChildcareRequestDate GetChildcareRequestDates(int childcareRequestId, DateTime date, string token);
        void UpdateChildcareRequest(MpChildcareRequest request);
        void DeleteAllChildcareRequestDates(int childcareRequestId);
    }
}
