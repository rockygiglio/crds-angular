using System.Collections.Generic;
using System.Linq;
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

        public bool CreatePaymentAndDetail(MpPaymentDetail paymentInfo)
        {
            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Post(new List<MpPaymentDetail>(new List<MpPaymentDetail> {paymentInfo})) == 200;
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
    }
}
