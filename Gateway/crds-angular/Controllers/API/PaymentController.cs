using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class PaymentController : MPAuth
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

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

        //[VersionedRoute(template: "payment", minimumVersion: "1.0.0")]
        //[Route("payment")]
        //[HttpPost]
        //public IHttpActionResult SavePayment([FromBody] PaymentDTO payment)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
        //        var dataError = new ApiErrorDto("Payment data Invalid", new InvalidOperationException("Invalid Payment Data" + errors));
        //        throw new HttpResponseException(dataError.HttpResponseMessage);
        //    }

        //    return Authorized(token =>
        //    {
        //        try
        //        {
        //            var paymentRet = _paymentService.PostPayment(payment);
        //            return Ok(paymentRet);
        //        }
        //        // catch some custom exceptions thrown from the payment service
        //        catch (InvoiceNotFoundException e)
        //        {
        //            var apiError = new ApiErrorDto("Invoice Not Found", e);
        //            throw new HttpResponseException(apiError.HttpResponseMessage);
        //        }
        //        catch (ContactNotFoundException e)
        //        {
        //            var apiError = new ApiErrorDto("Contact Not Found", e);
        //            throw new HttpResponseException(apiError.HttpResponseMessage);
        //        }
        //        catch (PaymentTypeNotFoundException e)
        //        {
        //            var apiError = new ApiErrorDto("PaymentType Not Found", e);
        //            throw new HttpResponseException(apiError.HttpResponseMessage);
        //        }
        //        catch (Exception e)
        //        {
        //            var apiError = new ApiErrorDto("SavePayment Failed", e);
        //            throw new HttpResponseException(apiError.HttpResponseMessage);
        //        }
        //    });
        //}
    }
}
