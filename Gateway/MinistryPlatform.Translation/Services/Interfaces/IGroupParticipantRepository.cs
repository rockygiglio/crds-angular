using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGroupParticipantRepository
    {
        int Get(int groupId, int participantId); 
        List<MpGroupServingParticipant> GetServingParticipants(List<int> participants, long from, long to, int loggedInContactId);
    }
}