using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Sendgrid
{
    public class SendgridEvent
    {
        [JsonProperty(PropertyName = "sg_message_id")]
        public string SGMessageId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty(PropertyName = "smtp-id")]
        public string SmtpId { get; set; }

        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }
    }
}