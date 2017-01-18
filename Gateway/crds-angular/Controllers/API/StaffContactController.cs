using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class StaffContactController : MPAuth
    {
        private readonly crds_angular.Services.Interfaces.IStaffContactService _contactService;

        public StaffContactController(crds_angular.Services.Interfaces.IStaffContactService contactService, IUserImpersonationService userImpersonationService) : base(userImpersonationService)
        {
            _contactService = contactService;
        }

        [ResponseType(typeof (List<Dictionary<string, object>>))]
        [VersionedRoute(template: "staff-contacts", minimumVersion: "1.0.0")]
        [Route("staffcontacts")]
        public IHttpActionResult Get()
        {
            return Authorized(t =>
            {
                try
            {
                var users = _contactService.GetStaffContacts();

                return Ok(users);
            }
                catch (Exception e)
                {
                    var msg = "Error getting staff users ";
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}
