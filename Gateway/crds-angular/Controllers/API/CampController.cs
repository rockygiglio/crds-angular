using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Camp;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class CampController: MPAuth
    {
        private readonly ICampService _campService;
        public CampController(ICampService campService)
        {
            _campService = campService;
        }

        [VersionedRoute(template: "camps/{eventId}/family", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}/family")]
        [HttpGet]
        public IHttpActionResult GetCampFamily(int eventId)
        {
            return Authorized(token =>
            {
                try
                {
                    var members = _campService.GetEligibleFamilyMembers(eventId, token);
                    return Ok(members);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Camp Family", e
                        );
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(List<MyCampDTO>))]
        [VersionedRoute(template: "myCamp", minimumVersion: "1.0.0")]
        [Route("my-camp")]
        [HttpGet]
        public IHttpActionResult GetMyCampsInfo()
        {
            return Authorized(token =>
            {
                try
                {
                    var myCampsInfo = _campService.GetMyCampInfo(token);
                    return Ok(myCampsInfo);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("My Camp Info", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (CampDTO))]
        [VersionedRoute(template: "camps/{eventId}", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}")]
        [HttpGet]
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

        [ResponseType(typeof(CampReservationDTO))]
        [VersionedRoute(template: "camps/{eventId}/{contactId}", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}/{contactId}")]
        [HttpGet]
        public IHttpActionResult GetCamperInfo(int eventId, int contactId)
        {
            return Authorized(token =>
            {
                try
                {
                    var camperInfo = _campService.GetCamperInfo(token, eventId, contactId);
                    return Ok(camperInfo);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("CamperInfo", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [VersionedRoute(template: "camps/{eventId}", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}")]
        [HttpPost]
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
                    _campService.SaveCampReservation(campReservation, eventId, token);
                    return Ok();
                }
               
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Camp Reservation failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [VersionedRoute(template: "camps/{eventId}/waivers{contactId}", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}/waivers{contactId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCampWaivers(int eventId, int contactId)
        {
            return Authorized(token =>
            {
                try
                {
                    var waivers = _campService.GetCampWaivers(eventId, contactId);
                    return Ok(waivers);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Failed to get waiver data", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [VersionedRoute(template: "camps/medical/{contactId}", minimumVersion: "1.0.0")]
        [Route("camps/medical/{contactId}")]
        public IHttpActionResult SaveMedicalInformation([FromBody] MedicalInfoDTO medicalInfo, int contactId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Camper Medical Info Invalid", new InvalidOperationException("Invalid Camper Medical Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _campService.SaveCamperMedicalInfo(medicalInfo, contactId, token);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Camp Medical Info failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [VersionedRoute(template: "camps/{eventId}/waivers/{contactId}", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}/waivers/{contactId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveWaivers([FromBody] List<CampWaiverResponseDTO> waivers, int eventId, int contactId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Waiver data Invalid", new InvalidOperationException("Invalid Waiver Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _campService.SaveWaivers(token, eventId, contactId, waivers);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Failed to save waiver data", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

                
            });
        }

        [VersionedRoute(template:"camps/{eventId}/emergencycontact/{contactId}", minimumVersion: "1.0.0")]
        [Route("camps/{eventId}/emergencycontact/{contactId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveCamperEmergencyContact([FromBody] CampEmergencyContactDTO emergencyContact, int eventId, int contactId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("Camper Emergency Contact data Invalid", new InvalidOperationException("Invalid Camper emergency contact Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _campService.SaveCamperEmergencyContactInfo(emergencyContact, eventId, contactId, token);
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
