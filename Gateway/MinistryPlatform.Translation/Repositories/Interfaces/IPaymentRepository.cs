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
        MpPayment GetPaymentById(int paymentId);
        int UpdateDonationStatus(int paymentId, int statusId, DateTime dateTime, string statusNote);
        void AddPaymentToBatch(int batchId, int paymentId);
        int CreatePaymentBatch(string batchName,DateTime setupDateTime,decimal batchTotalAmount,int itemCount,int batchEntryType,int? depositId,DateTime finalizedDateTime,string processorTransferId);
    }
}
