using System;
using System.Collections.Generic;
using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models.Payments;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Result<MpPaymentDetailReturn> CreatePaymentAndDetail(MpPaymentDetail paymentInfo);
        List<MpPayment> GetPaymentsForInvoice(int invoiceId);
        MpPayment GetPaymentByTransactionCode(string stripePaymentId);
        int UpdateDonationStatus(int paymentId, int statusId, DateTime dateTime, string statusNote);
    }
}
