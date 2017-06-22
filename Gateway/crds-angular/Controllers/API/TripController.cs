﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;

namespace crds_angular.Controllers.API
{
    public class TripController : MPAuth
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService, IUserImpersonationService userImpersonationService, IAuthenticationRepository authenticationRepository) : base(userImpersonationService, authenticationRepository)
        {
            _tripService = tripService;
        }

        [ResponseType(typeof (List<FamilyMemberTripDto>))]
        [VersionedRoute(template: "trip/{campaignId}/family-members", minimumVersion: "1.0.0")]
        [Route("trip/{campaignId}/family-members")]
        [HttpGet]
        public IHttpActionResult GetFamilyWithTripInfo(int campaignId)
        {
            return Authorized(token =>
            {
                try
                {
                    var familyMembers = _tripService.GetFamilyMembers(campaignId, token);
                    return Ok(familyMembers);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Get Family With Trip Info", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }

            });
        }

        [VersionedRoute(template: "trip/scholarship/{campaignId}/{contactId}", minimumVersion: "1.0.0")]
        [Route("trip/scholarship/{campaignId}/{contactId}")]
        [HttpGet]
        public IHttpActionResult ContactHasScholarship(int contactId, int campaignId)
        {
            return Authorized(token =>
            {
                try
                {
                    var scholarshipped = _tripService.HasScholarship(contactId, campaignId);
                    if (scholarshipped)
                    {
                        return Ok();
                    }
                    return new StatusCodeResult(HttpStatusCode.ExpectationFailed, this);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Check for scholarship", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (TripFormResponseDto))]
        [VersionedRoute(template: "trip/form-responses/{selectionId}/{selectionCount}/{recordId}", minimumVersion: "1.0.0")]
        [Route("trip/form-responses/{selectionId}/{selectionCount}/{recordId}")]
        [HttpGet]
        public IHttpActionResult TripFormResponses(int selectionId, int selectionCount, int recordId)
        {
            return Authorized(token =>
            {
                try
                {
                    var groups = _tripService.GetFormResponses(selectionId, selectionCount, recordId);
                    return Ok(groups);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("GetGroupsForEvent Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (TripCampaignDto))]
        [VersionedRoute(template: "trip/campaign/{campaignId}", minimumVersion: "1.0.0")]
        [Route("trip/campaign/{campaignId}")]
        [HttpGet]
        public IHttpActionResult GetCampaigns(int campaignId)
        {
            return Authorized(token =>
            {
                try
                {
                    var campaign = _tripService.GetTripCampaign(campaignId);
                    return Ok(campaign);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Get Campaign Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [VersionedRoute(template: "trip/generate-private-invite", minimumVersion: "1.0.0")]
        [Route("trip/generate-private-invite")]
        [HttpPost]
        public IHttpActionResult GeneratePrivateInvite([FromBody] PrivateInviteDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("GeneratePrivateInvite Data Invalid", new InvalidOperationException("Invalid GeneratePrivateInvite Data" + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _tripService.GeneratePrivateInvite(dto, token);
                    return Ok();
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("GeneratePrivateInvite Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (TripParticipantDto))]
        [VersionedRoute(template: "trip/search/{query?}", minimumVersion: "1.0.0")]
        [Route("trip/search/{query?}")]
        [HttpGet]
        public IHttpActionResult Search(string query)
        {
            try
            {
                var list = _tripService.Search(query);
                return Ok(list);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Trip Search Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof (TripParticipantDto))]
        [VersionedRoute(template: "trip/participant/{tripParticipantId}", minimumVersion: "1.0.0")]
        [Route("trip/participant/{tripParticipantId}")]
        [HttpGet]
        public IHttpActionResult TripParticipant(string tripParticipantId)
        {
            try
            {
                // Get Participant
                var searchString = tripParticipantId + ',';
                var participant = _tripService.Search(searchString).FirstOrDefault();
                return Ok(participant);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Trip Search Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof (MyTripsDto))]
        [VersionedRoute(template: "trip/my-trips", minimumVersion: "1.0.0")]
        [Route("trip/mytrips")]
        [HttpGet]
        public IHttpActionResult MyTrips()
        {
            return Authorized(token =>
            {
                try
                {
                    var trips = _tripService.GetMyTrips(token);
                    return Ok(trips);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Failed to retrieve My Trips info", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof (ValidatePrivateInviteDto))]
        [VersionedRoute(template: "trip/validate-private-invite/{campaignId}/{invitationKey}", minimumVersion: "1.0.0")]
        [Route("trip/validate-private-invite/{campaignId}/{invitationKey}")]
        [HttpGet]
        public IHttpActionResult ValidatePrivateInvite(int campaignId, string invitationKey)
        {
            return Authorized(token =>
            {
                try
                {
                    var retVal = new ValidatePrivateInviteDto {Valid = _tripService.ValidatePrivateInvite(campaignId, invitationKey, token)};
                    return Ok(retVal);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("ValidatePrivateInvite Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
