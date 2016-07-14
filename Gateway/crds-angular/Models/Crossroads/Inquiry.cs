using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    /// <summary>
    /// A private Group or Trip Inquiry.
    /// </summary>
    public class Inquiry
    {
        /// <summary>
        /// The Group ID for who the inquiry is associated to.
        /// </summary>
        [JsonProperty(PropertyName = "groupId")]
        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// The email address to associate with the inquirier.  An email will be sent to this address.
        /// </summary>
        [JsonProperty(PropertyName = "emailAddress")]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The phone number to associate with the inquirier.
        /// </summary>
        [JsonProperty(PropertyName = "phoneNumber")]
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The name of the person asking to join the group or trip.
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The name of the person asking to join the group or trip.
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The date the inquiry is created.
        /// </summary>
        [JsonProperty(PropertyName = "requestDate")]
        [Required]
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// True if they have been placed or denied.
        /// </summary>
        [JsonProperty(PropertyName = "placed")]
        [Required]
        public bool Placed { get; set; }


        /// <summary>
        /// The generated numeric ID of the inquirier's record. This is returned when creating an inquiry.
        /// </summary>
        [JsonProperty(PropertyName = "inquiryId")]
        public int InquiryId { get; set; }
    }

}