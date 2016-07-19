using System;
using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Childcare
{
    [MpRestApiTable(Name = "Childcare_Dashboard")]
    public class ChildcareDashboard
    {
        [JsonProperty(PropertyName = "Nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Group_ID")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "Group_Name")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "Group_Type_ID")]
        public int GroupTypeId { get; set; }

        [JsonProperty(PropertyName = "Event_ID")]
        public int EventID { get; set; }

        [JsonProperty(PropertyName = "Event_Title")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "Cancelled")]
        public bool Cancelled { get; set; }

        [JsonProperty(PropertyName = "Event_Start_Date")]
        public DateTime EventStartDate { get; set; }

        [JsonProperty(PropertyName = "Event_End_Date")]
        public DateTime EventEndDate { get; set; }

        [JsonProperty(PropertyName = "Congregation_ID")]
        public int CongregationID { get; set; }
    }
}