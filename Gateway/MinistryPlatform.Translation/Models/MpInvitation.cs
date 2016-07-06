using System;
using System.Runtime.Serialization;

namespace MinistryPlatform.Translation.Models
{
    public class MpInvitation
    {
        public int SourceId { get; set; }

        public int GroupRoleId { get; set; }

        public string EmailAddress { get; set; }

        public string RecipientName { get; set; }

        public DateTime RequestDate { get; set; }

        public InvitationType InvitationType { get; set; }

    }

    public enum InvitationType
    {
        Group = 1,
        Trip = 2
    }
}