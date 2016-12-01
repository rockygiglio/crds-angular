
using MinistryPlatform.Translation.Models.Payments;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IPaymentTypeRepository
    {
        MpPaymentType GetPaymentType(int paymentTypeId);
        bool PaymentTypeExists(int paymentTypeId);
    }
}
