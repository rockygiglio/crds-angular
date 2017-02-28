using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Finder
{
    public class SpPinDto
    {
        [JsonProperty("firstName")]
        public string First_Name { get; set; }

        [JsonProperty("lastName")]
        public string Last_Name { get; set; }

        [JsonProperty("emailAddress")]
        public string Email_Address { get; set; }

        [JsonProperty("contactId")]
        public int Contact_ID { get; set; }

        [JsonProperty("participantId")]
        public int Participant_ID { get; set; }

        [JsonProperty("hostStatusId")]
        public int Host_Status_ID { get; set; }

        [JsonProperty("gathering")]
        public string Gathering { get; set; }

        [JsonProperty("householdId")]
        public int Household_ID { get; set; }

        [JsonProperty("siteName")]
        public string Site_Name{ get; set; }

        [JsonProperty("address_line_1")]
        public string Address_Line_1 { get; set; }

        [JsonProperty("address_line_2")]
        public string Address_Line_2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postalCode")]
        public int Postal_Code { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double? Longitude { get; set; }
    }
}
