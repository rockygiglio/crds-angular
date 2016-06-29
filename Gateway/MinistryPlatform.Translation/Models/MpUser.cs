namespace MinistryPlatform.Translation.Models
{
    public class MpUser
    {
        public string UserId { get; set; }
        public string Guid { get; set; }
        public bool CanImpersonate { get; set; }
        public string UserEmail { get; set; }
        public int UserRecordId { get; set; }
        public string DisplayName { get; set; }
    }
}
