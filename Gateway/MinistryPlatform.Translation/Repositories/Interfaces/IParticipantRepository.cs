using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        MpParticipant GetParticipant(int contactId);
        List<MpResponse> GetParticipantResponses(int participantId);
        MpParticipant GetParticipantRecord(string token);
        void UpdateParticipant(Dictionary<string, object> getDictionary);
        int CreateParticipantRecord(int contactId);
    }
}
