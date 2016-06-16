using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IBulkEmailRepository
    {
        List<MpBulkEmailPublication> GetPublications(string token);
        void UpdateLastSyncDate(string token, MpBulkEmailPublication publication);
        List<int> GetPageViewIds(string token, int publicationId);
        List<MpBulkEmailSubscriber> GetSubscribers(string token, int publicationId, List<int> pageViewIds);
        void UpdateSubscriber(string token, MpBulkEmailSubscriber subscriber);
        bool SetSubscriberStatus(string token, MpBulkEmailSubscriberOpt subscriberOpt);
    }
}
