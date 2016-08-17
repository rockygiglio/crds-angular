using crds_angular.Models.Crossroads;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupToolService
    {
        List<Invitation> GetInvitations(int sourceId, int invitationType, string token);
        List<Inquiry> GetInquiries(int groupId, string token);

        void RemoveParticipantFromMyGroup(string token, int groupTypeId, int groupId, int groupParticipantId, string message = null);
        void ApproveDenyInquiryFromMyGroup(string token, int groupTypeId, int groupId, bool approve, Inquiry inquiry, string message = null);
        void AcceptDenyGroupInvitation(string token, int groupId, string invitationGuid, bool approve);

        void SendGroupParticipantEmail(int groupId,
                                       int participantId,
                                       bool isGroupParticipantId,
                                       GroupDTO group,
                                       int emailTemplateId,
                                       string subjectTemplateContentBlockTitle = null,
                                       string emailTemplateContentBlockTitle = null,
                                       string message = null,
                                       Participant fromParticipant = null);

        MyGroup VerifyCurrentUserIsGroupLeader(string token, int groupTypeId, int groupId);
	    void SendAllGroupParticipantsEmail(string token, int groupId, int groupTypeId, string subject, string message);
        void SendAllGroupLeadersEmail(string token, int groupId, GroupMessageDTO message);
        List<GroupDTO> SearchGroups(int groupTypeId, string keywords = null, string location = null);
        void SubmitInquiry(string token, int groupId);
        void EndGroup(int groupId, int reasonEndedId);
        void SendGroupEndedParticipantEmail(GroupParticipantDTO participant);
    }
}
