using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampReservationDTO
    {
        [JsonProperty(PropertyName = "contactId")]
        public int? ContactId { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "preferredName")]
        public string PreferredName { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public int Gender { get; set; }

        [JsonProperty(PropertyName = "currentGrade")]
        public string CurrentGrade { get; set; }

        [JsonProperty(PropertyName = "schoolAttending")]
        public string SchoolAttending { get; set; }

        [JsonProperty(PropertyName = "schoolAttendingNext")]
        public string SchoolAttendingNext { get; set; }

        [JsonProperty(PropertyName = "crossroadsSite")]
        public int CrossroadsSite { get; set; }

        [JsonProperty(PropertyName = "roommate")]
        public string RoomMate { get; set; }
    }
}

