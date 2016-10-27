using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class AddressController : ApiController
    {
        private readonly IAddressGeocodingService _addressGeocodingService;

        public AddressController(IAddressGeocodingService addressGeocodingService)
        {
            _addressGeocodingService = addressGeocodingService;
        }

        /// <summary>
        /// Validate an input address by first geocoding, then returning the parsed address components, including the latitude and longitude.  Note that this is not a true street-level address validation.  This simply verifies that a given input can be geocoded, which might mean that address components are moved in order to get to a city/zip center, if the given street address is not found.
        /// </summary>
        /// <param name="address">An <see cref="AddressDTO">AddressDTO</see> with individual components populated</param>
        /// <returns>An <see cref="AddressDTO">AddressDTO</see> with parsed components including latitude and longitude.  This will return a 404/Not Found if the input address could not be located.</returns>
        [ResponseType(typeof(AddressDTO))]
        [VersionedRoute(template: "address/validate", minimumVersion: "1.0.0")]
        [Route("address/validate")]
        [HttpPost]
        public IHttpActionResult ValidateAddress(AddressDTO address)
        {
            return ValidateAddress(address.ToString());
        }

        /// <summary>
        /// Validate an input address by first geocoding, then returning the parsed address components, including the latitude and longitude.  Note that this is not a true street-level address validation.  This simply verifies that a given input can be geocoded, which might mean that address components are moved in order to get to a city/zip center, if the given street address is not found.
        /// </summary>
        /// <param name="address">An address string</param>
        /// <returns>An <see cref="AddressDTO">AddressDTO</see> with parsed components including latitude and longitude.  This will return a 404/Not Found if the input address could not be located.</returns>
        [ResponseType(typeof(AddressDTO))]
        [VersionedRoute(template: "address/validate", minimumVersion: "1.0.0")]
        [Route("address/validate")]
        [HttpGet]
        public IHttpActionResult ValidateAddress([FromUri(Name = "address")]string address)
        {
            try
            {
                return Ok(_addressGeocodingService.ValidateAddress(address));
            }
            catch (InvalidAddressException)
            {
                var apiError = new ApiErrorDto($"Invalid address {address}", code: HttpStatusCode.NotFound);
                return ResponseMessage(apiError.HttpResponseMessage);
            }
        }
    }
}