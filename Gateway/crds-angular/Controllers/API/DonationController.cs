﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Models.Json;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using log4net;
using Microsoft.Ajax.Utilities;
using MinistryPlatform.Translation.Models;
using MPInterfaces = MinistryPlatform.Translation.Repositories.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Security;

namespace crds_angular.Controllers.API
{
    public class DonationController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(DonationController));

        private readonly MPInterfaces.IDonorRepository _mpDonorService;
        private readonly IPaymentProcessorService _stripeService;
        private readonly IAuthenticationRepository _authenticationService;
        private readonly MPInterfaces.IContactRepository _contactRepository; 
        private readonly IDonorService _gatewayDonorService;
        private readonly IDonationService _gatewayDonationService;
        private readonly IUserImpersonationService _impersonationService;
        private readonly MPInterfaces.IDonationRepository _mpDonationService;
        private readonly MPInterfaces.IPledgeRepository _mpPledgeService;
        private readonly IPaymentService _paymentService;
        private readonly MPInterfaces.IInvoiceRepository _invoiceRepository;

        public DonationController(MPInterfaces.IDonorRepository mpDonorService,
                                  IPaymentProcessorService stripeService,
                                  IAuthenticationRepository authenticationService,
                                  MPInterfaces.IContactRepository contactRepository,
                                  IDonorService gatewayDonorService,
                                  IDonationService gatewayDonationService,
                                  MPInterfaces.IDonationRepository mpDonationService,
                                  MPInterfaces.IPledgeRepository mpPledgeService,
                                  IUserImpersonationService impersonationService,
                                  IPaymentService paymentService,
                                  MPInterfaces.IInvoiceRepository invoiceRepository) : base(impersonationService, authenticationService)
        {
            _mpDonorService = mpDonorService;
            _stripeService = stripeService;
            _authenticationService = authenticationService;
            _contactRepository = contactRepository;
            _gatewayDonorService = gatewayDonorService;
            _gatewayDonationService = gatewayDonationService;
            _impersonationService = impersonationService;
            _invoiceRepository = invoiceRepository;
            _mpDonationService = mpDonationService;
            _mpPledgeService = mpPledgeService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// Retrieves a list of "quick" recommended donation amounts for in-line giving
        /// </summary>
        /// <returns>A list of donation amounts (int)</returns>
        [VersionedRoute(template: "donations/predefined-amounts", minimumVersion: "1.0.0")]
        [Route("donations/predefinedamounts")]
        [HttpGet]
        public IHttpActionResult GetPredefinedDonationAmounts()
        {
            List<int> predefinedDonationAmounts =_mpDonationService.GetPredefinedDonationAmounts();
            return Ok(predefinedDonationAmounts);
        }

        /// <summary>
        /// Function serves TWO Routes - api/donations AND api/donations/{donationYear}
        /// Retrieve list of donations for the logged-in donor, optionally for the specified year, and optionally returns only soft credit donations (by default returns only direct gifts), and optionally include recurring gifts.
        /// </summary>
        /// <param name="softCredit">A bool indicating if the result should contain only soft-credit (true), only direct (false), or all (null) donations.  Defaults to null.</param>
        /// <param name="donationYear">A year filter (YYYY format) for donations returned - defaults to null, meaning return all available donations regardless of year.</param>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <param name="limit">A limit of donations to return starting at the most resent - defaults to null, meaning return all available donations with no limit.</param>
        /// <param name="includeRecurring">Include recurring donations</param>
        /// <returns>A list of DonationDTOs</returns>
        [VersionedRoute(template: "donations", minimumVersion: "1.0.0")]
        [Route("donations")]
        [VersionedRoute(template: "donations/{donationYear:regex(\\d{4})?}", minimumVersion: "1.0.0")]
        [Route("donations/{donationYear:regex(\\d{4})?}")]
        [HttpGet]
        [RequiresAuthorization]
        public IHttpActionResult GetDonations(string donationYear = null,
                                              int? limit = null,
                                              [FromUri(Name = "softCredit")] bool? softCredit = null,
                                              [FromUri(Name = "impersonateDonorId")] int? impersonateDonorId = null,
                                              bool? includeRecurring = true)
        {
            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;
                try
                {
                    var donations = (impersonateDonorId != null)
                        ? _impersonationService.WithImpersonation(token,
                                                                  impersonateUserId,
                                                                  () =>
                                                                      _gatewayDonationService.GetDonationsForAuthenticatedUser(token, donationYear, limit, softCredit, includeRecurring))
                        : _gatewayDonationService.GetDonationsForAuthenticatedUser(token, donationYear, limit, softCredit, includeRecurring);
                    if (donations == null || !donations.HasDonations)
                    {
                        return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No matching donations found")));
                    }

                    return (Ok(donations));
                }
                catch (UserImpersonationException e)
                {
                    return (e.GetRestHttpActionResult());
                }
            }));
        }

        /// <summary>
        /// Retrieve a list of donation years for the logged-in donor.  This includes any year the donor has given either directly, or via soft-credit.
        /// </summary>
        /// <param name="impersonateDonorId">An optional donorId of a donor to impersonate</param>
        /// <returns>A list of years (string)</returns>
        [VersionedRoute(template: "donations/years", minimumVersion: "1.0.0")]
        [Route("donations/years")]
        [HttpGet]
        public IHttpActionResult GetDonationYears([FromUri(Name = "impersonateDonorId")] int? impersonateDonorId = null)
        {
            return (Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _mpDonorService.GetEmailViaDonorId(impersonateDonorId.Value).Email;
                try
                {
                    var donationYears = (impersonateDonorId != null)
                        ? _impersonationService.WithImpersonation(token, impersonateUserId, () => _gatewayDonationService.GetDonationYearsForAuthenticatedUser(token))
                        : _gatewayDonationService.GetDonationYearsForAuthenticatedUser(token);

                    if (donationYears == null || !donationYears.HasYears)
                    {
                        return (RestHttpActionResult<ApiErrorDto>.WithStatus(HttpStatusCode.NotFound, new ApiErrorDto("No donation years found")));
                    }

                    return (Ok(donationYears));
                }
                catch (UserImpersonationException e)
                {
                    return (e.GetRestHttpActionResult());
                }
            }));
        }

        [ResponseType(typeof (DonationDTO))]
        [VersionedRoute(template: "donation", minimumVersion: "1.0.0")]
        [Route("donation")]
        public IHttpActionResult Post([FromBody] CreateDonationDTO dto)
        {
            return (Authorized(token =>
                                   CreateDonationAndDistributionAuthenticated(token, dto),
                               () => CreateDonationAndDistributionUnauthenticated(dto)));
        }

        [VersionedRoute(template: "donation/message", minimumVersion: "1.0.0")]
        [Route("donation/message")]
        public IHttpActionResult SendMessageToDonor([FromBody] MessageToDonorDTO dto)
        {
            return (Authorized(token =>
            {
                var contactId = _contactRepository.GetContactId(token);
                _gatewayDonationService.SendMessageToDonor(dto.DonorId, dto.DonationDistributionId, contactId, dto.Message, dto.TripName);
                return Ok();
            }));
        }

        [RequiresAuthorization]
        [VersionedRoute(template: "gp-export/file/{selectionId}/{depositId}", minimumVersion: "1.0.0")]
        [Route("gpexport/file/{selectionId}/{depositId}")]
        [HttpGet]
        public IHttpActionResult GetGPExportFile(int selectionId, int depositId)
        {
            return Authorized(token =>
            {
                try
                {
                    // get export file and name
                    var deposit = _gatewayDonationService.GetDepositById(depositId);
                    var fileName = _gatewayDonationService.GPExportFileName(deposit);
                    var stream = _gatewayDonationService.CreateGPExport(selectionId, depositId, token);

                    return new FileResult(stream, fileName);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("GP Export File Creation Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (List<DepositDTO>))]
        [VersionedRoute(template: "gp-export/filenames/{selectionId}", minimumVersion: "1.0.0")]
        [Route("gpexport/filenames/{selectionId}")]
        [HttpGet]
        public IHttpActionResult GetGPExportFileNames(int selectionId)
        {
            return Authorized(token =>
            {
                try
                {
                    var deposits = _gatewayDonationService.GenerateGPExportFileNames(selectionId, token);
                    return Ok(deposits);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Getting GP Export File Names Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        private IHttpActionResult CreateDonationAndDistributionAuthenticated(String token, CreateDonationDTO dto)
        {
            var isPayment = (dto.TransactionType != null && dto.TransactionType.Equals("PAYMENT"));

            try
            {
                if (isPayment)
                {
                    //check if invoice exists before create Stripe Charge
                    if (dto.InvoiceId != null && !_invoiceRepository.InvoiceExists(dto.InvoiceId.Value))                    
                    {                        
                      var apiError = new ApiErrorDto("Invoice Not Found", new InvoiceNotFoundException(dto.InvoiceId.Value));                      
                      throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                }

                var contactId = _contactRepository.GetContactId(token);
                var donor = _mpDonorService.GetContactDonor(contactId);
                var charge = _stripeService.ChargeCustomer(donor.ProcessorId, dto.Amount, donor.DonorId, isPayment);
                var fee = charge.BalanceTransaction != null ? charge.BalanceTransaction.Fee : null;

                int? pledgeId = null;
                if (dto.PledgeCampaignId != null && dto.PledgeDonorId != null)
                {
                    var pledge = _mpPledgeService.GetPledgeByCampaignAndDonor(dto.PledgeCampaignId.Value, dto.PledgeDonorId.Value);
                    if (pledge != null)
                    {
                        pledgeId = pledge.PledgeId;
                    }
                }

                if (!isPayment)
                {
                    var donationAndDistribution = new MpDonationAndDistributionRecord
                    {
                        DonationAmt = dto.Amount,
                        FeeAmt = fee,
                        DonorId = donor.DonorId,
                        ProgramId = dto.ProgramId,
                        PledgeId = pledgeId,
                        ChargeId = charge.Id,
                        PymtType = dto.PaymentType,
                        ProcessorId = donor.ProcessorId,
                        SetupDate = DateTime.Now,
                        RegisteredDonor = true,
                        Anonymous = dto.Anonymous,
                        SourceUrl = dto.SourceUrl,
                        PredefinedAmount = dto.PredefinedAmount
                    };

                    var donationId = _mpDonorService.CreateDonationAndDistributionRecord(donationAndDistribution, !dto.TripDeposit);
                    if (!dto.GiftMessage.IsNullOrWhiteSpace() && pledgeId != null)
                    {
                        SendMessageFromDonor(pledgeId.Value, donationId, dto.GiftMessage);
                    }
                    var response = new DonationDTO
                    {
                        ProgramId = dto.ProgramId,
                        Amount = (int) dto.Amount,
                        Id = donationId.ToString(),
                        Email = donor.Email
                    };

                    return Ok(response);
                }
                else //Payment flow (non-contribution transaction)
                {
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                        var dataError = new ApiErrorDto("Payment data Invalid", new InvalidOperationException("Invalid Payment Data" + errors));
                        throw new HttpResponseException(dataError.HttpResponseMessage);
                    }

                    try
                    {                        
                        var invoiceId = dto.InvoiceId != null ? dto.InvoiceId.Value : 0;
                        var payment = new MpDonationAndDistributionRecord
                        {
                            DonationAmt = dto.Amount,
                            PymtType = dto.PaymentType,
                            ProcessorId = charge.Id,
                            ContactId = contactId,
                            InvoiceId = invoiceId,                            
                            FeeAmt = fee
                        };
                        var paymentReturn = _paymentService.PostPayment(payment);
                        var response = new DonationDTO
                        {
                            Amount = (int)dto.Amount,
                            Email = donor.Email,
                            PaymentId = paymentReturn.PaymentId
                        };
                        return Ok(response);
                    }
                    catch (InvoiceNotFoundException e)
                    {
                        var apiError = new ApiErrorDto("Invoice Not Found", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                    catch (ContactNotFoundException e)
                    {
                        var apiError = new ApiErrorDto("Contact Not Found", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                    catch (PaymentTypeNotFoundException e)
                    {
                        var apiError = new ApiErrorDto("PaymentType Not Found", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                    catch (Exception e)
                    {
                        var apiError = new ApiErrorDto("SavePayment Failed", e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                }
            }
            catch (PaymentProcessorException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donation/Payment Post Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        private IHttpActionResult CreateDonationAndDistributionUnauthenticated(CreateDonationDTO dto)
        {
            bool isPayment = false;

            try
            {
                var donor = _gatewayDonorService.GetContactDonorForEmail(dto.EmailAddress);
                var charge = _stripeService.ChargeCustomer(donor.ProcessorId, dto.Amount, donor.DonorId, isPayment);
                var fee = charge.BalanceTransaction != null ? charge.BalanceTransaction.Fee : null;
                int? pledgeId = null;
                if (dto.PledgeCampaignId != null && dto.PledgeDonorId != null)
                {
                    var pledge = _mpPledgeService.GetPledgeByCampaignAndDonor(dto.PledgeCampaignId.Value, dto.PledgeDonorId.Value);
                    if (pledge != null)
                    {
                        pledgeId = pledge.PledgeId;
                    }
                }

                var donationAndDistribution = new MpDonationAndDistributionRecord
                {
                    DonationAmt = dto.Amount,
                    FeeAmt = fee,
                    DonorId = donor.DonorId,
                    ProgramId = dto.ProgramId,
                    PledgeId = pledgeId,
                    ChargeId = charge.Id,
                    PymtType = dto.PaymentType,
                    ProcessorId = donor.ProcessorId,
                    SetupDate = DateTime.Now,
                    RegisteredDonor = false,
                    Anonymous = dto.Anonymous,
                    PredefinedAmount = dto.PredefinedAmount,
                    SourceUrl = dto.SourceUrl
                };

                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(donationAndDistribution);
                if (!dto.GiftMessage.IsNullOrWhiteSpace() && pledgeId != null)
                {
                    SendMessageFromDonor(pledgeId.Value, donationId, dto.GiftMessage);
                }

                var response = new DonationDTO()
                {
                    ProgramId = dto.ProgramId,
                    Amount = (int)dto.Amount,
                    Id = donationId.ToString(),
                    Email = donor.Email
                };

                return Ok(response);
            }
            catch (PaymentProcessorException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donation Post Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        private void SendMessageFromDonor(int pledgeId, int donationId, string message)
        {
            try
            {
                _mpDonationService.SendMessageFromDonor(pledgeId, donationId, message);
            }
            catch (Exception ex) {
                _logger.Error(string.Format("Send Message From Donor Failed, pledgeId ({0})", pledgeId),ex);
            }
        }
    }
}
