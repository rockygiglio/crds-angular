﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Json;
using crds_angular.Security;
using crds_angular.Services;
using crds_angular.Services.Analytics;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models.DTO;
using Crossroads.ApiVersioning;
using Crossroads.ClientApiKeys;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;


namespace crds_angular.Controllers.API
{
    public class LoginController : MPAuth
    {

        private readonly IPersonService _personService;
        private readonly IUserRepository _userService;
        private readonly ILoginService _loginService;

        private readonly IContactRepository _contactRepository;
        private readonly IAnalyticsService _analyticsService;

        public LoginController(ILoginService loginService, 
                                IPersonService personService, 
                                IUserRepository userService, 
                                IAnalyticsService analyticsService,
                                IUserImpersonationService userImpersonationService, 
                                IAuthenticationRepository authenticationRepository,
                                IContactRepository contactRepository) : base(userImpersonationService, authenticationRepository)

        {
            _loginService = loginService;
            _personService = personService;
            _userService = userService;

            _contactRepository = contactRepository;
            _analyticsService = analyticsService;

        }

        [VersionedRoute(template: "request-password-reset", minimumVersion: "1.0.0")]
        [Route("requestpasswordreset")]
        [HttpPost]
        public IHttpActionResult RequestPasswordReset(PasswordResetRequest request)
        {
            try
            {
                _loginService.PasswordResetRequest(request.Email);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.InternalServerError();
            }
        }

        [VersionedRoute(template: "verify-reset-token/{token}", minimumVersion: "1.0.0")]
        [Route("verifyresettoken/{token}")]
        [HttpGet]
        public IHttpActionResult VerifyResetTokenRequest(string token)
        {
            try
            {
                ResetTokenStatus status = new ResetTokenStatus();
                status.TokenValid = _loginService.VerifyResetToken(token);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return this.InternalServerError();
            }
        }

        [VersionedRoute(template: "reset-password", minimumVersion: "1.0.0")]
        [Route("resetpassword")]
        [HttpPost]
        public IHttpActionResult ResetPassword(PasswordReset request)
        {
            try
            {
                var userEmail = _loginService.ResetPassword(request.Password, request.Token);
                return Ok();
            }
            catch (Exception ex)
            {
                return this.InternalServerError();
            }
        }

        [ResponseType(typeof (LoginReturn))]
        [VersionedRoute(template: "authenticated", minimumVersion: "1.0.0")]
        [Route("authenticated")]
        [HttpGet]
        public IHttpActionResult isAuthenticated()
        {
            return Authorized(token =>
            {
                try
                {
                    //var personService = new PersonService();
                    var person = _personService.GetLoggedInUserProfile(token);

                    if (person == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        var roles = _personService.GetLoggedInUserRoles(token);
                        var user = _userService.GetByAuthenticationToken(token);
                        var l = new LoginReturn(token, person.ContactId, person.FirstName, person.EmailAddress, person.MobilePhone, roles, user.CanImpersonate);
                        return this.Ok(l);
                    }
                }
                catch (Exception)
                {
                    return this.Unauthorized();
                }
            });
        }

        [VersionedRoute(template: "login", minimumVersion: "1.0.0")]
        [Route("login")]
        [ResponseType(typeof (LoginReturn))]
        // TODO - Once Ez-Scan has been updated to send a client API key (US7764), remove the IgnoreClientApiKey attribute
        [IgnoreClientApiKey]
        public IHttpActionResult Post([FromBody] Credentials cred)
        {
            try
            {
                // try to login
                var authData = AuthenticationRepository.Authenticate(cred.username, cred.password);
                var token = authData.AccessToken;
                var exp = authData.ExpiresIn+"";
                var refreshToken = authData.RefreshToken;

                if (token == "")
                {
                    return this.Unauthorized();
                }

                var userRoles = _personService.GetLoggedInUserRoles(token);
                var user = _userService.GetByAuthenticationToken(token);
                var p = _personService.GetLoggedInUserProfile(token);
                var r = new LoginReturn
                {
                    userToken = token,
                    userTokenExp = exp,
                    refreshToken = refreshToken,
                    userId = p.ContactId,
                    username = p.FirstName,
                    userEmail = p.EmailAddress,
                    roles = userRoles,
                    age = p.Age,
                    userPhone = p.MobilePhone,
                    canImpersonate = user.CanImpersonate 
                };


                _loginService.ClearResetToken(cred.username);
                _contactRepository.UpdateUsertoActive(p.ContactId);
                _analyticsService.Track(cred.username, "SignedIn"); 


                return this.Ok(r);
            }
            catch (Exception e)
            {
                var apiError = new ApiErrorDto("Login Failed", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }

        [VersionedRoute(template: "verify-credentials", minimumVersion: "1.0.0")]
        [Route("verifycredentials")]
        [HttpPost]
        public IHttpActionResult VerifyCredentials([FromBody] Credentials cred)
        {
            return Authorized(token =>
            {
                try
                {
                    var authData = AuthenticationRepository.Authenticate(cred.username, cred.password);

                    // if the username or password is wrong, auth data will be null
                    if (authData == null)
                    {
                        return this.Unauthorized();
                    }
                    else
                    {
                        return this.Ok();
                    }
                }
                catch (Exception e)
                {
                    var apiError = new ApiErrorDto("Verify Credentials Failed", e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

    }

    public class LoginReturn
    {
        public LoginReturn(){}
        public LoginReturn(string userToken, int userId, string username, string userEmail, string userPhone, List<MpRoleDto> roles, Boolean canImpersonate){
            this.userId = userId;
            this.userToken = userToken;
            this.username = username;
            this.userEmail = userEmail;
            this.userPhone = userPhone;
            this.roles = roles;
            this.canImpersonate = canImpersonate;
        }
        public string userToken { get; set; }
        public string userTokenExp { get; set; }
        public string refreshToken { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string userEmail { get; set;  }
        public List<MpRoleDto> roles { get; set; }
        public Boolean canImpersonate { get; set; }
        public int age { get; set; }
        public string userPhone { get; set; }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }

}
