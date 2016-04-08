using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GoVolunteer
{
    public class Registrant
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "dob")]
        public string DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string MobilePhone { get; set; }

        public Dictionary<string, object> GetDictionary()
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Contact_ID", ContactId},
                {"Date_Of_Birth", DateOfBirth},
                {"Email_Address", EmailAddress},
                {"Nickname", FirstName},
                {"Last_Name", LastName},
                {"Mobile_Phone", MobilePhone}
            };
            return dictionary;
        }
    }
}