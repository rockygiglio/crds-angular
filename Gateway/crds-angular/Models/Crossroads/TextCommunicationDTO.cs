using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace crds_angular.Models.Crossroads
{
    public class TextCommunicationDto
    {
        [JsonProperty(PropertyName = "templateId")]
        public int TemplateId { get; set; }

        [JsonProperty(PropertyName = "mergeData")]
        public Dictionary<string, object> MergeData { get; set; }

        [JsonProperty(PropertyName = "toPhoneNumber")]
        public string ToPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "startDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }
    }
}