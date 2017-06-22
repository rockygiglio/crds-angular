﻿using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.FunctionalHelpers;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfigurationWrapper _configurationWrapper;

        private readonly int _invoiceCancelled;

        public InvoiceRepository(IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository, IProductRepository productRepository, IConfigurationWrapper configurationWrapper)
        {
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
            _productRepository = productRepository;
            _configurationWrapper = configurationWrapper;
            _invoiceCancelled = _configurationWrapper.GetConfigIntValue("InvoiceCancelled");
        }
        public bool InvoiceExists(int invoiceId)
        {
           return GetInvoice(invoiceId) != null;
        }

        public bool InvoiceExistsForEventParticipant(int eventParticipantId)
        {           
            var filter = new Dictionary<string, object> { { "Event_Participant_ID", eventParticipantId }};
            var apiToken = _apiUserRepository.GetToken();
            var invoices =_ministryPlatformRest.UsingAuthenticationToken(apiToken).Get<MpInvoiceDetail>("Invoice_Detail", filter);
            return invoices.Where(i => i.InvoiceStatusId != _invoiceCancelled).ToList().Any();
        }

        public void SetInvoiceStatus(int invoiceId, int statusId)
        {
            var dict = new Dictionary<string, object> {{"Invoice_ID", invoiceId}, {"Invoice_Status_ID", statusId}};

            var update = new List<Dictionary<string, object>> {dict};

            var apiToken = _apiUserRepository.GetToken();
            _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put("Invoices",update);
        }

        public MpInvoice GetInvoice(int invoiceId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var filter = $"Invoice_ID={invoiceId} AND Invoice_Status_ID!={_invoiceCancelled}";

            var resultLst = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpInvoice>(filter);
            var blah = resultLst.Any();
            return blah ? resultLst.First() : null;
        }

        public MpInvoiceDetail GetInvoiceDetailForInvoice(int invoiceId)
        {
            var filter = new Dictionary<string, object> {{"Invoice_ID", invoiceId}};

            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Get<MpInvoiceDetail>("Invoice_Detail", filter).FirstOrDefault();
        }

        public bool CreateInvoiceAndDetail(int productId, int? productOptionPriceId, int purchaserContactId, int recipientContactId, int eventParticipantId)
        {
            var product = _productRepository.GetProduct(productId);
            var productOptionPrice = productOptionPriceId != null ?_productRepository.GetProductOptionPrice((int)productOptionPriceId).OptionPrice : 0;

            var invoice = new MpNestedInvoiceDetail
            {
                Invoice = new MpInvoice()
                {
                    InvoiceDate = DateTime.Now,
                    InvoiceTotal = product.BasePrice + productOptionPrice,
                    PurchaserContactId = purchaserContactId,
                    InvoiceStatusId = 1
                },
                ProductId = productId,
                Quantity = 1,
                ProductOptionPriceId = productOptionPriceId,
                LineTotal = product.BasePrice + productOptionPrice,
                RecipientContactId = recipientContactId,
                EventParticipantId = eventParticipantId
            };
            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Post(new List<MpNestedInvoiceDetail>(new List<MpNestedInvoiceDetail> { invoice })) == 200;
        }

        public Result<MpInvoiceDetail> GetInvoiceDetailsForProductAndCamper(int productId, int camperId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var invoiceDetails = _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<MpInvoiceDetail>($"Recipient_Contact_ID_Table.[Contact_ID]={camperId} AND Product_ID_Table.[Product_ID]={productId} AND Invoice_Status_ID!={_invoiceCancelled}", "Invoice_ID_Table.[Invoice_ID]");
            if (invoiceDetails.Any())
            {
                if (invoiceDetails.First() != null)
                {
                    return new Result<MpInvoiceDetail>(true, invoiceDetails.First());
                }
            }
            return new Result<MpInvoiceDetail>(false, "no invoice details for that user and product");
        }

        public int GetInvoiceIdForPayment(int paymentId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var searchString = $"Payment_ID_Table.[Payment_ID]={paymentId}";
            var column = "Invoice_Detail_ID_Table_Invoice_ID_Table.[Invoice_ID]";
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Search<int>("Payment_Detail", searchString, column, null, false);
        }
    }
}
