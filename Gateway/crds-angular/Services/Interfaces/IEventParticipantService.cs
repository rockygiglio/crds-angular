using crds_angular.Models.Crossroads.Events;

namespace crds_angular.Services.Interfaces
{
    public interface IEventParticipantService
    {
        EventParticipantDTO GetEventParticipantByContactAndEvent(int contactId, int eventId);
        bool IsParticipantInvalid(EventParticipantDTO participant );
    }
}