using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Payments;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        bool CreatePaymentAndDetail(MpPaymentDetail paymentInfo);
        List<MpPayment> GetPaymentsForInvoice(int invoiceId);
    }
}
