using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Camp
{
    public class MyCampDTO
    {
        [JsonProperty(PropertyName = "Contact_ID")]
        public int CamperContactId { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string CamperNickName { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string CamperLastName { get; set; }

        [JsonProperty(PropertyName = "Event_Title")]
        public string CampName { get; set; }

        [JsonProperty(PropertyName = "Event_Start_Date")]
        public DateTime CampStartDate { get; set; }

        [JsonProperty(PropertyName = "Event_End_Date")]
        public DateTime CampEndDate { get; set; }
    }
}