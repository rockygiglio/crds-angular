using System;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class MPChildcareEmail
    {
        [JsonProperty(PropertyName = "Email_Address")]
        public String EmailAddress { get; set; }
    }
}