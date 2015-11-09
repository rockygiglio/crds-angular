﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Security;
using IEventService = crds_angular.Services.Interfaces.IEventService;

namespace crds_angular.Controllers.API
{
    public class EventController : MPAuth
    {
        private IMinistryPlatformService _ministryPlatformService;        
        private readonly IApiUserService _apiUserService;
        private readonly IEventService _eventService;

        public EventController(IMinistryPlatformService ministryPlatformService, IApiUserService apiUserService, IEventService eventService)
        {
            this._ministryPlatformService = ministryPlatformService;
            _eventService = eventService;
            _apiUserService = apiUserService;
        }

        [ResponseType(typeof(List<Event>))]
        [Route("api/events/{site}")]
        public IHttpActionResult Get(string site)
        {
            var token = _apiUserService.GetToken();

            var todaysEvents = _ministryPlatformService.GetRecordsDict("TodaysEventLocationRecords", token, site, "5 asc");//Why 5 you ask... Think Ministry

            var events = ConvertToEvents(todaysEvents);

            return this.Ok(events);
        }

        [ResponseType(typeof (Event))]
        [Route("api/event/{eventid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult EventById(int eventId)
        {
            return Authorized(token => {
                                           try
                                           {
                                               return Ok(_eventService.getEvent(eventId));
                                           }
                                           catch (Exception e)
                                           {
                                               var apiError = new ApiErrorDto("Get Event by Id failed", e);
                                               throw new HttpResponseException(apiError.HttpResponseMessage);   
                                           }
                
            });
        }

        [ResponseType(typeof(Event))]
        [Route("api/event/{eventid}/childcare")]
        [AcceptVerbs("GET")]
        public IHttpActionResult ChildcareEventById(int parentEventId)
        {
            return Authorized(token =>
            {
                try
                {
                    return Ok(_eventService.GetChildcareEvent(parentEventId));
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Get Event by Id failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }

        private List<Event> ConvertToEvents(List<Dictionary<string, object>> todaysEvents)
        {
            //init our return variable
            var events = new List<Event>();

            //iterate over the events
            foreach (var thisEvent in todaysEvents)
            {
                string startDateKey = "Event_Start_Date";
                string nameKey = "Event_Title";
                string locationNameKey = "Room_Name";
                string locationNumberKey = "Room_Number";
                string time = "";
                string meridian = "";
                string name = "";
                string location = "";
                if (thisEvent.ContainsKey(startDateKey) && thisEvent[startDateKey] != null)
                {
                    DateTime startDate = (DateTime)thisEvent[startDateKey];
                    time = startDate.ToString("h:mm");
                    meridian = startDate.ToString("tt");
                }
                if (thisEvent.ContainsKey(nameKey) && thisEvent[nameKey] != null)
                {
                    name = thisEvent[nameKey].ToString();
                }
                if (thisEvent.ContainsKey(locationNameKey) && thisEvent[locationNameKey] != null)
                {
                    location = thisEvent[locationNameKey].ToString();
                }
                if (thisEvent.ContainsKey(locationNumberKey) && thisEvent[locationNumberKey] != null)
                {
                    location += " "+thisEvent[locationNumberKey].ToString();
                }
                var e = new Event
                {
                    time = time,
                    meridian = meridian,
                    name = name,
                    location = location
                };
                events.Add(e);
            }
            return events;
        }
    }
}