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

        void SendGroupParticipantEmail(int groupId, int groupParticipantId, GroupDTO group, int emailTemplateId, string emailTemplateContentBlockTitle = null, string customMessage = null, Participant fromParticipant = null);
        void SendAllGroupParticipantsEmail(string token, int groupId, int groupTypeId, string subject, string message);
    }
}
