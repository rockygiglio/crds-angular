using System;

namespace MinistryPlatform.Translation.Exceptions
{
    public class PaymentNotFoundException : ApplicationException
    {
        public PaymentNotFoundException(string chargeId) : base(string.Format("Could not locate payment for charge {0}", chargeId))
        {
        }
    }
}
