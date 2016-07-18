using crds_angular.Models.Crossroads;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupToolService
    {
        List<Invitation> GetInvitations(int sourceId, int invitationType, string token);
        List<Inquiry> GetInquiries(int groupId, string token);

        void RemoveParticipantFromMyGroup(string token, int groupTypeId, int groupId, int groupParticipantId, string message = null);

        void SendGroupParticipantEmail(int groupId, int groupParticipantId, GroupDTO group, int emailTemplateId, string emailTemplateContentBlockTitle, string customMessage = null);
    }
}
