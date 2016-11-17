using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.FunctionalHelpers;
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

        public MpPayment GetPaymentByTransactionCode(string stripePaymentId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var searchString = $"Transaction_Code='{stripePaymentId}'";
            var payment = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpPayment>(searchString);
            return payment.FirstOrDefault() ?? new MpPayment();
        }

        public MpPayment GetPaymentById(int paymentId)
        {
            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Get<MpPayment>(paymentId);
        }

        public int UpdateDonationStatus(int paymentId, int statusId, DateTime dateTime, string statusNote)
        {
            var apiToken = _apiUserRepository.GetToken();

            var payment = GetPaymentById(paymentId);
            payment.PaymentStatus = statusId;

            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put<MpPayment>(new List<MpPayment> {payment});
        }
    }
}
