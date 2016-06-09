using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models.Childcare;
using Newtonsoft.Json;

namespace crds_angular.Controllers.API
{
    public class ChildcareController : MPAuth
    {
        private readonly IChildcareService _childcareService;
        private readonly IEventService _eventService;

        public ChildcareController(IChildcareService childcareService, IEventService eventService)
        {
            _childcareService = childcareService;
            _eventService = eventService;
        }

        [Route("api/childcare/rsvp")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveRsvp([FromBody] ChildcareRsvpDto saveRsvp)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("SaveRsvp Data Invalid", new InvalidOperationException("Invalid SaveRsvp Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _childcareService.SaveRsvp(saveRsvp, token);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Childcare-SaveRsvp failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(Event))]
        [Route("api/childcare/event/{eventid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult ChildcareEventById(int eventid)
        {
            return Authorized(token =>
            {
                try
                {
                    return Ok(_eventService.GetMyChildcareEvent(eventid, token));
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("ChildcareEventById failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(List<FamilyMember>))]
        [Route("api/childcare/eligible-children")]
        [AcceptVerbs("GET")]
        public IHttpActionResult ChildrenEligibleForChildcare()
        {
            return Authorized(token =>
            {
                try
                {
                    return Ok(_childcareService.MyChildren(token));
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("ChildrenEligibleForChildcare failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [Route("api/childcare/request")]
        [AcceptVerbs("POST")]
        public IHttpActionResult CreateChildcareRequest([FromBody] ChildcareRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Save Request Data Invalid", new InvalidOperationException("Invalid Save request Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _childcareService.CreateChildcareRequest(request, token);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Create Childcare Request Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                
            });
        }

        [Route("api/childcare/request/approve/{requestId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult ApproveChildcareRequest(int requestId, ChildcareRequestDto childcareRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Save Request Data Invalid", new InvalidOperationException("Invalid Save request Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _childcareService.ApproveChildcareRequest(requestId, token, childcareRequest);
                    return Ok();
                }
                catch (EventMissingException e)
                {
                    var errors = new DateError() {Errors = e.MissingDateTimes, Message = e.Message};
                    var json = JsonConvert.SerializeObject(errors, Formatting.Indented);
                    var message = new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);
                    message.Content = new StringContent(json);
                    throw new HttpResponseException(message);
                }
                catch (ChildcareDatesMissingException e)
                {
                    var json = JsonConvert.SerializeObject(e.Message, Formatting.None);
                    var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    message.Content = new StringContent(json);
                    throw new HttpResponseException(message);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Approve Childcare Request Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }

        [Route("api/childcare/request/reject/{requestId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult RejectChildcareRequest(int requestId, ChildcareRequest childcareRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Save Request Data Invalid", new InvalidOperationException("Invalid Save request Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _childcareService.RejectChildcareRequest(requestId, token);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Reject Childcare Request Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }

        [Route("api/childcare/getrequest/{requestid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetChildcareRequest(int requestid)
        {

            return Authorized(token =>
            {
                try
                {
                    return Ok(_childcareService.GetChildcareRequestForReview(requestid, token));
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Create Childcare Request Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }

        private class DateError
        {
            public List<DateTime> Errors { get; set;}
            public string Message { get; set;}
        }
    }
}