using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;
using MpResponse = MinistryPlatform.Translation.Models.MpResponse;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IOpportunityRepository
    {

        MpOpportunity GetOpportunityById(int opportunityId, string token);
        int GetOpportunitySignupCount(int opportunityId, int eventId, string token);
        List<DateTime> GetAllOpportunityDates(int id, string token);
        MpGroup GetGroupParticipantsForOpportunity(int id, string token);
        DateTime GetLastOpportunityDate(int opportunityId, string token);
        int DeleteResponseToOpportunities(int participantId, int opportunityId, int eventId);
        int RespondToOpportunity(string token, int opportunityId, string comments);
        MpResponse GetMyOpportunityResponses(int contactId, int opportunityId);
        MpResponse GetOpportunityResponse(int contactId, int opportunityId);
        MpResponse GetOpportunityResponse(int opportunityId, int eventId, MpParticipant participant);
        List<Models.Opportunities.MpResponse> SearchResponseByGroupAndEvent(String searchString);
        List<Models.Opportunities.MpResponse> GetContactsOpportunityResponseByGroupAndEvent(int groupId, int eventId);
        List<MpResponse> GetOpportunityResponses(int opportunityId, string token);
        void RespondToOpportunity(MpRespondToOpportunityDto opportunityResponse);
        int RespondToOpportunity(int participantId, int opportunityId, string comments, int eventId, bool response);
    }
}
