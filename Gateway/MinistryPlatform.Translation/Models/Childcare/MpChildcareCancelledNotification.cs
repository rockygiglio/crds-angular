using System;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class MpChildcareCancelledNotification
    {
        [JsonProperty(PropertyName = "Contact_ID")]
        public int EnrollerContactId { get; set; }

        [JsonProperty(PropertyName = "Email_Address")]
        public string EnrollerEmail { get; set; }

        [JsonProperty(PropertyName = "Enroller_Nickname")]
        public string EnrollerNickname { get; set; }

        [JsonProperty(PropertyName = "Group_Name")]
        public string EnrollerGroupName { get; set; }

        [JsonProperty(PropertyName = "Congregation_ID")]
        public int EnrollerGroupLocationId { get; set; }

        [JsonProperty(PropertyName = "Congregation_Name")]
        public string EnrollerGroupLocationName { get; set; }

        [JsonProperty(PropertyName = "Childcare_Contact")]
        public int ChildcareContactId { get; set; }

        [JsonProperty(PropertyName = "Childcare_Email")]
        public string ChildcareContactEmail { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string ChildNickname { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string ChildLastname { get; set; }

        [JsonProperty(PropertyName = "Event_Start_Date")]
        public DateTime ChildcareEventDate { get; set; }
    }
}