using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Opportunities;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IGroupParticipantRepository
    {
        int Get(int groupId, int participantId); 
        List<MpGroupServingParticipant> GetServingParticipants(List<int> participants, long from, long to, int loggedInContactId);
        List<MpGroup> GetAllGroupsLeadByParticipant(int participantId);
        List<MpRsvpMember> GetRsvpMembers(int groupId, int eventId);
        List<MpSU2SOpportunity> GetListOfOpportunitiesByEventAndGroup(int groupId, int eventId);
        int GetRsvpYesCount(int groupId, int eventId);
    }
}