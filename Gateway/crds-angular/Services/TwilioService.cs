﻿using System;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Twilio;

namespace crds_angular.Services
{
    public class TwilioService : ITextCommunicationService
    {
        public ILog _logger = LogManager.GetLogger(typeof(TwilioService));

        private readonly string _fromPhoneNumber;
        private readonly TwilioRestClient _twilio;

        public TwilioService(IConfigurationWrapper configurationWrapper)
        {
            var accountSid = configurationWrapper.GetConfigValue("TwilioAccountSid");
            var authToken = configurationWrapper.GetEnvironmentVarAsString("TWILIO_AUTH_TOKEN");
            _fromPhoneNumber = configurationWrapper.GetConfigValue("TwilioFromPhoneNumber");
            _twilio = new TwilioRestClient(accountSid, authToken);
        }

        public void SendTextMessage(string toPhoneNumber, string body)
        {
            _logger.Debug("Sending text message to "+ toPhoneNumber);
            var message = _twilio.SendMessage(_fromPhoneNumber, toPhoneNumber, body);
            if (message.RestException != null)
            {
                _logger.Error(message.RestException.Message);
            }
        }

        public void SetLogger(ILog logger)
        {
            _logger = logger;
        }
    }
}
