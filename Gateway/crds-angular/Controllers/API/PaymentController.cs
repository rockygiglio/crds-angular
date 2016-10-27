using System;
using System.Linq;
using System.Web.Http;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Payment;
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


        [VersionedRoute(template: "payment", minimumVersion: "1.0.0")]
        [Route("payment")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SavePayment([FromBody] PaymentDTO payment)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Payment data Invalid", new InvalidOperationException("Invalid Payment Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _paymentService.PostPayment(payment);
                    return Ok();
                }
                // catch some custom exceptions thrown from the payment service
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
            });
        }
    }
}
