using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IParticipantRepository
    {
        Participant GetParticipant(int contactId);
        List<MpResponse> GetParticipantResponses(int participantId);
        Participant GetParticipantRecord(string token);
        void UpdateParticipant(Dictionary<string, object> getDictionary);
        int CreateParticipantRecord(int contactId);
    }
}
