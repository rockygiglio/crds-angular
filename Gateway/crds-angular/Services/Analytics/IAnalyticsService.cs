using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services.Interfaces
{
    public interface IAnalyticsService
    {
        void Track(string userId, string eventName);
    }
}