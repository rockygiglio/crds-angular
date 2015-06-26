﻿using System;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class DonationController : MPAuth
    {
        private readonly MPInterfaces.IDonorService _mpDonorService;
        private readonly IPaymentService _stripeService;
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly IDonorService _gatewayDonorService;

        public DonationController(MPInterfaces.IDonorService mpDonorService, IPaymentService stripeService,
            MPInterfaces.IAuthenticationService authenticationService, IDonorService gatewayDonorService)
        {
            _mpDonorService = mpDonorService;
            _stripeService = stripeService;
            _authenticationService = authenticationService;
            _gatewayDonorService = gatewayDonorService;
        }

        [ResponseType(typeof(DonationDTO))]
        [Route("api/donation")]
        public IHttpActionResult Post([FromBody] CreateDonationDTO dto)
        {
            return (Authorized(token => CreateDonationAndDistributionAuthenticated(token, dto), () => CreateDonationAndDistributionUnauthenticated(dto)));
        }

        private IHttpActionResult CreateDonationAndDistributionAuthenticated(String token, CreateDonationDTO dto)
        {
            try{
                var contactId = _authenticationService.GetContactId(token);
                var donor = _mpDonorService.GetContactDonor(contactId);
                var chargeId = _stripeService.ChargeCustomer(donor.ProcessorId, dto.Amount, donor.DonorId, dto.PaymentType);
                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(dto.Amount, donor.DonorId, dto.ProgramId, chargeId, dto.PaymentType, donor.ProcessorId, DateTime.Now, true);
                var response = new DonationDTO()
                    {
                        program_id = dto.ProgramId,
                        amount = dto.Amount,
                        donation_id = donationId.ToString(),
                        email = dto.EmailAddress
                    };

                    return Ok(response);
                }
                catch (StripeException stripeException)
                {
                    return (stripeException.GetStripeResult());
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Donation Post Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }       
        }

        private IHttpActionResult CreateDonationAndDistributionUnauthenticated(CreateDonationDTO dto)
        {
            try
            {
                var donor = _gatewayDonorService.GetContactDonorForEmail(dto.EmailAddress);
                var chargeId = _stripeService.ChargeCustomer(donor.ProcessorId, dto.Amount, donor.DonorId, dto.PaymentType);
                var donationId = _mpDonorService.CreateDonationAndDistributionRecord(dto.Amount, donor.DonorId, dto.ProgramId, chargeId, dto.PaymentType, donor.ProcessorId, DateTime.Now, false);

                var response = new DonationDTO()
                {
                    program_id = dto.ProgramId,
                    amount = dto.Amount,
                    donation_id = donationId.ToString(),
                    email = dto.EmailAddress
                };

                return Ok(response);
            }
            catch (StripeException stripeException)
            {
                return (stripeException.GetStripeResult());
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Donation Post Failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }       
        }
        
    }
    
}
