using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using RestSharp;

namespace crds_angular.Services
{
    public class CorkboardService: ICorkboardService
    {
        private readonly IRestClient _corkboardRestClient;
        private readonly IConfigurationWrapper _configuration;

        public CorkboardService(IRestClient corkboardRestClient, IConfigurationWrapper configuration)
        {
            _corkboardRestClient = corkboardRestClient;
            _configuration = configuration;
        }

            
        public void SyncPosts()
        {
            // TODO: Implement code
        }
    }
}
