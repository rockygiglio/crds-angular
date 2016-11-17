using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;


        public PaymentRepository(IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }

        public Result<MpPaymentDetailReturn> CreatePaymentAndDetail(MpPaymentDetail paymentInfo)
        {
            var apiToken = _apiUserRepository.GetToken();
            var paymentList = new List<MpPaymentDetail>
            {
                paymentInfo
            };
            var result =  _ministryPlatformRest.UsingAuthenticationToken(apiToken).PostWithReturn<MpPaymentDetail, MpPaymentDetailReturn>(paymentList);
            return result != null && result.Count > 0
                ? new Result<MpPaymentDetailReturn>(true, result.First())
                : new Result<MpPaymentDetailReturn>(false, "Unable to add new payment");
        }

        public List<MpPayment> GetPaymentsForInvoice(int invoiceId)
        {
            var apiToken = _apiUserRepository.GetToken();
            
            var parms = new Dictionary<string, object>
            {
                {"Invoice_Number", invoiceId }
            };
            var payments = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Get<MpPayment>("Payments", parms);

            return payments ?? new List<MpPayment>();
        }

        public void AddPaymentToBatch(int batchId, int paymentId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var parms = new Dictionary<string, object>
            {
                {"Payment_ID", paymentId},
                {"Batch_ID", batchId}
            };
            var parmList = new List<Dictionary<string, object>> {parms};

            try
            {
                _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put("Payments",parmList);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"AddPaymentToBatch failed. batchId: {batchId}, paymentId: {paymentId}", e);
            }
        }

        public int CreatePaymentBatch(string batchName, DateTime setupDateTime, decimal batchTotalAmount, int itemCount,
            int batchEntryType, int? depositId, DateTime finalizedDateTime, string processorTransferId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var batchRecord = new MpBatch
            {
                BatchName = batchName,
                SetupDate = setupDateTime,
                BatchTotal = batchTotalAmount,
                ItemCount = itemCount,
                BatchEntryTypeId = batchEntryType,
                DepositId = depositId,
                FinalizeDate = finalizedDateTime,
                ProcessorTransferId = processorTransferId
            };

            try
            {
                var batchId = _ministryPlatformRest.UsingAuthenticationToken(apiToken).PostWithReturn<MpBatch,MpBatch>(new List<MpBatch>() { batchRecord}).FirstOrDefault().BatchId;

                return (batchId);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"CreatPaymentBatch failed. batchName: {batchName}, setupDateTime: {setupDateTime}, batchTotalAmount: {batchTotalAmount}, itemCount: {itemCount}, batchEntryType: {batchEntryType}, depositId: {depositId}, finalizedDateTime: {finalizedDateTime}", e);
            }
        }
    }
}
