using System;
using System.Collections.Generic;
using Quartz;

namespace crds_angular.Models.Crossroads
{
    public class ScheduledJob
    {
        public DateTime StartDateTime { get; set; }
        public Type JobType { get; set; }
        public Object Dto { get; set; }
    }
}