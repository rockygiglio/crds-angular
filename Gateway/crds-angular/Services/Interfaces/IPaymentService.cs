
using crds_angular.Models.Crossroads.Payment;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentService
    {
        void PostPayment(PaymentDTO payment);
    }
}