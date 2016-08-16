using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Web.Http;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using crds_angular.Models.Crossroads;
using crds_angular.Exceptions.Models;
using crds_angular.Models;
using crds_angular.Processors;
using Crossroads.Utilities.Messaging.Interfaces;
using Crossroads.Utilities.Messaging;
using log4net;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.Controllers.API
{
    public class TextCommunicationController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TextCommunicationController));

        private readonly IMessageQueue _messageQueue;
        private readonly IMessageFactory _messageFactory;
        private readonly ITextCommunicationService _textCommunicationService;
        private string eventQueueName;
        private int streamReminderTemplateId;

        public TextCommunicationController(ITextCommunicationService textCommunicationService, IConfigurationWrapper configurationWrapper,
            IMessageQueueFactory messageQueueFactory = null, IMessageFactory messageFactory = null, IMessageQueue messageQueue = null)
        {
            _textCommunicationService = textCommunicationService;
            eventQueueName = configurationWrapper.GetConfigValue("ScheduledJobsQueueName");
            streamReminderTemplateId = configurationWrapper.GetConfigIntValue("StreamReminderTemplate");
            _messageQueue = messageQueue;
            //_messageQueue = new MessageQueueImpl(messageQueueFactory.CreateQueue(eventQueueName, QueueAccessMode.Send));
            //messageQueue = messageQueueFactory.CreateQueue(eventQueueName, QueueAccessMode.Send);
            _messageFactory = messageFactory;
        }

        /// <summary>
        /// Schedule a text message to a specific contactId/number at a specific time
        /// </summary>
        [Route("api/scheduletext")]
        public IHttpActionResult PostReminder(TextCommunicationDto textCommunicationData)
        {
            if (textCommunicationData == null || !ModelState.IsValid)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("Received invalid schedule text event " + textCommunicationData);
                }
                return (BadRequest(ModelState));
            }

            _logger.Debug("Received schedule text request " + textCommunicationData);

            try
            {
                // Hardcode template ID for now to mitigate misuse risk
                textCommunicationData.TemplateId = streamReminderTemplateId;

                ScheduledJob scheduledJob = new ScheduledJob();
                scheduledJob.StartDateTime = textCommunicationData.StartDate.Value;
                scheduledJob.JobType = typeof(SendTextMessageJob);
                scheduledJob.Dto = textCommunicationData;

                var message = _messageFactory.CreateMessage(scheduledJob);

                if (!_messageQueue.Exists(eventQueueName))
                {
                    _messageQueue.CreateQueue(eventQueueName, QueueAccessMode.Send);
                }

                _messageQueue.Send(message, MessageQueueTransactionType.None);
            }
            catch (Exception e)
            {
                var msg = "Unexpected error processing schedule text request " + textCommunicationData;
                _logger.Error(msg, e);
                var apiError = new ApiErrorDto("Schedule Text failed", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }

            return this.Ok();

        }

    }
}
