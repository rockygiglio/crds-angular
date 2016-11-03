using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Opportunity;
using crds_angular.Security;
using Crossroads.Utilities.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class OpportunityController : MPAuth
    {
        private readonly IOpportunityRepository _opportunityService;

        public OpportunityController(IOpportunityRepository opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [ResponseType(typeof (int))]
        [VersionedRoute(template: "opportunity/{opportunityId}", minimumVersion: "1.0.0")]
        [Route("opportunity/{opportunityId}")]
        public IHttpActionResult Post(int opportunityId, [FromBody] string stuff)
        {
            var comments = string.Format("Request on {0}", DateTime.Now.ToString(CultureInfo.CurrentCulture));

            return Authorized(token =>
            {
                try
                {
                    var id = _opportunityService.RespondToOpportunity(token, opportunityId, comments);
                    return Ok(id);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            });
        }

        [ResponseType(typeof (int))]
        [VersionedRoute(template: "opportunity/saveQualifiedServer", minimumVersion: "1.0.0")]
        [Route("opportunity/save-qualified-server")]
        public IHttpActionResult Post([FromBody] MpRespondToOpportunityDto opportunityResponse)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors)
                    .Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("POST Data Invalid",
                    new InvalidOperationException("Invalid POST Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            try
            {
                if (opportunityResponse.Participants.Count > 0)
                {
                    _opportunityService.RespondToOpportunity(opportunityResponse);
                }
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Opportunity POST failed", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
            return Ok();
        }

        [ResponseType(typeof (List<long>))]
        [VersionedRoute(template: "opportunity/getAllOpportunityDates/{opportunityId}", minimumVersion: "1.0.0")]
        [Route("opportunity/getAllOpportunityDates/{opportunityId}")]
        public IHttpActionResult GetAllOpportunityDates(int opportunityId)
        {
            var oppDates = new List<long>();
            return Authorized(token =>
            {
                var opportunities = _opportunityService.GetAllOpportunityDates(opportunityId, token);
                oppDates.AddRange(opportunities.Select(opp => opp.ToUnixTime()));
                return Ok(oppDates);
            });
        }

        [ResponseType(typeof (Dictionary<string, long>))]
        [VersionedRoute(template: "opportunity/getLastOpportunityDate/{opportunityId}", minimumVersion: "1.0.0")]
        [Route("opportunity/getLastOpportunityDate/{opportunityId}")]
        public IHttpActionResult GetLastOpportunityDate(int opportunityId)
        {
            return
                Authorized(
                    token =>
                        Ok(new Dictionary<string, long>
                        {
                            {"date", _opportunityService.GetLastOpportunityDate(opportunityId, token).ToUnixTime()}
                        }));
        }

        [ResponseType(typeof (OpportunityGroup))]
        [VersionedRoute(template: "opportunity/{opportunityId}", minimumVersion: "1.0.0")]
        [Route("opportunity/{opportunityId}")]
        public IHttpActionResult GetGroupParticipantsForOpportunity(int opportunityId)
        {
            return Authorized(token =>
            {
                var group = _opportunityService.GetGroupParticipantsForOpportunity(opportunityId, token);
                var oppGrp = Mapper.Map<MpGroup, OpportunityGroup>(group);
                return Ok(oppGrp);
            });
        }

        [ResponseType(typeof (OpportunityResponseDto))]
        [VersionedRoute(template: "opportunity/getResponseForOpportunity/{opportunityId}/{contactId}", minimumVersion: "1.0.0")]
        [Route("opportunity/getResponseForOpportunity/{opportunityId}/{contactId}")]
        public IHttpActionResult GetResponseForOpportunity(int opportunityId, int contactId)
        {
            try
            {
                var response = _opportunityService.GetOpportunityResponse(contactId, opportunityId);
                var mapped = Mapper.Map<MpResponse, OpportunityResponseDto>((MpResponse)response);
                return Ok(mapped);
            }
            catch (Exception exception)
            {
                var apiError = new ApiErrorDto("Get Response For Opportunity", exception);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}