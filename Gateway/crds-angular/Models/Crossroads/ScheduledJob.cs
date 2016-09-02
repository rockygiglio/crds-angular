using System;
using System.Collections.Generic;

namespace crds_angular.Models.Crossroads
{
    public class ScheduledJob
    {
        public DateTime StartDateTime { get; set; }
        public Type JobType { get; set; }
        public Dictionary<string,object> Dto { get; set; }
    }
}