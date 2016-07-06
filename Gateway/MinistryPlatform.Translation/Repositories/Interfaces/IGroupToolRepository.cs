using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IGroupToolRepository
    {
        List<MpInvitation> GetInvitees(int GroupId);

        //        int CreateGroup(MpGroup group);
        //
        //        int addParticipantToGroup(int participantId,
        //                                  int groupId,
        //                                  int groupRoleId,
        //                                  Boolean childCareNeeded,
        //                                  DateTime startDate,
        //                                  DateTime? endDate = null,
        //                                  Boolean? employeeRole = false);
        //
        //        MpGroup getGroupDetails(int groupId);
        //
        //        bool checkIfUserInGroup(int participantId, IList<MpGroupParticipant> participants);
        //
        //
        //        void UpdateGroupRemainingCapacity(MpGroup group);
        //
        //        List<MpGroupParticipant> GetGroupParticipants(int groupId);

    }
}