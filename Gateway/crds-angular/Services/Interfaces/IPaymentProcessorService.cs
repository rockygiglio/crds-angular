using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IPaymentProcessorService
    {
        StripeCustomer CreateCustomer(string customerToken, string donorDescription = null, string Email =null, string DisplayName = null);  
        StripeCustomer GetCustomer(string customerId);
        StripeCustomer DeleteCustomer(string customerId);
        StripeToken CreateToken(string accountNumber, string routingNumber, string accountHolderName);
        StripeCharge ChargeCustomer(string customerToken, decimal amount, int donorId, bool isPayment, string Email = null, string DisplayName = null);  
        StripeCharge ChargeCustomer(string customerToken, string customerSourceId, decimal amount, int donorId, string checkNumber, string Email = null, string DisplayName = null);
        string UpdateCustomerDescription(string customerToken, int donorId);
        SourceData UpdateCustomerSource(string customerToken, string cardToken);
        SourceData GetDefaultSource(string customerToken);
        SourceData GetSource(string customerId, string sourceId);

        List<StripeCharge> GetChargesForTransfer(string transferId);
        StripeRefund GetChargeRefund(string chargeId);
        StripeRefundData GetRefund(string refundId);
        StripeCharge GetCharge(string chargeId);        
        StripePlan CreatePlan(RecurringGiftDto recurringGiftDto, MpContactDonor mpContactDonor, string Email = null, string DisplayName = null); 
        StripeSubscription CreateSubscription(string planName, string customer, DateTime trialEndDate, string Email = null, string DisplayName = null); 
        StripeSubscription UpdateSubscriptionPlan(string customerId, string subscriptionId, string planId, DateTime? trialEndDate = null);
        StripeSubscription UpdateSubscriptionTrialEnd(string customerId, string subscriptionId, DateTime? trialEndDate);
        StripeSubscription GetSubscription(string customerId, string subscriptionId);
        StripeCustomer AddSourceToCustomer(string customerToken, string cardToken);
        StripeSubscription CancelSubscription(string customerId, string subscriptionId);
        StripePlan CancelPlan(string planId);
    }
}
