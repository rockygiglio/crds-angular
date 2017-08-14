﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Analytics;

namespace crds_angular.Services.Interfaces
{
    public interface IAnalyticsService
    {
        void Track(string userId, string eventName);
        void Track(string userId, string eventName, EventProperties props);
    }
}