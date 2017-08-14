﻿using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Analytics;
using Crossroads.ApiVersioning;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Controllers.API
{
    public class UserController : MPAuth
    {
        private readonly IAccountService _accountService;
        private readonly IContactRepository _contactRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAnalyticsService _analyticsService;
        // Do not change this string without also changing the same in the corejs register_controller
        private const string DUPLICATE_USER_MESSAGE = "Duplicate User";

        public UserController(IAccountService accountService, 
                                IContactRepository contactRepository, 
                                IUserRepository userRepository, 
                                IAnalyticsService analyticsService,
                                IUserImpersonationService userImpersonationService, 
                                IAuthenticationRepository authenticationRepository) : base(userImpersonationService, authenticationRepository)
        {
            _accountService = accountService;
            _contactRepository = contactRepository;
            _userRepository = userRepository;
            _analyticsService = analyticsService;
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
                _analyticsService.Track(userRecord.email, "SignedUp");
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
        [ResponseType(typeof(User))]
        [Route("user")]
        [HttpGet]
        public IHttpActionResult Get(string username)
        {
            return Authorized(token =>
            {
                try
                {                    
                    int userid = _userRepository.GetUserIdByUsername(username);
                    MpUser user = _userRepository.GetUserByRecordId(userid);
                    var userRoles = _userRepository.GetUserRoles(userid);
                    MpMyContact contact = _contactRepository.GetContactByUserRecordId(user.UserRecordId);

                    var r = new LoginReturn
                    {
                        userToken = token,
                        userTokenExp = "",
                        refreshToken = "",
                        userId = contact.Contact_ID,
                        username = contact.First_Name,
                        userEmail = contact.Email_Address,
                        roles = userRoles,
                        age = contact.Age,
                        userPhone = contact.Mobile_Phone,
                        canImpersonate = user.CanImpersonate
                    };

                    return Ok(r);
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
