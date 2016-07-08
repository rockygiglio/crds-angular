using crds_angular.Models.Crossroads;
using System.Collections.Generic;
using Event = crds_angular.Models.Crossroads.Events.Event;
using crds_angular.Models.Crossroads.Groups;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupService
    {
        GroupDTO GetGroupDetails(int groupId);

        GroupDTO getGroupDetails(int groupId, int contactId, Participant participant, string authUserToken);

        void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants);

        List<Event> GetGroupEvents(int groupId, string token = null);

        List<GroupContactDTO> GetGroupMembersByEvent(int groupId, int eventId, string recipients);
		
        GroupDTO CreateGroup(GroupDTO group);
		
        List<GroupDTO> GetGroupsByTypeForParticipant(string token, int participantId, int groupTypeId);

        Participant GetParticipantRecord(string token);

        void SendJourneyEmailInvite(EmailCommunicationDTO email, string token);

        List<GroupParticipantDTO> GetGroupParticipants(int groupId);

        void LookupParticipantIfEmpty(string token, List<ParticipantSignup> partId);

        List<GroupDTO> GetSmallGroupsForAuthenticatedUser(string token);
    }
}
