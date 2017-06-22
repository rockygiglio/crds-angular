﻿using System.Runtime.Serialization;
using Crossroads.Web.Common;
using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinistryPlatform.Translation.Models.DTO
{
    [MpRestApiTable(Name = "cr_Preferred_Donation_Amounts")]
    public class PredefinedDonationAmountDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("domain_id")]
        public int DomainId { get; set; }
    }

}
