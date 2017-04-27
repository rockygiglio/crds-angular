using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Finder;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;
using System.ComponentModel.DataAnnotations;
using crds_angular.Exceptions;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Models.Crossroads.Groups;
using log4net;

namespace crds_angular.Controllers.API
{
    public class FinderController : MPAuth
    {
        private readonly IAwsCloudsearchService _awsCloudsearchService;
        private readonly IAddressService _addressService;
        private readonly IFinderService _finderService;
        private readonly IAuthenticationRepository _authenticationRepo;
        private readonly IAddressGeocodingService _addressGeocodingService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FinderController(IAddressService addressService,
                                IAddressGeocodingService addressGeocodingService, 
                                IFinderService finderService,
                                IUserImpersonationService userImpersonationService,
                                IAuthenticationRepository authenticationRepository,
                                IAwsCloudsearchService awsCloudsearchService)
            : base(userImpersonationService, authenticationRepository)
        {
            _addressService = addressService;
            _finderService = finderService;
            _addressGeocodingService = addressGeocodingService;
            _awsCloudsearchService = awsCloudsearchService;
            _authenticationRepo = authenticationRepository;
        }

        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/pin/{participantId}", minimumVersion: "1.0.0")]
        [Route("finder/pin/{participantId}")]
        [HttpGet]
        public IHttpActionResult GetPinDetails([FromUri]int participantId)
        {
            try
            {
                var pin = _finderService.GetPinDetailsForPerson(participantId);
                pin.Address = _finderService.RandomizeLatLong(pin.Address);
                return Ok(pin);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Pin Details Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/pinByGroupID/{groupId}", minimumVersion: "1.0.0")]
        [Route("finder/pinByGroupID/{groupId}")]
        [HttpGet]
        public IHttpActionResult GetPinDetailsByGroup([FromUri]int groupId)
        {
            try
            {
                var list = _finderService.GetPinDetailsForGroup(groupId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Pin Details Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof(GroupParticipantDTO[]))]
        [VersionedRoute(template: "finder/participants/{groupId}", minimumVersion: "1.0.0")]
        [Route("finder/participants/{groupId}")]
        [HttpGet]
        public IHttpActionResult GetParticipantsForGroup([FromUri]int groupId)
        {
            try
            {
                var list = _finderService.GetParticipantsForGroup(groupId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Group Participants Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/pin/contact/{contactId}/{throwOnEmptyCoordinates?}", minimumVersion: "1.0.0")]
        [Route("finder/pin/contact/{contactId}/{throwOnEmptyCoordinates?}")]
        [HttpGet]
        public IHttpActionResult GetPinDetailsByContact([FromUri]int contactId, [FromUri]bool throwOnEmptyCoordinates = true)
        {
            try
            {
                var participantId = _finderService.GetParticipantIdFromContact(contactId);
                //refactor this to JUST get location;
                var pin = _finderService.GetPinDetailsForPerson(participantId);
                bool pinHasInvalidGeoCoords = ( (pin.Address == null) || (pin.Address.Latitude == null || pin.Address.Longitude == null)
                                               || (pin.Address.Latitude == 0 && pin.Address.Longitude == 0));

                if (pinHasInvalidGeoCoords && throwOnEmptyCoordinates)
                {
                   return Content(HttpStatusCode.ExpectationFailed, "Invalid Latitude/Longitude");
                }
                pin.Address = _finderService.RandomizeLatLong(pin.Address);
                return Ok(pin);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Pin Details by Contact Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof(AddressDTO))]
        [VersionedRoute(template: "finder/pinbyip/{ipAddress}", minimumVersion: "1.0.0")]
        [Route("finder/pinbyip/{ipAddress}")]
        [HttpGet]
        public IHttpActionResult GetPinByIpAddress([FromUri]string ipAddress)
        {
            try
            {
                var address = _finderService.GetAddressForIp(ipAddress.Replace('$','.'));
                return Ok(address);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Pin By Ip Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }


        [RequiresAuthorization]
        [ResponseType(typeof(AddressDTO))]
        [VersionedRoute(template: "finder/group/address/{groupId}", minimumVersion: "1.0.0")]
        [Route("finder/group/address/{groupId}")]
        [HttpGet]
        public IHttpActionResult GetGroupAddress([FromUri] int groupId)
        {
            return Authorized(token =>
            {
                try
                {
                    var address = _finderService.GetGroupAddress(token, groupId);
                    return (Ok(address));
                }
                catch (Exception e)
                {
                    _logger.Error("Could not get address", e);
                    var apiError = new ApiErrorDto("Get Address Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [RequiresAuthorization]
        [ResponseType(typeof(AddressDTO))]
        [VersionedRoute(template: "finder/person/address/{participantId}", minimumVersion: "1.0.0")]
        [Route("finder/person/address/{participantId}")]
        [HttpGet]
        public IHttpActionResult GetPersonAddress([FromUri] int participantId)
        {
            return Authorized(token =>
            {
                try
                {
                    var address = _finderService.GetPersonAddress(token, participantId);
                    return (Ok(address));
                }
                catch (Exception e)
                {
                    _logger.Error("Could not get address", e);
                    var apiError = new ApiErrorDto("Get Address Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Create Pin with provided address details
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/pin", minimumVersion: "1.0.0")]
        [Route("finder/pin")]
        [HttpPost]
        public IHttpActionResult PostPin([FromBody] PinDto pin)
        {
            return Authorized(token =>
            {
                try
                {

                    if (pin.Address != null && string.IsNullOrEmpty(pin.Address.AddressLine1) == false)
                    {
                        _finderService.UpdateHouseholdAddress(pin);
                    }

                    if (pin.Participant_ID == 0 || String.IsNullOrEmpty(pin.Participant_ID.ToString()))
                    {
                        pin.Participant_ID =_finderService.GetParticipantIdFromContact((int)pin.Contact_ID);
                    }

                    _finderService.EnablePin((int)pin.Participant_ID);
                    _logger.DebugFormat("Successfully created pin for contact {0} ", pin.Contact_ID);

                    //Ensure that address id is available
                    var personPin = _finderService.GetPinDetailsForPerson((int)pin.Participant_ID);

                    _awsCloudsearchService.UploadNewPinToAws(personPin); 

                    return (Ok(pin));
                }
                catch (Exception e)
                {
                    _logger.Error("Could not create pin", e);
                    var apiError = new ApiErrorDto("Save Pin Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Remove pin from map
        /// </summary>
        [RequiresAuthorization]
        [VersionedRoute(template: "finder/pin/removeFromMap", minimumVersion: "1.0.0")]
        [Route("finder/pin/removeFromMap")]
        [HttpPost]
        public IHttpActionResult RemovePinFromMap([FromBody] int participantId)
        {
            return Authorized(token =>
            {
                try
                {
                    _finderService.DisablePin(participantId);
                    return Ok();

                }
                catch (Exception e)
                {
                    _logger.Error("Could not create pin", e);
                    var apiError = new ApiErrorDto("Remove pin from map failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Create Pin with provided address details
        /// </summary>
        [RequiresAuthorization]
        [ResponseType(typeof(PinDto))]
        [VersionedRoute(template: "finder/gathering/edit", minimumVersion: "1.0.0")]
        [Route("finder/gathering/edit")]
        [HttpPut]
        public IHttpActionResult EditGatheringPin([FromBody] PinDto pin)
        {
            return Authorized(token =>
            {
                try
                {
                    if (pin.Contact_ID != _authenticationRepo.GetContactId(token))
                    {
                        throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    }

                    pin = _finderService.UpdateGathering(pin);
                    _awsCloudsearchService.UploadNewPinToAws(pin);

                    return (Ok(pin));
                }
                catch (Exception e)
                {
                    _logger.Error("Could not update pin", e);
                    var apiError = new ApiErrorDto("Save Pin Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(PinSearchResultsDto))]
        [VersionedRoute(template: "finder/findpinsbyaddress/{userSearchAddress}/{lat?}/{lng?}/{upperleftlat?}/{upperleftlng?}/{bottomrightlat?}/{bottomrightlng?}", minimumVersion: "1.0.0")]
        [Route("finder/findpinsbyaddress/{userSearchAddress}/{lat?}/{lng?}/{upperleftlat?}/{upperleftlng?}/{bottomrightlat?}/{bottomrightlng?}")]
        [HttpGet]
        public IHttpActionResult GetFindPinsByAddress([FromUri]string userSearchAddress, [FromUri]string lat = "0", [FromUri]string lng = "0", [FromUri]string upperleftlat = "0", [FromUri]string upperleftlng = "0", [FromUri]string bottomrightlat = "0", [FromUri]string bottomrightlng = "0")
        {
            try
            {

                AwsBoundingBox boundingBox = null;
                if (upperleftlat != "0" && upperleftlng != "0" && bottomrightlat != "0" && bottomrightlng != "0")
                {
                    boundingBox = _awsCloudsearchService.BuildBoundingBox(upperleftlat, upperleftlng, bottomrightlat, bottomrightlng);
                }
               
                var originCoords = _finderService.GetGeoCoordsFromAddressOrLatLang(userSearchAddress, lat, lng);
                var pinsInRadius = _finderService.GetPinsInBoundingBox(originCoords, userSearchAddress, boundingBox);

                foreach (var pin in pinsInRadius)
                {
                    if (pin.PinType != PinType.SITE)
                    {
                        pin.Address = _finderService.RandomizeLatLong(pin.Address);
                    }
                }

                var result = new PinSearchResultsDto(new GeoCoordinates(originCoords.Latitude, originCoords.Longitude), pinsInRadius);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Pin By Address Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [RequiresAuthorization]
        [ResponseType(typeof(PinSearchResultsDto))]                                   
        [VersionedRoute(template: "finder/findmypinsbycontactid/{contactId}/{lat}/{lng}", minimumVersion: "1.0.0")]
        [Route("finder/findmypinsbycontactid/{contactId}/{lat}/{lng}")]
        [HttpGet]
        public IHttpActionResult GetMyPinsByContactId([FromUri]int contactId, [FromUri]string lat, [FromUri]string lng)
        {
            return Authorized(token =>
            {
                try
                {
                    var originCoords = _finderService.GetGeoCoordsFromLatLong(lat, lng);
                    var centerLatitude = originCoords.Latitude;
                    var centerLongitude = originCoords.Longitude;

                    var pinsForContact = _finderService.GetMyPins(token, originCoords, contactId);

                    if (pinsForContact.Count > 0)
                    {
                        var addressLatitude = pinsForContact[0].Address.Latitude;
                        if (addressLatitude != null) centerLatitude = (double)addressLatitude;

                        var addressLongitude = pinsForContact[0].Address.Longitude;
                        if (addressLongitude != null) centerLongitude = (double)addressLongitude;
                    }

                    var result = new PinSearchResultsDto(new GeoCoordinates(centerLatitude, centerLongitude), pinsForContact);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Get Pins for My Stuff Failed", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
        /// <summary>
        /// Logged in user invites a participant to the gathering
        /// </summary>
        [RequiresAuthorization]
        [VersionedRoute(template: "finder/pin/invitetogathering/{gatheringId}", minimumVersion: "1.0.0")]
        [Route("finder/pin/invitetogathering/{gatheringId}")]
        [HttpPost]
        public IHttpActionResult InviteToGathering([FromUri] int gatheringId, [FromBody] User person)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(val => val.Errors).Aggregate("", (current, err) => current + err.Exception.Message);
                var dataError = new ApiErrorDto("CreateInvitation Data Invalid", new InvalidOperationException("Invalid CreateInvitation Data " + errors));
                throw new HttpResponseException(dataError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _finderService.InviteToGathering(token, gatheringId, person);
                    return (Ok());
                }
                catch (ValidationException e)
                {
                    var error = new ApiErrorDto("Not authorized to send invitations of this type", e, HttpStatusCode.Forbidden);
                    throw new HttpResponseException(error.HttpResponseMessage);
                }
                catch (Exception e)
                {
                    _logger.Error($"Could not create invitation to recipient {person.firstName + " " + person.lastName} ({person.email}) for group {3}", e);
                    var apiError = new ApiErrorDto("CreateInvitation Failed", e, HttpStatusCode.InternalServerError);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Logged in user requests to join gathering
        /// </summary>
        [RequiresAuthorization]
        [VersionedRoute(template: "finder/pin/gatheringjoinrequest", minimumVersion: "1.0.0")]
        [Route("finder/pin/gatheringjoinrequest")]
        [HttpPost]
        public IHttpActionResult GatheringJoinRequest([FromBody]int gatheringId)
        {
            return Authorized(token =>
            {
                try
                {
                    _finderService.GatheringJoinRequest(token, gatheringId);
                    return (Ok());
                }
                catch (Exception e)
                {
                    _logger.Error("Could not generate request", e);
                    if (e.Message == "User already has request")
                    {
                        throw new HttpResponseException(HttpStatusCode.Conflict);
                    }
                    else if (e.Message == "User already a member")
                    {
                        throw new HttpResponseException(HttpStatusCode.NotAcceptable);
                    }
                    else
                    {
                        throw new HttpResponseException(new ApiErrorDto("Gathering request failed", e).HttpResponseMessage);
                    }

                }
            });
        }

        /// <summary>
        /// Logged in user requests to join gathering
        /// </summary>
        [RequiresAuthorization]
        [VersionedRoute(template: "finder/sayhi/{fromId}/{toId}", minimumVersion: "1.0.0")]
        [Route("finder/sayhi/{fromId}/{toId}")]
        [HttpPost]
        public IHttpActionResult SayHi([FromUri]int fromId, [FromUri]int toId)
        {
            return Authorized(token =>
            {
                try
                {
                    _finderService.SayHi(fromId, toId);
                    return Ok();
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Say Hi Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        /// <summary>
        /// Logged in user requests to be a host
        /// </summary>
        [RequiresAuthorization]
        [VersionedRoute(template: "finder/pin/requesttobehost", minimumVersion: "1.0.0")]
        [Route("finder/pin/requesttobehost")]
        [HttpPost]
        public IHttpActionResult RequestToBeHost([FromBody]HostRequestDto hostRequest)
        {
            return Authorized(token =>
            {
                try
                {
                    _finderService.RequestToBeHost(token, hostRequest);
                    return Ok();
                }
                catch (GatheringException e)
                {
                    _logger.Error("Host already has a gathering at this location.", e);
                    throw new HttpResponseException(HttpStatusCode.NotAcceptable);
                }
                catch (Exception e)
                {
                    _logger.Error("Could not generate request", e);
                    throw new HttpResponseException(new ApiErrorDto("Gathering request failed", e).HttpResponseMessage);
                }
            });
        }

        [ResponseType(typeof(PinSearchResultsDto))]
        [VersionedRoute(template: "finder/uploadallcloudsearchrecords", minimumVersion: "1.0.0")]
        [Route("finder/uploadallcloudsearchrecords")]
        [HttpGet]
        public IHttpActionResult UploadAllCloudsearchRecords()
        {
            try
            {
                var response = _awsCloudsearchService.UploadAllConnectRecordsToAwsCloudsearch();
                return Ok(response);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("UploadAllCloudsearchRecords", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [ResponseType(typeof(PinSearchResultsDto))]
        [VersionedRoute(template: "finder/deleteallcloudsearchrecords", minimumVersion: "1.0.0")]
        [Route("finder/deleteallcloudsearchrecords")]
        [HttpGet]
        public IHttpActionResult DeleteAllCloudsearchRecords()
        {
            try
            {
                var response = _awsCloudsearchService.DeleteAllConnectRecordsInAwsCloudsearch();
                return Ok(response);
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("DeleteAllCloudsearchRecords Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }



        /// <summary>
        /// Allows an invitee to accept or deny a group invitation.
        /// </summary>
        /// <param name="groupId">An integer identifying the group that the invitation is associated to.</param>
        /// <param name="invitationKey">An string identifying the private invitation.</param>
        /// <param name="accept">A boolean showing if the invitation is being approved or denied.</param>
        [AcceptVerbs("POST")]
        // note - This AcceptVerbs attribute on an entry with the Http* Method attribute causes the
        //        API not to be included in the swagger output. We're doing it because there's a fail
        //        in the swagger code when the body has a boolean in it that breaks in the JS causing
        //        the GroopTool and all subsequent controller APIs not to show on the page. This is a
        //        stupid fix for a bug that is out of our control.
        [RequiresAuthorization]
        [VersionedRoute(template: "finder/group/{groupId}/invitation/{invitationKey}", minimumVersion: "1.0.0")]
        [Route("finder/group/{groupId:int}/invitation/{invitationKey}")]
        [HttpPost]
        public IHttpActionResult ApproveDenyGroupInvitation([FromUri] int groupId, [FromUri] string invitationKey, [FromBody] bool accept)
        {
            return Authorized(token =>
            {
                try
                {
                    _finderService.AcceptDenyGroupInvitation(token, groupId, invitationKey, accept);
                    return Ok();
                }
                catch (GroupParticipantRemovalException e)
                {
                    throw new HttpResponseException(HttpStatusCode.NotAcceptable);
                }
                catch (DuplicateGroupParticipantException e)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto($"Error when accepting: {accept}, for group {groupId}", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
