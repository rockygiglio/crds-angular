using System;

namespace MinistryPlatform.Translation.Exceptions
{
    public class DonationNotFoundException : ApplicationException
    {
        public DonationNotFoundException(string chargeId) : base(string.Format("Could not locate donation for charge {0}", chargeId))
        {
        }
    }
}
