using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Security;
using MinistryPlatform.Translation.Repositories.Interfaces;
using IEventService = crds_angular.Services.Interfaces.IEventService;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class EventToolController : MPAuth
    {
        private readonly IApiUserRepository _apiUserService;
        private readonly IEventService _eventService;

        public EventToolController(IApiUserRepository apiUserService, IEventService eventService)
        {
            _eventService = eventService;
            _apiUserService = apiUserService;
        }

        [ResponseType(typeof (EventToolDto))]
        [VersionedRoute(template: "eventTool/{eventId}", minimumVersion: "1.0.0")]
        [Route("eventTool/{eventId}")]
        [HttpGet]
        public IHttpActionResult GetEventReservation(int eventId)
        {
            return Authorized(token =>
            {
                try
                {
                    var eventReservation = _eventService.GetEventReservation(eventId);
                    return Ok(eventReservation);
                }
                catch (Exception e)
                {
                    var msg = "EventToolController: GET " + eventId;
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(EventToolDto))]
        [VersionedRoute(template: "eventTool/{eventId}/rooms", minimumVersion: "1.0.0")]
        [Route("eventTool/{eventId:int}/rooms")]
        [HttpGet]
        public IHttpActionResult GetEventRoomDetails(int eventId)
        {
            return Authorized(token =>
            {
                try
                {
                    var eventReservation = _eventService.GetEventRoomDetails(eventId);
                    return Ok(eventReservation);
                }
                catch (Exception e)
                {
                    var msg = "EventToolController: GetEventRoomDetails " + eventId;
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [VersionedRoute(template: "eventTool", minimumVersion: "1.0.0")]
        [Route("eventTool")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] EventToolDto eventReservation)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token =>
                {
                    try
                    {
                        _eventService.CreateEventReservation(eventReservation, token);
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        var msg = "EventToolController: POST " + eventReservation.Title;
                        logger.Error(msg, e);
                        var apiError = new ApiErrorDto(msg, e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                });
            }
            var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
            var dataError = new ApiErrorDto("Event Data Invalid", new InvalidOperationException("Invalid Event Data" + errors));
            throw new HttpResponseException(dataError.HttpResponseMessage);
        }

        [VersionedRoute(template: "eventTool/{eventId}", minimumVersion: "1.0.0")]
        [Route("eventTool/{eventId}")]
        [HttpPut]
        public IHttpActionResult Put([FromBody] EventToolDto eventReservation, int eventId)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token =>
                {
                    try
                    {
                        if (eventId == 0)
                        {
                            throw new ApplicationException("Invalid Event Id");
                        }
                        _eventService.UpdateEventReservation(eventReservation, eventId, token);
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        var msg = "EventToolController: PUT " + eventReservation.Title;
                        logger.Error(msg, e);
                        var apiError = new ApiErrorDto(msg, e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                });
            }
            var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
            var dataError = new ApiErrorDto("Event Data Invalid", new InvalidOperationException("Invalid Event Data" + errors));
            throw new HttpResponseException(dataError.HttpResponseMessage);
        }


        [VersionedRoute(template: "eventTool/{eventId}/rooms", minimumVersion: "1.0.0")]
        [Route("eventTool/{eventId:int}/rooms")]
        [HttpPut]
        public IHttpActionResult UpdateEventRoom([FromBody] EventRoomDto room, int eventId)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token =>
                {
                    try
                    {
                        if (eventId == 0)
                        {
                            throw new ApplicationException("Invalid Event Id");
                        }
                        
                        return Ok(_eventService.UpdateEventRoom(room, eventId, token));
                    }
                    catch (Exception e)
                    {
                        var msg = "EventToolController: UpdateEventRoom " + room.Name;
                        logger.Error(msg, e);
                        var apiError = new ApiErrorDto(msg, e);
                        throw new HttpResponseException(apiError.HttpResponseMessage);
                    }
                });
            }
            var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
            var dataError = new ApiErrorDto("Event Data Invalid", new InvalidOperationException("Invalid Event Data" + errors));
            throw new HttpResponseException(dataError.HttpResponseMessage);
        }
    }
}