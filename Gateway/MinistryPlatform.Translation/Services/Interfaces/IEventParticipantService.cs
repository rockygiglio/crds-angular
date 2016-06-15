using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventParticipantService
    {
        bool AddDocumentsToTripParticipant(List<TripDocuments> documents, int eventParticipantId);
        List<TripParticipant> TripParticipants(string search);
        List<EventParticipant> GetChildCareParticipants(int daysBeforeEvent);
        List<EventParticipant> GetEventParticipants(int eventId, int? roomId = null);
    }
}
