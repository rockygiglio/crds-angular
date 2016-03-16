using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GoVolunteerRegistrationController : MPAuth
    {
        private readonly IGoVolunteerService _goVolunteerService;

        public GoVolunteerRegistrationController(IGoVolunteerService goVolunteerService)
        {
            _goVolunteerService = goVolunteerService;
        }

        [AcceptVerbs("GET")]
        [Route("api/goVolunteerRegistration")]
        [ResponseType(typeof (Registration))]
        public IHttpActionResult Get()
        {
            var registration = new Registration();
            registration.Self = new Person {FirstName = "Lakshmi", LastName = "Nair", EmailAddress = "email@nair.com"};
            registration.Equipment = new List<Equipment>
            {
                new Equipment
                {
                    Id = 1,
                    Notes = "test"
                }
            };
            registration.PrepWork = new List<PrepWork> { new PrepWork { Id = 7042, Spouse = false } };
            registration.ProjectPreferences = new List<ProjectPreference>{ new ProjectPreference {Id=5, Priority = 1}};
            registration.Spouse = new Person { FirstName = "Spouse", LastName = "Nair", EmailAddress = "spouse@nair.com" };
            registration.AdditionalInformation = "Additional Information";
            registration.ChildAgeGroup = new List<ChildrenAttending> { new ChildrenAttending { Count = 3, Id = 7043 } };
            registration.CreateGroupConnector = false;
            registration.GroupConnectorId = 0;
            registration.InitiativeId = 1;
            registration.OrganizationId = 1;
            registration.PreferredLaunchSiteId = 3;
            registration.RoleId = 2;
            registration.SpouseParticipation = true;
            registration.WaiverSigned = true;


            return Ok(registration);
        }

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
                        //throw new NotImplementedException("POST goVolunteerRegistration");
                        _goVolunteerService.CreateRegistration(goVolunteerRegistration, token);
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