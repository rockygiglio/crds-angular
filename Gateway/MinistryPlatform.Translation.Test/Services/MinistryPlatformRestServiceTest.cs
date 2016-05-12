using System.Net;
using MinistryPlatform.Translation.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using MinistryPlatform.Models.Attributes;

namespace MinistryPlatform.Translation.Test.Services
{
    public class MinistryPlatformRestServiceTest
    {
        private MinistryPlatformRestService _fixture;

        private Mock<IRestClient> _restClient;

        [SetUp]
        public void SetUp()
        {
            _restClient = new Mock<IRestClient>();
            _fixture = new MinistryPlatformRestService(_restClient.Object);
        }

        [Test]
        public void TestSearchAllRecords()
        {
            var models = new List<TestModelWithRestApiTable>
            {
                new TestModelWithRestApiTable
                {
                    Id = 1,
                    Name = "name 1"
                },
                new TestModelWithRestApiTable
                {
                    Id = 2,
                    Name = "name 2"
                },
            };

            var restResponse = new Mock<IRestResponse>(MockBehavior.Strict);
            restResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            restResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            restResponse.SetupGet(mocked => mocked.Content).Returns(JsonConvert.SerializeObject());

            _restClient.Setup(mocked => mocked.Execute(It.IsAny<IRestRequest>())).Returns(restResponse.Object);

            var results = _fixture.Search<TestModelWithRestApiTable>();
        }

    }

    [RestApiTable(Name = "MP_Table_Name")]
    internal class TestModelWithRestApiTable
    {
        [JsonProperty(PropertyName = "ID")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }

    internal class TestModelWithoutRestApiTable
    {
        
    }
}
