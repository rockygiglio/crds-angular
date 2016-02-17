using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using crds_angular.Services.Interfaces;
using log4net;
using log4net.Appender;
using StripeCustomer = crds_angular.Models.Crossroads.Stewardship.StripeCustomer;

namespace CrossroadsStripeOnboarding.Services
{
    public class ListStripeSubscriptions
    {
        private const string Null = "NULL";

        private static readonly ILog Logger = LogManager.GetLogger(typeof (ListStripeSubscriptions));
        private static readonly ILog ListOutput = LogManager.GetLogger("LIST_OUTPUT");

        private readonly IPaymentService _paymentService;

        public ListStripeSubscriptions(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public void ListSubscriptions(string fileName)
        {
            var appender = ListOutput.Logger.Repository.GetAppenders().First(x => x.Name.Equals("ListOutputLog"));

            Logger.Info(string.Format("Starting list process - CSV results will be written to {0}", ((FileAppender)appender).File));
            ListOutput.Info("customerId,subscriptionId,paymentId,amount,last4,institution,accountType,interval,name,success,message");
            var file = new StreamReader(fileName);
            string line = null;
            while ((line = file.ReadLine()) != null)
            {
                StripeCustomer customer = null;
                try
                {
                    customer = _paymentService.GetCustomer(line);
                }
                catch (Exception e)
                {
                    LogSubscriptionLine(line, Null, Null, 0, Null, Null, Null, Null, Null, false, "Stripe Error: " + e.Message);
                    Logger.Error("Error looking up customer " + line, e);
                    continue;
                }

                if (customer == null)
                {
                    LogSubscriptionLine(line, Null, Null, 0, Null, Null, Null, Null, Null, false, "Customer not found in Stripe");
                    Logger.Warn("Skipping customer " + line + ", not found in Stripe");
                    continue;
                }

                if (customer.subscriptions == null || customer.subscriptions.data == null || !customer.subscriptions.data.Any())
                {
                    LogSubscriptionLine(line, Null, Null, 0, Null, Null, Null, Null, Null, false, "Customer subscriptions not found in Stripe");
                    Logger.Warn("Skipping customer " + line + ", subscriptions not found in Stripe");
                    continue;
                }

                foreach (var s in customer.subscriptions.data)
                {
                    var sub = (Dictionary<string, object>) s;
                    var plan = (Dictionary<string, object>)sub["plan"];
                    var defaultSource = customer.sources.data.Find(src => src.id.Equals(customer.default_source));
                    LogSubscriptionLine(line,
                                        sub["id"] as string,
                                        customer.default_source,
                                        plan["amount"] as long? ?? 0,
                                        defaultSource.bank_last4 ?? defaultSource.last4,
                                        defaultSource.@object.Equals("card") ? defaultSource.brand : "Bank",
                                        defaultSource.@object,
                                        plan["interval"] as string,
                                        plan["name"] as string,
                                        true,
                                        string.Empty);
                }
            }
        }

        private void LogSubscriptionLine(string customerId, string subscriptionId, string paymentId, long amount, string last4, string institution, string accountType, string interval, string name, bool success, string message)
        {
            ListOutput.Info(string.Format("{0},{1},{2},{3:F2},{4},{5},{6},{7},{8},{9},{10}", customerId, subscriptionId, paymentId, amount / 100.0, last4, institution, accountType, interval, name, success, message));
        }
    }
}
