using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Finder
{
    [MpRestApiTable(Name = "Contacts")]
    public class FinderPinDto
    {
        public int Contact_ID { get; set; }
        public int Participant_ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonProperty("Email_Address")]
        public string EmailAddress { get; set; }

        public FinderAddressDto Address { get; set; }

    }
}
