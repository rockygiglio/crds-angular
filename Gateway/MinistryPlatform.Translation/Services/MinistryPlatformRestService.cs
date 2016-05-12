using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MinistryPlatform.Translation.Extensions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace MinistryPlatform.Translation.Services
{
    public class MinistryPlatformRestService
    {
        private readonly IRestClient _ministryPlatformRestClient;
        private readonly ThreadLocal<string> _authToken = new ThreadLocal<string>();

        public MinistryPlatformRestService(IRestClient ministryPlatformRestClient)
        {
            _ministryPlatformRestClient = ministryPlatformRestClient;
        }

        public MinistryPlatformRestService UsingAuthenticationToken(string authToken)
        {
            _authToken.Value = authToken;
            return this;
        }

        public T Get<T>(int recordId, string selectColumns = null)
        {
            var url = AddColumnSelection($"/tables/{GetTableName<T>()}/{recordId}", selectColumns);
            var request = new RestRequest(url, Method.GET);
            AddAuthorization(request);

            var response = _ministryPlatformRestClient.Execute(request);
            _authToken.Value = null;
            response.CheckForErrors($"Error getting {GetTableName<T>()} by ID {recordId}", true);

            var content = JsonConvert.DeserializeObject<List<T>>(response.Content);
            if (content == null || !content.Any())
            {
                return default(T);
            }

            return content.FirstOrDefault();
        }

        public List<T> Search<T>(string searchString = null, string selectColumns = null)
        {
            var search = string.IsNullOrWhiteSpace(searchString) ? string.Empty : $"?$filter={searchString}";

            var url = AddColumnSelection($"/tables/{GetTableName<T>()}{search}", selectColumns);
            var request = new RestRequest(url, Method.GET);
            AddAuthorization(request);

            var response = _ministryPlatformRestClient.Execute(request);
            _authToken.Value = null;
            response.CheckForErrors($"Error searching {GetTableName<T>()}");

            var content = JsonConvert.DeserializeObject<List<T>>(response.Content);

            return content;
        }

        private void AddAuthorization(IRestRequest request)
        {
            if (_authToken.IsValueCreated)
            {
                request.AddHeader("Authorization", $"Bearer {_authToken.Value}");
            }
        }

        private static string GetTableName<T>()
        {
            var table = typeof(T).GetAttribute<RestApiTable>();
            if (table == null)
            {
                throw new NoTableDefinitionException(typeof(T));
            }

            return table.Name;
        }

        private static string AddColumnSelection(string url, string selectColumns)
        {
            return string.IsNullOrWhiteSpace(selectColumns) ? url : $"{url}&$select={selectColumns}";
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class RestApiTable : Attribute
    {
        public string Name { get; set; }
    }

    public class NoTableDefinitionException : Exception
    {
        public NoTableDefinitionException(Type t) : base($"No RestApiTable attribute specified on type {t}") { }
    }
}