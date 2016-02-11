using System;
using System.Linq;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using CrossroadsStripeOnboarding.Models;
using log4net;
using log4net.Appender;
using crds_angular.Models.Json;

namespace CrossroadsStripeOnboarding.Services
{
    public class FixSubscriptionCycle
    {
        private const string Null = "NULL";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(FixSubscriptionCycle));
        private static readonly ILog FixSubOutput = LogManager.GetLogger("FIX_SUB_OUTPUT");

        private readonly MinistryPlatformContext _mpContext;
        private readonly IPaymentService _paymentService;

        public FixSubscriptionCycle(MinistryPlatformContext mpContext, IPaymentService paymentService)
        {
            _mpContext = mpContext;
            _paymentService = paymentService;
        }

        public void Fix()
        {
            var appender = FixSubOutput.Logger.Repository.GetAppenders().First(x => x.Name.Equals("FixSubOutputLog"));

            Logger.Info(string.Format("Starting fix subscription process - CSV results will be written to {0}", ((FileAppender)appender).File));
            var recurringGifts = _mpContext.RecurringGifts.ToList().Where(gift => !gift.End_Date.HasValue).ToList();
            var giftsToProcess = recurringGifts.Count();
            Logger.Info(string.Format("Fixing {0} recurring gifts", giftsToProcess));

            FixSubOutput.Info("mpRecurringGiftId,stripeSubscriptionId,mpAmount,stripeAmount,mpFrequency,stripeFrequency,mpRepeat,stripeRepeat,success,errorMessage");

            var giftsProcessed = 0;

            foreach (var gift in recurringGifts)
            {
                var percentComplete = (int)Math.Round((double)(++giftsProcessed * 100.0) / giftsToProcess);

                Logger.Info(string.Format("Processing gift #{0} ({1}% complete)", giftsProcessed, percentComplete));

                var success = true;
                var errorMessage = string.Empty;

                StripeSubscription sub = null;
                var mpRecurringGiftId = gift.Recurring_Gift_ID;
                var mpAmount = gift.Amount;
                var mpFrequency = gift.Frequency.ToString();
                var mpRepeat = gift.Frequency == Frequency.Monthly ? gift.Day_Of_Month + "" : gift.DayOfWeek.ToString();

                if (gift.DonorAccount == null || string.IsNullOrWhiteSpace(gift.DonorAccount.Processor_ID) || string.IsNullOrWhiteSpace(gift.Subscription_ID))
                {
                    success = false;
                    errorMessage = "Recurring gift is missing Stripe processor information";
                }
                else
                {
                    try
                    {
                        sub = _paymentService.GetSubscription(gift.DonorAccount.Processor_ID, gift.Subscription_ID);
                        if (sub == null || string.IsNullOrWhiteSpace(sub.Id))
                        {
                            success = false;
                            errorMessage = string.Format("Stripe problem: Nothing returned for subscription {0} for customer {1}", gift.Subscription_ID, gift.DonorAccount.Processor_ID);
                        }
                    }
                    catch (Exception e)
                    {
                        success = false;
                        errorMessage = string.Format("Stripe exception: Could not lookup subscription for customer {0} subscription {1}: {2}",
                                                     gift.DonorAccount.Processor_ID,
                                                     gift.Subscription_ID,
                                                     e.Message);
                        Logger.Error(errorMessage, e);
                    }
                }

                DateTime? trialEndDate = null;
                if (success)
                {
                    if ("trialing".Equals(sub.Status) && sub.TrialEnd.HasValue)
                    {
                        trialEndDate = sub.TrialEnd.Value.Date;
                    }
                    else
                    {
                        trialEndDate = CalculateTrialEndDate(gift);
                    }

                    if (!trialEndDate.HasValue)
                    {
                        success = false;
                        errorMessage = "Trial end date cannot be set this month; run again next month";
                    }
                }

                StripeSubscription newSub = null;
                if (success)
                {
                    try
                    {
                        newSub = _paymentService.UpdateSubscriptionTrialEnd(gift.DonorAccount.Processor_ID, gift.Subscription_ID, trialEndDate);
                    }
                    catch (Exception e)
                    {
                        success = false;
                        errorMessage = string.Format("Stripe exception: Could not update subscription for customer {0} subscription {1}: {2}",
                                                     gift.DonorAccount.Processor_ID,
                                                     gift.Subscription_ID,
                                                     e.Message);
                        Logger.Error(errorMessage, e);
                    }
                    
                }

                var stripeSubscriptionId = string.IsNullOrWhiteSpace(gift.Subscription_ID) ? Null : gift.Subscription_ID;
                var stripeAmount = newSub == null || newSub.Plan == null ? 0.00M : (decimal)(newSub.Plan.Amount / 100.00M);
                var freq = newSub == null || newSub.Plan == null
                    ? null
                    : newSub.Plan.Interval.ToLower().Equals("month") ? Frequency.Monthly : newSub.Plan.Interval.ToLower().Equals("week") ? Frequency.Weekly : (Frequency?)null;
                var stripeFrequency = freq == null ? Null : freq.ToString();
                var startDate = newSub == null || string.IsNullOrWhiteSpace(newSub.CurrentPeriodEnd) ? (DateTime?)null : StripeEpochTime.ConvertEpochToDateTime(long.Parse(newSub.CurrentPeriodEnd));
                var stripeRepeat = freq != null && freq == Frequency.Monthly && startDate != null
                    ? startDate.Value.Day + ""
                    : freq != null && freq == Frequency.Weekly && startDate != null ? startDate.Value.DayOfWeek.ToString() : Null;

                LogRecurringGiftUpdate(mpRecurringGiftId, stripeSubscriptionId, mpAmount, stripeAmount, mpFrequency, stripeFrequency, mpRepeat, stripeRepeat, success, errorMessage);
            }
        }

