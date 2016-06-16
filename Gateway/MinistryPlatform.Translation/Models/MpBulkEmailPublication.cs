using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpBulkEmailPublication
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThirdPartyPublicationId { get; set; }
        public DateTime LastSuccessfulSync { get; set; }
    }
}
