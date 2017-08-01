using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using MinistryPlatform.Translation.PlatformService;
using Segment;
using Segment.Model;

namespace crds_angular.Services.Analytics
{
    public class AnalyticsAstronomer
    {
        private string applicationId = Environment.GetEnvironmentVariable("ASTRONOMER_APPLICATION_ID");

        public AnalyticsAstronomer()
        {
            Segment.Analytics.Initialize(applicationId, new Config().SetAsync(true));
        }

        public  void Track(string userId, string eventName, EventProperties props)
        {
            Properties segProps = mapProps(props);
            Segment.Analytics.Client.Track(userId, eventName, segProps);
            Segment.Analytics.Client.Flush();

        }

        private Properties mapProps(EventProperties eventProps)
        {
            var props = new Properties();
            foreach (KeyValuePair<string, object> p in eventProps)
            {
                props.Add(p.Key, p.Value);
            }
            return props;
        }
    }
}