using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public enum PinType
    {
        PERSON = 1,
        GATHERING = 2,
        SITE = 3 
    };

    public class PinDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("contactId")]
        public int? Contact_ID { get; set; }

        [JsonProperty("participantId")]
        public int? Participant_ID { get; set; }

        [JsonProperty("address")]
        public AddressDTO Address { get; set; }

        [JsonProperty("hostStatus")]
        public int? Host_Status_ID { get; set; }

        [JsonProperty("gathering")]
        public GroupDTO Gathering { get; set; }

        [JsonProperty("householdId")]
        public int? Household_ID { get; set; }

        [JsonProperty("isFormDirty")]
        public bool isFormDirty { get; set; }

        [JsonProperty("pinType")]
        public PinType PinType { get; set;  }

        [JsonProperty("proximity")]
        public decimal? Proximity { get; set; }
    }
}