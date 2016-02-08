using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Subscription;

namespace crds_angular.Services.Interfaces
{
    public interface ISubscriptionsService
    {
        List<Dictionary<string, object>> GetSubscriptions(int contactId, string token);

        int SetSubscriptions(Dictionary<string, object> subscription, int contactId, string token);

        OptInResponse AddListSubscriber(string emailAddress, string listName);
    }
}