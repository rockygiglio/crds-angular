using System.Collections.Generic;

namespace MinistryPlatform.Translation.Models
{
    public class MpBulkEmailSubscriber
    {
        public int ContactPublicationId { get; set; }
        public int ContactId { get; set; }
        public string ThirdPartyContactId { get; set; }
        public string EmailAddress { get; set; }
        public bool Subscribed { get; set; }
        public Dictionary<string, string> MergeFields { get; private set; }

        public MpBulkEmailSubscriber()
        {
            MergeFields = new Dictionary<string, string>();
        }

        public MpBulkEmailSubscriber Clone()
        {
            return (MpBulkEmailSubscriber) MemberwiseClone();
        }
    }
}