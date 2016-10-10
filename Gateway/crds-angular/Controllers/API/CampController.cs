using System;
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
    public class CampController: MPAuth
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
        [Route("api/camps/{eventid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCampEventDetails(int eventId)
        {
            return Authorized(token =>
            {
                try
                {
                    var campEventInfo = _campService.GetCampEventDetails(eventId);
                    return Ok(campEventInfo);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("EventInfo", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [Route("api/camps/{eventid}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveCampReservation([FromBody] CampReservationDTO campReservation, int eventId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Camper Application data Invalid", new InvalidOperationException("Invalid Camper Application Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _campService.SaveCampReservation(campReservation, eventId);
                    return Ok();
                }
               
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Camp Reservation failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
