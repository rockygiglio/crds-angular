
using System;
using System.Collections.Generic;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads.Payment;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.FunctionalHelpers;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class PaymentServiceTest
    {

        private readonly Mock<IInvoiceRepository> _invoiceRepository;
        private readonly Mock<IPaymentRepository> _paymentRepository;
        private readonly Mock<IContactRepository> _contactRepository;
        private readonly Mock<IPaymentTypeRepository> _paymentTypeRepository;
        private readonly Mock<IConfigurationWrapper> _configWrapper;
        private readonly IPaymentService _fixture;

        private readonly int paidInFull = 54;
        private readonly int somePaid = 45;

        public PaymentServiceTest()
        {
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _paymentRepository = new Mock<IPaymentRepository>();
            _contactRepository = new Mock<IContactRepository>();
            _paymentTypeRepository = new Mock<IPaymentTypeRepository>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _configWrapper.Setup(m => m.GetConfigIntValue("PaidInFull")).Returns(paidInFull);
            _configWrapper.Setup(m => m.GetConfigIntValue("SomePaid")).Returns(somePaid);
            _fixture = new PaymentService(_invoiceRepository.Object, _paymentRepository.Object, _configWrapper.Object, _contactRepository.Object, _paymentTypeRepository.Object );            
        }         

        [Test]
        public void shouldGetPaymentDetailDTO()
        {
            const int paymentId = 12345;
            const int invoiceId = 3389753;
            const int contactId = 12323354;
            const string emailAddress = "help_me@usa.com";
            const string token = "NOOOOO";


            var me = fakeMyContact(contactId, emailAddress);
            var invoice = fakeInvoice(invoiceId, contactId, 500.00M);
            var payments = fakePayments(contactId, 24M, paymentId);

            _contactRepository.Setup(m => m.GetMyProfile(token)).Returns(me);
            _invoiceRepository.Setup(m => m.GetInvoice(invoiceId)).Returns(invoice);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(invoiceId)).Returns(payments);

            PaymentDetailDTO ret = _fixture.GetPaymentDetails(paymentId, invoiceId, token);
            Assert.AreEqual(24M, ret.PaymentAmount);
            Assert.AreEqual(emailAddress, ret.RecipientEmail);
            Assert.AreEqual(452M, ret.TotalToPay);

            _contactRepository.VerifyAll();
            _invoiceRepository.VerifyAll();
            _paymentRepository.VerifyAll();
        }

        [Test]
        public void ShouldGetPaymentDetailDtOwithPaymentId0()
        {
            const int paymentId = 0;
            const int invoiceId = 3389753;
            const int contactId = 12323354;
            const string emailAddress = "help_me@usa.com";
            const string token = "NOOOOO";


            var me = fakeMyContact(contactId, emailAddress);
            var invoice = fakeInvoice(invoiceId, contactId, 500.00M);
            var payments = fakePayments(contactId, 24M, paymentId);

            _contactRepository.Setup(m => m.GetMyProfile(token)).Returns(me);
            _invoiceRepository.Setup(m => m.GetInvoice(invoiceId)).Returns(invoice);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(invoiceId)).Returns(payments);

            PaymentDetailDTO ret = _fixture.GetPaymentDetails(paymentId, invoiceId, token);
            Assert.AreEqual(24M, ret.PaymentAmount);
            Assert.AreEqual(emailAddress, ret.RecipientEmail);
            Assert.AreEqual(452M, ret.TotalToPay);

            _contactRepository.VerifyAll();
            _invoiceRepository.VerifyAll();
            _paymentRepository.VerifyAll();
        }

        [Test]
        public void shouldNotHavePaymentDetails()
        {
            const int paymentId = 12345;
            const int invoiceId = 3389753;
            const int contactId = 12323354;
            const string emailAddress = "help_me@usa.com";
            const string token = "NOOOOO";

            var me = fakeMyContact(contactId, emailAddress);
            var invoice = fakeInvoice(invoiceId, contactId, 500.00M);
            var payments = fakePayments(1, 24M, paymentId);

            _contactRepository.Setup(m => m.GetMyProfile(token)).Returns(me);
            _invoiceRepository.Setup(m => m.GetInvoice(invoiceId)).Returns(invoice);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(invoiceId)).Returns(payments);

            Assert.Throws<Exception>(() =>
            {
                _fixture.GetPaymentDetails(paymentId, invoiceId, token);
                _contactRepository.VerifyAll();
                _invoiceRepository.VerifyAll();
                _paymentRepository.VerifyAll();
            });


        }

        [Test]
        public void shouldNotFindInvoice()
        {
            var paymentDto = fakePaymentDto();
            //MpDonationAndDistributionRecord

            _invoiceRepository.Setup(m => m.InvoiceExists(paymentDto.InvoiceId)).Returns(false);            
            Assert.Throws<InvoiceNotFoundException>(() =>
            {
                _fixture.PostPayment(paymentDto);
                _invoiceRepository.VerifyAll();
                _configWrapper.VerifyAll();
            });
        }

        [Test]
        public void shouldNotFindContact()
        {
            var paymentDto = fakePaymentDto();

            _invoiceRepository.Setup(m => m.InvoiceExists(paymentDto.InvoiceId)).Returns(true);
            _contactRepository.Setup(m => m.GetContactById(paymentDto.ContactId)).Returns((MpMyContact) null);
            Assert.Throws<ContactNotFoundException>(() =>
            {
                _fixture.PostPayment(paymentDto);
                _invoiceRepository.VerifyAll();
                _contactRepository.VerifyAll();
                _configWrapper.VerifyAll();
            });
        }

        [Test]
        public void shouldNotFindPaymentType()
        {
            var paymentDto = fakePaymentDto();

            _invoiceRepository.Setup(m => m.InvoiceExists(paymentDto.InvoiceId)).Returns(true);
            _contactRepository.Setup(m => m.GetContactById(paymentDto.ContactId)).Returns(new MpMyContact()
            {
                Contact_ID = paymentDto.ContactId
            });
            _paymentTypeRepository.Setup(m => m.PaymentTypeExists(5)).Returns(false);

            Assert.Throws<PaymentTypeNotFoundException>(() =>
            {
                _fixture.PostPayment(paymentDto);
                _invoiceRepository.VerifyAll();
                _contactRepository.VerifyAll();
                _paymentTypeRepository.VerifyAll();
                _configWrapper.VerifyAll();
            });
        }

        [Test]
        public void shouldFailToMakePayment()
        {
            var paymentDto = fakePaymentDto();
            var invoiceDetail = falkeInvoiceDetail(paymentDto.InvoiceId);

            _invoiceRepository.Setup(m => m.InvoiceExists(paymentDto.InvoiceId)).Returns(true);
            _contactRepository.Setup(m => m.GetContactById(paymentDto.ContactId)).Returns(new MpMyContact()
            {
                Contact_ID = paymentDto.ContactId
            });
            _paymentTypeRepository.Setup(m => m.PaymentTypeExists(5)).Returns(true);
            _invoiceRepository.Setup(m => m.GetInvoiceDetailForInvoice(paymentDto.InvoiceId)).Returns(invoiceDetail);            

        

            _paymentRepository.Setup(m => m.CreatePaymentAndDetail(It.IsAny<MpPaymentDetail>())).Returns(new Result<MpPaymentDetailReturn>(false, "error"));

            Assert.Throws<Exception>(() =>
            {
                _fixture.PostPayment(paymentDto);
                _invoiceRepository.VerifyAll();
                _contactRepository.VerifyAll();
                _paymentTypeRepository.VerifyAll();
                _paymentRepository.VerifyAll();
                _configWrapper.VerifyAll();

            });            
        }

        [Test]
        public void shouldMakePaymentOfSomePaid()
        {
            var paymentDto = fakePaymentDto();
            var invoiceDetail = falkeInvoiceDetail(paymentDto.InvoiceId);

            _invoiceRepository.Setup(m => m.InvoiceExists(paymentDto.InvoiceId)).Returns(true);
            _contactRepository.Setup(m => m.GetContactById(paymentDto.ContactId)).Returns(new MpMyContact()
            {
                Contact_ID = paymentDto.ContactId
            });
            _paymentTypeRepository.Setup(m => m.PaymentTypeExists(5)).Returns(true);
            _invoiceRepository.Setup(m => m.GetInvoiceDetailForInvoice(paymentDto.InvoiceId)).Returns(invoiceDetail);

            var retVal = new MpPaymentDetailReturn()
            {
                InvoiceDetailId = invoiceDetail.InvoiceDetailId,
                PaymentAmount = paymentDto.DonationAmt,
                PaymentDetailId = 398457,
                PaymentId = 2897234
            };

            _paymentRepository.Setup(m => m.CreatePaymentAndDetail(It.IsAny<MpPaymentDetail>())).Returns(new Result<MpPaymentDetailReturn>(true, retVal));

            var invoice = new MpInvoice()
            {
                InvoiceId = paymentDto.InvoiceId,
                InvoiceTotal = 500
            };                        

            var paymentsSoFar = new List<MpPayment>
            {
                new MpPayment()
                {
                    PaymentTotal = 20
                },
                 new MpPayment()
                {
                    PaymentTotal = 200
                }
            };

            _invoiceRepository.Setup(m => m.GetInvoice(invoiceDetail.InvoiceId)).Returns(invoice);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(paymentDto.InvoiceId)).Returns(paymentsSoFar);
            _invoiceRepository.Setup(m => m.SetInvoiceStatus(paymentDto.InvoiceId, somePaid));

            var val = _fixture.PostPayment(paymentDto);
            Assert.AreEqual(retVal, val);
            _invoiceRepository.VerifyAll();
            _contactRepository.VerifyAll();
            _paymentTypeRepository.VerifyAll();
            _paymentRepository.VerifyAll();
            _configWrapper.VerifyAll();
        }

        [Test]
        public void shouldMakePaymentOfFullPaid()
        {
            var paymentDto = fakePaymentDto();
            var invoiceDetail = falkeInvoiceDetail(paymentDto.InvoiceId);

            _invoiceRepository.Setup(m => m.InvoiceExists(paymentDto.InvoiceId)).Returns(true);
            _contactRepository.Setup(m => m.GetContactById(paymentDto.ContactId)).Returns(new MpMyContact()
            {
                Contact_ID = paymentDto.ContactId
            });
            _paymentTypeRepository.Setup(m => m.PaymentTypeExists(5)).Returns(true);
            _invoiceRepository.Setup(m => m.GetInvoiceDetailForInvoice(paymentDto.InvoiceId)).Returns(invoiceDetail);

            var retVal = new MpPaymentDetailReturn()
            {
                InvoiceDetailId = invoiceDetail.InvoiceDetailId,
                PaymentAmount = paymentDto.DonationAmt,
                PaymentDetailId = 398457,
                PaymentId = 2897234
            };

            _paymentRepository.Setup(m => m.CreatePaymentAndDetail(It.IsAny<MpPaymentDetail>())).Returns(new Result<MpPaymentDetailReturn>(true, retVal));

            var invoice = new MpInvoice()
            {
                InvoiceId = paymentDto.InvoiceId,
                InvoiceTotal = 500
            };

            var paymentsSoFar = new List<MpPayment>
            {
                new MpPayment()
                {
                    PaymentTotal = 300
                },
                 new MpPayment()
                {
                    PaymentTotal = 200
                }
            };

            _invoiceRepository.Setup(m => m.GetInvoice(invoiceDetail.InvoiceId)).Returns(invoice);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(paymentDto.InvoiceId)).Returns(paymentsSoFar);
            _invoiceRepository.Setup(m => m.SetInvoiceStatus(paymentDto.InvoiceId, paidInFull));

            var val = _fixture.PostPayment(paymentDto);
            Assert.AreEqual(retVal, val);
            _invoiceRepository.VerifyAll();
            _contactRepository.VerifyAll();
            _paymentTypeRepository.VerifyAll();
            _paymentRepository.VerifyAll();
            _configWrapper.VerifyAll();
        }

        [Test]
        public void GetPaymentByTransactionCodeShoudThrow()
        {
            var stripepaymentid = "qwerty";
            var e = new Exception("bad things");
            _paymentRepository.Setup(w => w.GetPaymentByTransactionCode(stripepaymentid)).Throws(e);

            Assert.Throws<PaymentNotFoundException>(() => _fixture.GetPaymentByTransactionCode(stripepaymentid));
        }

        public void DepositAlreadyExists()
        {
            const string token = "letmein";
            const int payerId = 3333;           
            const int invoiceId = 89989;

            var payments = fakePayments(payerId, 500);
            var me = fakeMyContact(payerId, "faekemail@jon.com");

            _contactRepository.Setup(m => m.GetMyProfile(token)).Returns(me);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(invoiceId)).Returns(payments);            
                
            var result = _fixture.DepositExists(invoiceId, token);
            Assert.IsTrue(result);
        }

        [Test]
        public void DepositDoesNotExist()
        {
            const string token = "letmein";
            const int payerId = 3333;
            const int invoiceId = 89989;

            var payments = fakePayments(1234, 500);
            var me = fakeMyContact(payerId, "faekemail@jon.com");

            _contactRepository.Setup(m => m.GetMyProfile(token)).Returns(me);
            _paymentRepository.Setup(m => m.GetPaymentsForInvoice(invoiceId)).Returns(payments);

            var result = _fixture.DepositExists(invoiceId, token);
            Assert.IsFalse(result);
        }

        private static List<MpPayment> fakePayments(int payerId, decimal paymentTotal, int paymentIdOfOne = 34525)
        {
            return new List<MpPayment>
            {
                new MpPayment()
                {
                    PaymentId = paymentIdOfOne,
                    ContactId = payerId,                  
                    PaymentTotal = paymentTotal
                }, 
                new MpPayment()
                {
                    PaymentTotal = 24.0M,

                }
            };
        }


        private static MpDonationAndDistributionRecord fakePaymentDto(int contactId = 1234, int invoiceId = 8970)
        {
            return new MpDonationAndDistributionRecord()
            {
                DonationAmt = 120.0M,
                ContactId = contactId,
                InvoiceId = invoiceId,
                PymtType = "bank",
                ProcessorId = "some meaningless string"
            };
        }

        private static MpInvoiceDetail falkeInvoiceDetail(int invoiceId)
        {
            return new MpInvoiceDetail()
            {
                InvoiceDetailId = 1346,
                InvoiceId = invoiceId
            };
        }

        private static MpInvoice fakeInvoice(int invoiceId, int contactId, decimal total)
        {
            return new MpInvoice()
            {
                InvoiceId = invoiceId,
                InvoiceTotal = total,
                PurchaserContactId = contactId,
                InvoiceStatusId = 4
            };
        }

        private static MpMyContact fakeMyContact(int contactId, string emailAddress)
        {
            // all I really care about is contactId and emailAddress
            return new MpMyContact()
            {
                Contact_ID = contactId,
                Email_Address = emailAddress
            };
        }

    }
}
