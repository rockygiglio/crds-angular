using System;
using System.Linq;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads.Payment;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPaymentRepository _paymentRepository;

        private readonly int _paidinfullStatus;
        private readonly int _somepaidStatus;

        public PaymentService(IInvoiceRepository invoiceRepository, IPaymentRepository paymentRepository, IConfigurationWrapper configurationWrapper)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            
            _paidinfullStatus = configurationWrapper.GetConfigIntValue("PaidInFull");
            _somepaidStatus = configurationWrapper.GetConfigIntValue("SomePaid");
        }
        public void PostPayment(PaymentDTO paymentDto)
        {
            //check if invoice exists
            if (!_invoiceRepository.InvoiceExists(paymentDto.InvoiceId))
            {
                throw new InvoiceNotFoundException(paymentDto.InvoiceId);
            }

            //create payment -- send model
            var payment = new MpPayment
            {
                InvoiceNumber = paymentDto.InvoiceId.ToString(),
                ContactId = paymentDto.ContactId,
                TransactionCode = paymentDto.StripeTransactionId,
                PaymentDate = DateTime.Now,
                PaymentTotal = paymentDto.Amount,
                PaymentTypeId = paymentDto.PaymentTypeId
            };
            var paymentDetail = new MpPaymentDetail
            {
                Payment = payment,
                PaymentAmount = paymentDto.Amount,
                InvoiceDetailId = _invoiceRepository.GetInvoiceDetailForInvoice(paymentDto.InvoiceId).InvoiceDetailId
                
            };

            //returns boolean
            _paymentRepository.CreatePaymentAndDetail(paymentDetail);

            //update invoice payment status
            var invoice = _invoiceRepository.GetInvoice(paymentDto.InvoiceId);
            var payments = _paymentRepository.GetPaymentsForInvoice(paymentDto.InvoiceId);
            var paymentTotal = payments.Sum(p => p.PaymentTotal);

            _invoiceRepository.SetInvoiceStatus(paymentDto.InvoiceId, paymentTotal >= invoice.InvoiceTotal ? _paidinfullStatus : _somepaidStatus);
        }
    }
}