using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class PinDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("contactId")]
        public int Contact_ID { get; set; }

        [JsonProperty("participantId")]
        public int Participant_ID { get; set; }

        [JsonProperty("address")]
        public AddressDto Address { get; set; }

        [JsonProperty("hostStatus")]
        public int Host_Status { get; set; }

        [JsonProperty("gathering")]
        public GroupDto Gathering { get; set; }

        
  }
}