using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampReservationDTO
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "preferredName")]
        public string PreferredName { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public int Gender { get; set; }

        [JsonProperty(PropertyName = "currentGrade")]
        public string CurrentGrade { get; set; }

        [JsonProperty(PropertyName = "schoolAttending")]
        public string SchoolAttending { get; set; }

        [JsonProperty(PropertyName = "schoolAttendingNext")]
        public string SchoolAttendingNext { get; set; }

        [JsonProperty(PropertyName = "crossroadsSite")]
        public string CrossroadsSite { get; set; }

        [JsonProperty(PropertyName = "roommate")]
        public string RoomMate { get; set; }

        [JsonProperty(PropertyName = "EmergencyContactFirstName")]
        public string EmergencyContactFirstName { get; set; }

        [JsonProperty(PropertyName = "EmergencyContactLastName")]
        public string EmergencyContactLastName { get; set; }

        [JsonProperty(PropertyName = "EmergecyContactMobilePhone")]
        public string EmergecyContactMobilePhone { get; set; }

        [JsonProperty(PropertyName = "EmergencyContactEmail")]
        public string EmergencyContactEmail { get; set; }

        [JsonProperty(PropertyName = "EmergencyContactRelationship")]
        public string EmergencyContactRelationship { get; set; }
    }
}