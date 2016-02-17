using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using crds_angular.Exceptions.Models;

namespace crds_angular.Controllers.API
{
    public class UserController : ApiController
    {

        private readonly IAccountService _accountService;
        // Do not change this string without also changing the same in the corejs register_controller
        private const string DUPLICATE_USER_MESSAGE = "Duplicate User";

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [ResponseType(typeof(User))]
        public IHttpActionResult Post([FromBody] User user)
        {
            try
            {
                var userRecord = _accountService.RegisterPerson(user);
                return Ok(userRecord);
            }
            catch (DuplicateUserException e)
            {
                var apiError = new ApiErrorDto(DUPLICATE_USER_MESSAGE, e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
            catch (ContactEmailExistsException contactException)
            {
                var apiError = new ApiErrorDto(string.Format("{0}", contactException.ContactId()), contactException, HttpStatusCode.Conflict);
                throw new HttpResponseException(apiError.HttpResponseMessage);                
            }
        }
    }
}
