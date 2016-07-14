using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    /// <summary>
    /// A private Group or Trip invitation.
    /// </summary>
    public class Invitation
    {
        /// <summary>
        /// The source of the invitation.  This should be the Group ID for a group invitation, or the Trip ID for a trip invitation.
        /// </summary>
        [JsonProperty(PropertyName = "sourceId")]
        [Required]
        public int SourceId { get; set; }

        /// <summary>
        /// The group role to invite a person to join with.  <b>Required for a Group invitation.</b> This should be the ID of the group role type, one of:<br/>
        /// <ul>
        /// <li>16 - Member</li>
        /// <li>22 - Leader</li>
        /// <li>66 - Apprentice</li>
        /// </ul>
        /// </summary>
        [JsonProperty(PropertyName = "groupRoleId")]
        public int GroupRoleId { get; set; }

        /// <summary>
        /// The email address to associate with the invitation.  An email will be sent to this address.
        /// </summary>
        [JsonProperty(PropertyName = "emailAddress")]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The name of the person receiving the invitation.
        /// </summary>
        [JsonProperty(PropertyName = "recipientName")]
        [Required]
        public string RecipientName { get; set; }

        /// <summary>
        /// The date the invitation is created.
        /// </summary>
        [JsonProperty(PropertyName = "requestDate")]
        [Required]
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// The ID of the type of invitation from the "Invitation Types" page. This is one of:
        /// <ul>
        /// <li>1 - Groups</li>
        /// <li>2 - Trips</li>
        /// </ul>
        /// </summary>
        [JsonProperty(PropertyName = "invitationType")]
        [Required]
        public int InvitationType { get; set; }


        /// <summary>
        /// The generated numeric ID of the invitation. This is returned when creating an invitation.
        /// </summary>
        [JsonProperty(PropertyName = "invitationId")]
        public int InvitationId { get; set; }

        /// <summary>
        /// The globally unique ID (GUID) assigned to this invitation.  This is used when accepting or declining an invitation. This is returned when creating an invitation.
        /// </summary>
        [JsonProperty(PropertyName = "invitationGuid")]
        public string InvitationGuid { get; set; }
    }

}