        private static DateTime? CalculateTrialEndDate(RecurringGift gift)
        {
            var today = DateTime.Today;
            return (gift.Frequency_ID == 1 ? GetStartDateForWeek(today, gift) : GetStartForMonth(today, gift));
        }

        private static DateTime? GetStartDateForWeek(DateTime today, RecurringGift gift)
        {
            var todayDay = (int) today.DayOfWeek;
            var giftDay = (int) gift.DayOfWeek;

            var days = todayDay == giftDay ? 7 : (giftDay - todayDay + 7) % 7;
            return today.AddDays(days);
        }

        private static DateTime? GetStartForMonth(DateTime today, RecurringGift gift)
        {
            var giftDay = gift.Day_Of_Month.Value;
            var months = today.Day < giftDay ? 0 : 1;

            var updatedGiftMonth = today.AddMonths(months);
            if (giftDay > DateTime.DaysInMonth(updatedGiftMonth.Year, updatedGiftMonth.Month))
            {
                // Can't update this gift, because the day of the gift isn't in the month of the gift, we'll try again next month
                return (null);
            }

            var giftDate = new DateTime(today.Year, today.Month, giftDay);
            return giftDate.AddMonths(months);
        }

        private static void LogRecurringGiftUpdate(int mpRecurringGiftId,
                                                  string stripeSubscriptionId,
                                                  decimal mpAmount,
                                                  decimal stripeAmount,
                                                  string mpFrequency,
                                                  string stripeFrequency,
                                                  string mpRepeat,
                                                  string stripeRepeat,
                                                  bool success,
                                                  string errorMessage)
        {
            FixSubOutput.Info(string.Format("{0},{1},{2:F2},{3:F2},{4},{5},{6},{7},{8},{9}",
                                            mpRecurringGiftId,
                                            stripeSubscriptionId,
                                            mpAmount,
                                            stripeAmount,
                                            mpFrequency,
                                            stripeFrequency,
                                            mpRepeat,
                                            stripeRepeat,
                                            success,
                                            errorMessage));
        }
    }
}
