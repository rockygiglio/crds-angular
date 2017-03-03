using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class PinDto
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("contactId")]
        public int Contact_ID { get; set; }

        [JsonProperty("participantId")]
        public int Participant_ID { get; set; }

        [JsonProperty("address")]
        public AddressDTO Address { get; set; }

        [JsonProperty("hostStatus")]
        public int Host_Status { get; set; }

        [JsonProperty("gathering")]
        public GroupDTO Gathering { get; set; }

        [JsonProperty("householdId")]
        public int Household_ID { get; set; }

        
  }
}