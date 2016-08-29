using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpInvitation
    {
        public int InvitationId { get; set; }

        public int SourceId { get; set; }

        public int GroupRoleId { get; set; }

        public string EmailAddress { get; set; }

        public string RecipientName { get; set; }

        public DateTime RequestDate { get; set; }

        public int InvitationType { get; set; }

        public string InvitationGuid { get; set; }

        public string CustomMessage { get; set; }
    }

}