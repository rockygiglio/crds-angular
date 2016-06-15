using System;
using MinistryPlatform.Translation.Models.Attributes;
using MinistryPlatform.Translation.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace MinistryPlatform.Translation.Test.Services
{
    [Category("IntegrationTests")]
    public class MinistryPlatformRestServiceIntegrationTest
    {
        private MinistryPlatformRestService _fixture;

        private string _authToken;

        [TestFixtureSetUp]
        public void SetupAll()
        {
            var auth = AuthenticationRepository.authenticate(Environment.GetEnvironmentVariable("API_USER"), Environment.GetEnvironmentVariable("API_PASSWORD"));
            _authToken = auth["token"].ToString();
        }

        [SetUp]
        public void SetUp()
        {
            var restClient = new RestClient(Environment.GetEnvironmentVariable("MP_REST_API_ENDPOINT"));
            _fixture = new MinistryPlatformRestService(restClient);
        }

        [Test]
        public void TestSearchAllPaymentTypes()
        {
            Console.WriteLine("TestSearchAllPaymentTypes");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MyPaymentType>();

            foreach (var p in results)
            {
                Console.WriteLine("Payment_Type\t{0}", p);
            }
        }

        [Test]
        public void TestSearchPaymentTypes()
        {
            Console.WriteLine("TestSearchPaymentTypes");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MyPaymentType>("Payment_Type_Id > 5");

            foreach (var p in results)
            {
                Console.WriteLine("Payment_Type\t{0}", p);
            }
        }

        [Test]
        public void TestSearchPaymentTypesSelectColumns()
        {
            Console.WriteLine("TestSearchPaymentTypesSelectColumns");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MyPaymentType>("Payment_Type_Id > 5", "Payment_Type_Id,Payment_Type");

            foreach (var p in results)
            {
                Console.WriteLine("Payment_Type\t{0}", p);
            }
        }

        [Test]
        public void TestGetPaymentType()
        {
            Console.WriteLine("TestGetPaymentType");
            var p = _fixture.UsingAuthenticationToken(_authToken).Get<MyPaymentType>(2);
            Console.WriteLine("Payment_Type\t{0}", p);
        }
    }

    [MpRestApiTable(Name = "Payment_Types")]
    public class MyPaymentType
    {
        [JsonProperty(PropertyName = "Payment_Type_ID")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "Payment_Type")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Payment_Type_Code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "__ExternalPaymentTypeID", NullValueHandling = NullValueHandling.Ignore)]
        public int? LegacyId { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Description: {2}, Code: {3}, LegacyId: {4}", Id, Name, Description, Code, LegacyId);
        }
    }
}
