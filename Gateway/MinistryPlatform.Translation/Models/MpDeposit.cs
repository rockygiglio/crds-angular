using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpDeposit
    {
        public int Id { get; set; }
        public string DepositName { get; set; }
        public decimal DepositTotalAmount { get; set; }
        public DateTime DepositDateTime { get; set; }
        public int BatchCount { get; set; }
        public bool Exported { get; set; }
        public string ProcessorTransferId { get; set; }
    }
}