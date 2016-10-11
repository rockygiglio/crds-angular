using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Camp_Event")]
    public class MpCampEvent
    {
        [JsonProperty(PropertyName = "Event_ID")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "Event_Title")]
        public int EventTitle { get; set; }

        [JsonProperty(PropertyName = "Event_Start_Date")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "Event_End_Date")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "Event_Type_ID")]
        public String EventType { get; set; }

        [JsonProperty(PropertyName = "Program_ID")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "Online_Registration_Product")]
        public int OnlineProductId { get; set; }

        [JsonProperty(PropertyName = "Registration_Start")]
        public DateTime RegistrationStartDate { get; set; }

        [JsonProperty(PropertyName = "Registration_End")]
        public DateTime RegistrationEndDate { get; set; }

    }
}
