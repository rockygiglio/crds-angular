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
        private string ASTRONOMER_APPLICATION_ID = "sZQew9P77arhJe3Qx";

        public  void Track(string userId, string eventName, EventProperties props)
        {
            Properties segProps = mapProps(props);
            Segment.Analytics.Initialize(ASTRONOMER_APPLICATION_ID, new Config().SetAsync(false));
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