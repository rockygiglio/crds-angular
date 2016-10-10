﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Controllers.API
{
    class CampController: MPAuth
    {
        private readonly ICampService _campService;
        private readonly IEventService _eventService;
        private readonly IPersonService _personService;
        public CampController(ICampService campService, IEventService eventService, IPersonService personService)
        {
            _campService = campService;
            _eventService = eventService;
            _personService = personService;
        }

        [ResponseType(typeof (CampDTO))]
        [Route("api/camps/{contactid}/{eventid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCampEventDetails(int contactId, int eventId)
        {
            return Authorized(token =>
            {
                try
                {
                    var campEventInfo = _campService.GetCampEventDetails(contactId, eventId);
                    return Ok(campEventInfo);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("EventInfo", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
