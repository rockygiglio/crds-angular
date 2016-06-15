using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Controllers.API
{
    public class LookupController : MPAuth
    {
        private IConfigurationWrapper _configurationWrapper;
        private readonly LookupService _lookupService;

        public LookupController(IConfigurationWrapper configurationWrapper, LookupService lookupService)
        {
            _configurationWrapper = configurationWrapper;
            _lookupService = lookupService;
        }


        /// <summary>
        /// Get lookup values for table passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof (List<Dictionary<string, object>>))]
        [Route("api/lookup/{table?}")]
        [HttpGet]
        public IHttpActionResult Lookup(string table)
        {
            return Authorized(t =>
            {
                var ret = new List<Dictionary<string, object>>();
                switch (table)
                {
                    case "genders":
                        ret = _lookupService.Genders(t);
                        break;
                    case "maritalstatus":
                        ret = _lookupService.MaritalStatus(t);
                        break;
                    case "serviceproviders":
                        ret = _lookupService.ServiceProviders(t);
                        break;
                    case "countries":
                        ret = _lookupService.Countries(t);
                        break;
                    case "states":
                        ret = _lookupService.States(t);
                        break;
                    case "crossroadslocations":
                        ret = _lookupService.CrossroadsLocations(t);
                        break;
                    case "workteams":
                        ret = _lookupService.WorkTeams(t);
                        break;
                    case "eventtypes":
                        ret = _lookupService.EventTypes(t);
                        break;
                    case "reminderdays":
                        ret = _lookupService.ReminderDays(t);
                        break;
                    case "meetingdays":
                        ret = _lookupService.MeetingDays(t);
                        break;
                    case "ministries":
                        ret = _lookupService.Ministries(t);
                        break;
                    case "childcarelocations":
                        ret = _lookupService.ChildcareLocations(t);
                        break;
                    default:
                        break;
                }
                if (ret.Count == 0)
                {
                    return this.BadRequest(string.Format("table: {0}", table));
                }
                return Ok(ret);
            });
        }

        /// <summary>
        /// Get lookup values for table passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [Route("api/lookup/group/{congregationid}/{ministryid}")]
        [HttpGet]
        public IHttpActionResult FindGroups(string congregationid, string ministryid)
        {
            return Authorized(t =>
            {
                var ret = new List<Dictionary<string, object>>();
                ret = _lookupService.GroupsByCongregationAndMinistry(t, congregationid, ministryid);

                if (ret.Count == 0)
                {
                    return this.BadRequest(string.Format("congregationid: {0} ministryid: {1}", congregationid, ministryid));
                }
                return Ok(ret);
            });
        }

        /// <summary>
        /// Get lookup values for table passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [Route("api/lookup/childcaretimes/{congregationid}")]
        [HttpGet]
        public IHttpActionResult FindChildcareTimes(string congregationid)
        {
            return Authorized(t =>
            {
                var ret = _lookupService.ChildcareTimesByCongregation(t, congregationid);
                return Ok(ret);
            });
        }

        [ResponseType(typeof (Dictionary<string, object>))]
        [HttpGet]
        [Route("api/lookup/{userId}/find/{email?}")]
        public IHttpActionResult EmailExists(int userId, string email)
        {
            //TODO let's clean this up
            var authorizedWithCookie = Authorized(t =>
            {
                var exists = _lookupService.EmailSearch(email, t);
                if (exists.Count == 0 || Convert.ToInt32(exists["dp_RecordID"]) == userId)
                {
                    return Ok();
                }
                return BadRequest();
            });

            if (authorizedWithCookie is UnauthorizedResult)
            {
                var apiUser = _configurationWrapper.GetEnvironmentVarAsString("API_USER");
                var apiPassword = _configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");

                var authData = AuthenticationRepository.authenticate(apiUser, apiPassword);
                var token = authData["token"].ToString();
                var exists = _lookupService.EmailSearch(email, token.ToString());
                if (exists.Count == 0)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return authorizedWithCookie;
        }

        protected static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() != typeof (DynamicJsonArray))
            {
                return null;
            }
            dynamic[] array = obj;
            return array;
        }
    }
}