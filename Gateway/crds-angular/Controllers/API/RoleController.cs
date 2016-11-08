using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class RoleController : MPAuth
    {

        private readonly IContactRepository _contactSerivce;

        public RoleController(IContactRepository contactSerivce)
        {
            _contactSerivce = contactSerivce;
        }

        [ResponseType(typeof(IList<int>))]
        [VersionedRoute(template: "role", minimumVersion: "1.0.0")]
        [Route("role")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Authorized(token =>
            {
                try
                {
                    var contactIds = _contactSerivce.GetContactIdByRoleId(id, token);
                    return this.Ok(contactIds);
                }
                catch (Exception )
                {
                    return BadRequest();
                }
            });
        }
    }
}