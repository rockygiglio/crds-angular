using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Twilio;

namespace crds_angular.Services
{
    public class TwilioService : ITextCommunicationService
    {
        private readonly string _accountSid;
        private readonly TwilioRestClient _twilio;

        public TwilioService(IConfigurationWrapper configurationWrapper)
        {
            var configurationWrapper1 = configurationWrapper;
            _accountSid = configurationWrapper1.GetConfigValue("TwilioAccountSid");
            var authToken = configurationWrapper1.GetConfigValue("TwilioAuthToken");
            _twilio = new TwilioRestClient(_accountSid, authToken);
        }

        public void SendTextMessage(string toPhoneNumber, string body)
        {
            var message = _twilio.SendMessage(_accountSid, toPhoneNumber, body);
        }
    }
}
