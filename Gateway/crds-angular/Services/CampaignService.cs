using System;
using crds_angular.Services.Interfaces;
using crds_angular.Models.Crossroads.Campaign;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IApiUserRepository _apiUserService;
        private readonly ICampaignRepository _campaignRepository;

        public CampaignService(IApiUserRepository apiUserService, ICampaignRepository campaignRepository)
        {
            _apiUserService = apiUserService;
            _campaignRepository = campaignRepository;
        }

        public PledgeCampaignSummaryDto GetSummary(int pledgeCampaignId)
        {
            var token = _apiUserService.GetToken();
            var campaignSummary = _campaignRepository.GetPledgeCampaignSummary(token, pledgeCampaignId);

            int totalDays = DaysInRange(campaignSummary.StartDate, campaignSummary.EndDate);
            int currentDay = DaysInRange(campaignSummary.StartDate, DateTime.Now);

            // clip to end date
            currentDay = Math.Min(currentDay, totalDays);

            return new PledgeCampaignSummaryDto
            {
                PledgeCampaignId = campaignSummary.PledgeCampaignId,
                TotalGiven = campaignSummary.TotalGiven,
                TotalCommitted = campaignSummary.TotalCommitted,
                CurrentDays = currentDay,
                TotalDays = totalDays,
                NotStartedPercent = ToPercentage(campaignSummary.NotStartedCount, campaignSummary.TotalCount),
                OnPacePercent = ToPercentage(campaignSummary.OnPaceCount, campaignSummary.TotalCount),
                CompletedPercent = ToPercentage(campaignSummary.CompletedCount, campaignSummary.TotalCount),
            };
        }

        private int DaysInRange(DateTime startDate, DateTime endDate)
        {
            int totalDays = (int) (endDate.Date - startDate.Date).TotalDays + 1;
            return Math.Max(0, totalDays);
        }

        private int ToPercentage(int numerator, int denominator)
        {
            int percent = (int) Math.Round(100.0 * numerator / denominator);
            return Math.Max(0, Math.Min(percent, 100));
        }
    }
}
