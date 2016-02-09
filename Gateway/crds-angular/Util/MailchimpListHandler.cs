using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using Crossroads.Utilities.Interfaces;
using System.Net;
using Crossroads.Utilities.Serializers;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using crds_angular.Models.Crossroads.Subscription;
using crds_angular.Models.MailChimp;

namespace crds_angular.Util
{
    public class MailchimpListHandler : Interfaces.IEmailListHandler
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MailchimpListHandler));
        private readonly IConfigurationWrapper _configWrapper;
        private readonly MPInterfaces.IBulkEmailRepository _bulkEmailRepository;

        public MailchimpListHandler(IConfigurationWrapper configWrapper, MPInterfaces.IBulkEmailRepository bulkEmailRepository)
        {
            _configWrapper = configWrapper;
            _bulkEmailRepository = bulkEmailRepository;
        }

        public OptInResponse AddListSubscriber(string email, string listName, string token)
        {
            var publications = _bulkEmailRepository.GetPublications(token);
            string publicationId = publications.First(r => r.Title == listName).ThirdPartyPublicationId;

            var client = GetEmailClient();

            // query mailchimp to see if the subscriber is present on the list
            var subscriberStatusRequest = new RestRequest("lists/" + publicationId + "/members/" + CalculateMD5Hash(email),Method.GET);
            subscriberStatusRequest.AddHeader("Content-Type", "application/json");

            var subscriberStatusResponse = client.Execute(subscriberStatusRequest);

            if (subscriberStatusResponse.StatusCode != HttpStatusCode.OK)
            {
                // Mailchimp will return a not found error, if the member is not on a list -- since it's a 404, this
                // additional step will determine if there is actually a problem connecting to the mailchimp list
                var mailchimpStatusRequest = new RestRequest("lists/" + publicationId, Method.GET);
                var mailchimpStatusResponse = client.Execute(mailchimpStatusRequest);

                if (mailchimpStatusResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.ErrorFormat("Failed adding subscriber {0} for list {1} with StatusCode = {2}", email, listName, subscriberStatusResponse.StatusCode);
                    return new OptInResponse
                    {
                        ErrorInSignupProcess = true,
                        UserAlreadySubscribed = false
                    };
                }
            }
            // do this to notify the user if they're already signed up and need to look for an 
            // opt-in message or take other action
            else
            {
                var responseContent = subscriberStatusResponse.Content;
                var responseContentJson = JObject.Parse(responseContent);

                BulkEmailSubscriberOptDTO subscriber = JsonConvert.DeserializeObject<BulkEmailSubscriberOptDTO>(responseContentJson.ToString());

                if (subscriber.Status == "subscribed" || subscriber.Status == "pending")
                {
                    return new OptInResponse
                    {
                        ErrorInSignupProcess = false,
                        UserAlreadySubscribed = true
                    };
                }
            }

            // create opt in request, if the person isn't already subscribed
            var request = new RestRequest("lists/" + publicationId + "/members/" + CalculateMD5Hash(email));
            request.AddHeader("Content-Type", "application/json");

            request.JsonSerializer = new RestsharpJsonNetSerializer();
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.PUT;

            Dictionary<string, string> requestBody = new Dictionary<string, string>();
            requestBody.Add("email_address", email);
            requestBody.Add("status", "pending");
            request.AddBody(requestBody);

            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.ErrorFormat("Failed adding subscriber {0} for list {1} with StatusCode = {2}", email, listName, response.StatusCode);
                return new OptInResponse
                {
                    ErrorInSignupProcess = true,
                    UserAlreadySubscribed = false
                };
            }
            else
            {
                return new OptInResponse
                {
                    ErrorInSignupProcess = false,
                    UserAlreadySubscribed = false
                };
            }
        }

        private RestClient GetEmailClient()
        {
            var apiUrl = _configWrapper.GetConfigValue("BulkEmailApiUrl");
            var apiKey = _configWrapper.GetEnvironmentVarAsString("BULK_EMAIL_API_KEY");

            var client = new RestClient(apiUrl);
            client.Authenticator = new HttpBasicAuthenticator("noname", apiKey);

            return client;
        }

        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private Dictionary<string, object> DeserializeToDictionary(string jo)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
            var values2 = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> d in values)
            {
                if (d.Value is JObject)
                {
                    values2.Add(d.Key, DeserializeToDictionary(d.Value.ToString()));
                }
                else
                {
                    values2.Add(d.Key, d.Value);
                }
            }
            return values2;
        }
    }
}