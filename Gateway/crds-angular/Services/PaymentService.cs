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
        private readonly IContactRepository _contactRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;

        private readonly int _paidinfullStatus;
        private readonly int _somepaidStatus;

        public PaymentService(IInvoiceRepository invoiceRepository, IPaymentRepository paymentRepository, IConfigurationWrapper configurationWrapper, IContactRepository contactRepository, IPaymentTypeRepository paymentTypeRepository)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            _contactRepository = contactRepository;
            _paymentTypeRepository = paymentTypeRepository;
            
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

            //check if contact exists
            if (_contactRepository.GetContactById(paymentDto.ContactId)==null)
            {
                throw new ContactNotFoundException(paymentDto.ContactId);
            }

            if (paymentDto.StripeTransactionId.Length > 50)
            {
                throw new Exception("Max length of 50 exceeded for transaction code");
            }

            //check if payment type exists
            if (!_paymentTypeRepository.PaymentTypeExists(paymentDto.PaymentTypeId))
            {
                throw new PaymentTypeNotFoundException(paymentDto.PaymentTypeId);
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