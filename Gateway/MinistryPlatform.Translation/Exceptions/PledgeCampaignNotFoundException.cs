using System;

namespace MinistryPlatform.Translation.Exceptions
{
    public class PledgeCampaignNotFoundException : ApplicationException
    {
        public PledgeCampaignNotFoundException(int pledgeCampaignId) : base($"Could not locate pledge campaign {pledgeCampaignId}")
        {
        }
    }
}
