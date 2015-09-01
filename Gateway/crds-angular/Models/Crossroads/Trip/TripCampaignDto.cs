﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripCampaignDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "formId")]
        public int FormId { get; set; }
        
        [JsonProperty(PropertyName = "formName")]
        public string FormName { get; set; }

        [JsonProperty(PropertyName = "ageLimit")]
        public int YoungestAgeAllowed { get; set; }

        [JsonProperty(PropertyName = "ageExceptions")]
        public List<int> AgeExceptions { get; set; } 
    }
}