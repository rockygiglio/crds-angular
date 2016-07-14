namespace MinistryPlatform.Translation.Models
{
    public class MpMessageTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }       
        public int FromContactId { get; set; }
        public string FromEmailAddress { get; set; }
        public int ReplyToContactId { get; set; }
        public string ReplyToEmailAddress { get; set; }
    }
}