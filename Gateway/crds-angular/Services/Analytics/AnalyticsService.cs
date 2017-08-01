using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services.Analytics
{
    public class AnalyticsService: IAnalyticsService
    {
        public void Track(string userId, string eventName)
        {
            EventProperties props = new EventProperties();
            props.Add("Source", "CrossroadsNet");
            var astronomer = new AnalyticsAstronomer();
            astronomer.Track(userId, eventName, props);
        }

        public void Track(string userId, string eventName, EventProperties props)
        {
            props.Add("Source", "CrossroadsNet");
            var astronomer = new AnalyticsAstronomer();
            astronomer.Track(userId, eventName, props);
        }
    }
}