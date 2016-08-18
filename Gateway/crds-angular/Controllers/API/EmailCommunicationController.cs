using System;
using System.Web.Http;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Repositories.Interfaces;
using crds_angular.Exceptions.Models;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.Controllers.API
{
    public class EmailCommunicationController : MPAuth
    {
        private readonly IEmailCommunication _emailCommunication;
        private readonly IConfigurationWrapper _configurationWrapper;

        public EmailCommunicationController(IEmailCommunication emailCommunication, IConfigurationWrapper configurationWrapper)
        {
            _emailCommunication = emailCommunication;
            _configurationWrapper = configurationWrapper;
        }

        /// <summary>
        /// Send an email to a specific contactId
        /// </summary>
        [RequiresAuthorization]
        [Route("api/sendemail")]
        public IHttpActionResult Post(EmailCommunicationDTO email)
        {
            return Authorized(token =>
            {
                try
                {
                    _emailCommunication.SendEmail(email, token);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            });
        }

        /// <summary>
        /// Send an email to a group, takes in a list of contactId's
        /// </summary>
        [RequiresAuthorization]
        [Route("api/sendgroupemail")]
        public IHttpActionResult Post([FromBody] CommunicationDTO communication)
        {
            return Authorized(token =>
            {
                try
                {
                    _emailCommunication.SendEmail(communication);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            });
        }

        /// <summary>
        /// Schedule an email to a specific contactId/emailAddress at a specific time
        /// </summary>
        [Route("api/sendEmailReminder")]
        public IHttpActionResult PostReminder(EmailCommunicationDTO email)
        {
            try
            {
                email.TemplateId = _configurationWrapper.GetConfigIntValue("StreamReminderTemplate");
                _emailCommunication.SendEmail(email);
                return this.Ok();
            }
            catch (System.Exception ex)
            {
                var apiError = new ApiErrorDto("Schedule Email failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}
