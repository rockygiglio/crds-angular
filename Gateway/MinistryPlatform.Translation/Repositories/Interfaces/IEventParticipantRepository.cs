using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IEventParticipantRepository
    {
        bool AddDocumentsToTripParticipant(List<MpTripDocuments> documents, int eventParticipantId);
        List<MpTripParticipant> TripParticipants(string search);
        List<MpEventParticipant> GetChildCareParticipants(int daysBeforeEvent);
        List<MpEventParticipant> GetEventParticipants(int eventId, int? roomId = null);
        int GetEventParticipantByContactId(int eventId, int contactId);
        DateTime? EventParticipantSignupDate(int contactId , int eventId , string apiToken);
    }
}
