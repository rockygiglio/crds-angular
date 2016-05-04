using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using Newtonsoft.Json.Linq;

namespace crds_angular.Services
{
    public class PostsService : IPostsService
    {
        private readonly IConfigurationWrapper _configuration;
        private readonly IGatewayWrapper _gateway;
        private readonly IEmailService _email;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPostRepository _postRepository;
        private readonly ICloudSearchWrapper _cloudSearchWrapper;

        public PostService(IConfigurationWrapper configuration, IGatewayWrapper gateway, IEmailService email, IPostRepository postRepository, ICloudSearchWrapper cloudSearchWrapper)
        {
            _configuration = configuration;
            _gateway = gateway;
            _email = email;
            _postRepository = postRepository;
            _cloudSearchWrapper = cloudSearchWrapper;
        }

        public JObject FlagPost(string id)
        {
            try
            {
                var result = _postRepository.Flag(id);

                var preSaveFlagCount = result["FlagCount"] != null ? result["FlagCount"].Value<int>() : 0;
                var title = result["Title"].Value<string>();

                if (preSaveFlagCount == 5)
                {
                    SendEmails(id, title);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error("Error: Could not update flag count", ex);
                throw;
            }
        }

        public JArray GetPosts()
        {
            try
            {
                var posts = _postRepository.GetAll();
                return posts;
            }
            catch (Exception ex)
            {
                _logger.Error("Error: Could not GetAllPosts", ex);
                throw;
            }
        }

        public JObject SavePost(JToken postItem, dynamic auth)
        {
            var post = _postRepository.Save(postItem, auth);
            _cloudSearchWrapper.SyncPostWithSearchIndex(post);
            return post;
        }


        private void SendEmails(string itemId, string title)
        {
            var apiUser = _gateway.GetApiUserInformation();
            var apiToken = apiUser.userToken.ToString();

            var recipientContactIds = _email.GetRecipientContactIds(apiToken);

            var templateId = _configuration.GetConfigIntValue("FlagPostTemplateId");
            var coreWebsite = _configuration.GetConfigValue("CoreWebsite");
            var link = coreWebsite + "corkboard/#/detail/" + itemId;

            int fromContactId = apiUser.userId;
            foreach (var recipientContactId in recipientContactIds)
            {
                _email.SendEmail(apiToken, fromContactId, recipientContactId, templateId, title, link);
            }
        }

        public JObject SyncCloudsearchPosts()
        {
            _postRepository.SyncCloudsearchPosts();
            return new JObject();
        }
    }
}