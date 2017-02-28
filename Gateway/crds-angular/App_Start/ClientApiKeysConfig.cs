using System.Web.Http;
using Crossroads.ClientApiKeys;

namespace crds_angular
{
    public static class ClientApiKeysConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Add a filter to verify domain-locked API keys
            var domainLockedApiKeyFilter = (DomainLockedApiKeyFilter)config.DependencyResolver.GetService(typeof(DomainLockedApiKeyFilter));
            config.Filters.Add(domainLockedApiKeyFilter);
        }

    }
}