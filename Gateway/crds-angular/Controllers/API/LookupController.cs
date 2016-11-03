using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Results;
using crds_angular.Models.Json;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Repositories;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class LookupController : MPAuth
    {
        private IConfigurationWrapper _configurationWrapper;
        private readonly LookupRepository _lookupRepository;

        public LookupController(IConfigurationWrapper configurationWrapper, LookupRepository lookupRepository)
        {
            _configurationWrapper = configurationWrapper;
            _lookupRepository = lookupRepository;
        }


        /// <summary>
        /// Get lookup values for table passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [VersionedRoute(template: "lookup/{table?}", minimumVersion: "1.0.0")]
        [Route("lookup/{table?}")]
        [HttpGet]
        public IHttpActionResult Lookup(string table)
        {
            return Authorized(t =>
            {
                return LookupValues(table, t);
            },
            () =>
            {
                return LookupValues(table, "");
            });
        }

        private IHttpActionResult LookupValues(string table, string token)
        {
            var ret = new List<Dictionary<string, object>>();
            switch (table)
            {
                case "genders":
                    ret = _lookupRepository.Genders(token);
                    break;
                case "maritalstatus":
                    ret = _lookupRepository.MaritalStatus(token);
                    break;
                case "serviceproviders":
                    ret = _lookupRepository.ServiceProviders(token);
                    break;
                case "countries":
                    ret = _lookupRepository.Countries(token);
                    break;
                case "states":
                    ret = _lookupRepository.States(token);
                    break;
                case "crossroadslocations":
                    // This returns Crossroads sites and NOT locations!
                    ret = _lookupRepository.CrossroadsLocations(token);
                    break;
                case "workteams":
                    ret = _lookupRepository.WorkTeams(token);
                    break;
                case "eventtypes":
                    ret = _lookupRepository.EventTypes(token);
                    break;
                case "reminderdays":
                    ret = _lookupRepository.ReminderDays(token);
                    break;
                case "meetingdays":
                    ret = _lookupRepository.MeetingDays(token);
                    break;
                case "ministries":
                    ret = _lookupRepository.Ministries(token);
                    break;
                case "childcarelocations":
                    ret = _lookupRepository.ChildcareLocations(token);
                    break;
                case "groupreasonended":
                    ret = _lookupRepository.GroupReasonEnded(token);
                    break;
                default:
                    break;
            }
            if (ret.Count == 0)
            {
                return this.BadRequest(string.Format("table: {0}", table));
            }
            return Ok(ret);

        }

        /// <summary>
        /// Get lookup values for genders
        /// </summary>
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [VersionedRoute(template: "lookup/genders", minimumVersion: "1.0.0")]
        [Route("lookup/genders")]
        [HttpGet]
        public IHttpActionResult LookupGenders()
        {
            return Lookup("genders");
        }

        /// <summary>
        /// Get lookup values for group ended reasons
        /// </summary>
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [VersionedRoute(template: "lookup/groupReasonEnded", minimumVersion: "1.0.0")]
        [Route("lookup/groupreasonended")]
        [HttpGet]
        public IHttpActionResult LookupGroupReasonEnded()
        {        
            return LookupValues("groupreasonended","");
        }

        /// <summary>
        /// Get lookup values for crossroads sites
        /// </summary>
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [VersionedRoute(template: "lookup/sites", minimumVersion: "1.0.0")]
        [Route("lookup/sites")]
        [HttpGet]
        public IHttpActionResult LookupSites()
        {
            return Lookup("crossroadslocations");
        }

        /// <summary>
        /// Get lookup values for table passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [VersionedRoute(template: "lookup/group/{congregationId}/{ministryId}", minimumVersion: "1.0.0")]
        [Route("lookup/group/{congregationid}/{ministryid}")]
        [HttpGet]
        public IHttpActionResult FindGroups(string congregationId, string ministryId)
        {
            return Authorized(t =>
            {
                var ret = new List<Dictionary<string, object>>();
                ret = _lookupRepository.GroupsByCongregationAndMinistry(t, congregationId, ministryId);

                if (ret.Count == 0)
                {
                    return this.BadRequest(string.Format("congregationId: {0} ministryId: {1}", congregationId, ministryId));
                }
                return Ok(ret);
            });
        }

        /// <summary>
        /// Get lookup values for table passed in
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(List<Dictionary<string, object>>))]
        [VersionedRoute(template: "lookup/childcareTimes/{congregationId}", minimumVersion: "1.0.0")]
        [Route("lookup/childcaretimes/{congregationId}")]
        [HttpGet]
        public IHttpActionResult FindChildcareTimes(string congregationId)
        {
            return Authorized(t =>
            {
                var ret = _lookupRepository.ChildcareTimesByCongregation(t, congregationId);
                return Ok(ret);
            });
        }

        [ResponseType(typeof(Dictionary<string, object>))]
        [VersionedRoute(template: "lookup/{userId}/find/{email?}", minimumVersion: "1.0.0")]
        [Route("lookup/{userId}/find/{email?}")]
        [HttpGet]
        public IHttpActionResult EmailExists(int userId, string email)
        {
            //TODO let's clean this up
            var authorizedWithCookie = Authorized(t =>
            {
                var exists = _lookupRepository.EmailSearch(email, t);
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
                var exists = _lookupRepository.EmailSearch(email, token.ToString());
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
            if (obj.GetType() != typeof(DynamicJsonArray))
            {
                return null;
            }
            dynamic[] array = obj;
            return array;
        }
    }
}