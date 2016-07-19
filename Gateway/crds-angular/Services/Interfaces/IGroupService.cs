using System;
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

        void addParticipantToGroupNoEvents(int groupId, ParticipantSignup participant);

        void addParticipantsToGroup(int groupId, List<ParticipantSignup> participants);

        void endDateGroupParticipant(int groupId, int groupParticipantId);

        List<Event> GetGroupEvents(int groupId, string token = null);

        List<GroupContactDTO> GetGroupMembersByEvent(int groupId, int eventId, string recipients);
		
        GroupDTO CreateGroup(GroupDTO group);

        List<GroupDTO> GetGroupsForParticipant(string token, int participantId);

        List<GroupDTO> GetGroupsByTypeForParticipant(string token, int participantId, int groupTypeId);

        Participant GetParticipantRecord(string token);

        void SendJourneyEmailInvite(EmailCommunicationDTO email, string token);

        List<GroupParticipantDTO> GetGroupParticipants(int groupId, bool active = true);

        void LookupParticipantIfEmpty(string token, List<ParticipantSignup> partId);

        List<GroupDTO> GetSmallGroupsForAuthenticatedUser(string token);

        List<GroupDTO> GetGroupsByTypeForAuthenticatedUser(string token, int groupTypeId, int? groupId = null);
        void SendAllGroupParticipantsEmail(string token, int groupId, string subject, string message);
    }
}
