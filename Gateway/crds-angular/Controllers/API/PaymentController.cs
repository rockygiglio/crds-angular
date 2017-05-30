﻿using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;

namespace crds_angular.Controllers.API
{
    public class PaymentController : MPAuth
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService, IUserImpersonationService userImpersonationService, IAuthenticationRepository authenticationRepository) : base(userImpersonationService, authenticationRepository)
        {
            _paymentService = paymentService;
        }

        [RequiresAuthorization]
        [VersionedRoute(template: "invoice/{invoiceId}", minimumVersion: "1.0.0")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetInvoicePaymentDetails(int invoiceId)
        {
            try
            {
                var res = _paymentService.GetPaymentDetails(invoiceId);
                return Ok(res);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Unable to get payment details", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [RequiresAuthorization]
        [VersionedRoute(template: "invoice/{invoiceId}/has-payment", minimumVersion: "1.0.0")]
        [HttpGet]
        public IHttpActionResult AlreadyPaidDeposit(int invoiceId)
        {
            return Authorized(token =>
            {
                try
                {
                    if (_paymentService.DepositExists(invoiceId, token))
                    {
                        return new StatusCodeResult(HttpStatusCode.Found, this);
                    }
                    return Ok();

                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Deposit Status", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [RequiresAuthorization]
        [VersionedRoute(template: "invoice/{invoiceId}/payment/{paymentId}", minimumVersion: "1.0.0")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetPaymentDetails(int paymentId, int invoiceId)
        {
            return Authorized((token) =>
            {
                try
                {
                    var res = _paymentService.GetPaymentDetails(paymentId, invoiceId, token);
                    return Ok(res);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Unable to get payment details", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            });
            
        }

        [RequiresAuthorization]
        [VersionedRoute(template: "payment/{paymentId}/confirmation", minimumVersion: "1.0.0")]
        [AcceptVerbs("POST")]
        public IHttpActionResult PaymentConfirmation(int paymentId, int contactId, int invoiceId, int eventId)
        {
            return Authorized((token) =>
            {
                try
                {
                    _paymentService.SendPaymentConfirmation(paymentId, eventId, token);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Unable to send a confirmation email", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }
    }
}
