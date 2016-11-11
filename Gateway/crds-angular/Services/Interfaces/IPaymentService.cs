
using crds_angular.Models.Crossroads.Payment;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Payments;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentService
    {
        MpPaymentDetailReturn PostPayment(MpDonationAndDistributionRecord payment);
        PaymentDetailDTO GetPaymentDetails(int paymentId, int invoiceId, string token);
    }
}