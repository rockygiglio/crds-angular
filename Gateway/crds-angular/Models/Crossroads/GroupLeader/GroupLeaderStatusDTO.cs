﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.GroupLeader
{
    public class GroupLeaderStatusDTO
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }
}