﻿using crds_angular.Models;
using crds_angular.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.SessionState;
using System.Net.Http.Headers;
using crds_angular.Security;

namespace crds_angular.Controllers.API
{
    public class LoginController : CookieAuth
    {

        [ResponseType(typeof(LoginReturn))]
        [HttpGet]
        [Route("api/authenticated")]
        public IHttpActionResult isAuthenticated()
        {
            return Authorized(token =>
            {
                var person = PersonService.getLoggedInUserProfile(token);
                if (person == null)
                {
                    return this.Unauthorized();
                }
                else
                {
                    var l = new LoginReturn(token, person.Contact_Id, person.First_Name);
                    return this.Ok(l);
                }
            });
        }

        [ResponseType(typeof(LoginReturn))]
        public IHttpActionResult Post([FromBody]Credentials cred)
        {
            
            
            // try to login 
            var token = TranslationService.Login(cred.username, cred.password);
            if (token == null)
            {
                return this.Unauthorized();
            } 
            var p = ProfileService.getLoggedInUserProfile(token);
            var r = new LoginReturn
            {
                userToken = token,
                userId = p.person.Contact_Id,
                username = p.person.First_Name
            };
            return this.Ok(r);
        }
    }

    public class LoginReturn
    {
        public LoginReturn(){}
        public LoginReturn(string userToken, int userId, string username){
            this.userId = userId;
            this.userToken = userToken;
            this.username = username;
        }
        public string userToken { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
    }

    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
