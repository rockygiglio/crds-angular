using System;
using System.Linq;
using System.Web.Http;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Security;

namespace crds_angular.Controllers.API
{
    public class GoVolunteerRegistrationController : MPAuth
    {
        [AcceptVerbs("POST")]
        [Route("api/goVolunteerRegistration")]
        public IHttpActionResult Post([FromBody] Registration goVolunteerRegistration)
        {
            if (ModelState.IsValid)
            {
                return Authorized(token =>
                {
                    try
                    {
                        throw new NotImplementedException("POST goVolunteerRegistration");
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        var msg = "GoVolunteerRegistrationController: POST " + goVolunteerRegistration;
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