using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        int CreateGroup(MpGroup group);

        int addParticipantToGroup(int participantId,
                                  int groupId,
                                  int groupRoleId,
                                  Boolean childCareNeeded,
                                  DateTime startDate,
                                  DateTime? endDate = null,
                                  Boolean? employeeRole = false);

        IList<MpEvent> getAllEventsForGroup(int groupId);

        MpGroup getGroupDetails(int groupId);

        bool checkIfUserInGroup(int participantId, IList<MpGroupParticipant> participants);

        bool checkIfRelationshipInGroup(int relationshipId, IList<int> currRelationshipList);

        List<MpGroupSignupRelationships> GetGroupSignupRelations(int groupType);

        bool ParticipantQualifiedServerGroupMember(int groupId, int participantId);
        bool ParticipantGroupMember(int groupId, int participantId);

        List<MpGroup> GetGroupsForEvent(int eventId);

        void SendCommunityGroupConfirmationEmail(int participantId, int groupId, bool waitlist, bool childcareNeeded);
        List<MpGroupParticipant> getEventParticipantsForGroup(int groupId, int eventId);

        IList<string> GetEventTypesForGroup(int groupId, string token = null);

        List<MpGroup> GetGroupsByTypeForParticipant(string token, int participantId, int groupTypeId);

        void UpdateGroupRemainingCapacity(MpGroup group);

        List<MpGroupParticipant> GetGroupParticipants(int groupId, Boolean active);

        List<MpGroupSearchResult> GetSearchResults(int groupTypeId);

        List<MpGroup> GetSmallGroupsForAuthenticatedUser(string userToken);
        void endDateGroupParticipant(int participantId, int groupId, DateTime? endDate = null);
    }
}
