using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using Crossroads.ApiVersioning;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;

namespace crds_angular.Controllers.API
{
    public class UserController : MPAuth
    {

        private readonly IAccountService _accountService;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        // Do not change this string without also changing the same in the corejs register_controller
        private const string DUPLICATE_USER_MESSAGE = "Duplicate User";

        public UserController(IAccountService accountService, IMinistryPlatformRestRepository ministryPlatformRest)
        {
            _accountService = accountService;
            _ministryPlatformRest = ministryPlatformRest;
        }

        [ResponseType(typeof(User))]
        [VersionedRoute(template: "user", minimumVersion: "1.0.0")]
        [Route("user")]
        [HttpPost]
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

        [RequiresAuthorization]
        [VersionedRoute(template: "user", minimumVersion: "1.0.0")]
        [Route("user")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Authorized(token =>
            {
                try
                {
                    List<MpUserDto> mpUserDtos = _ministryPlatformRest.UsingAuthenticationToken(token).Get<MpUserDto>("dp_Users",new Dictionary<string, object>());
                    List<MpUser> mpUsers = AutoMapper.Mapper.Map<List<MpUserDto>, List<MpUser>>(mpUserDtos);
                    return Ok(mpUsers);
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto($"{e.Message}");
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });


            
        }

    }
}
