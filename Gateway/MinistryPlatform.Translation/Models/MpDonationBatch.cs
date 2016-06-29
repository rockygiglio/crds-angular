namespace MinistryPlatform.Translation.Models
{
    public class MpDonationBatch
    {
        public int Id { get; set; }
        public string ProcessorTransferId { get; set; }
        public int? DepositId { get; set; }
        public string BatchName { get; set; }
    }
}
