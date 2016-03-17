using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Models.Crossroads.Lookups;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    class GatewayLookupControllerTest
    {
        private GatewayLookupController _fixture;
        private Mock<IGatewayLookupService> _gatewayLookupService;

        [SetUp]
        public void SetUp()
        {
            _gatewayLookupService = new Mock<IGatewayLookupService>();
            _fixture = new GatewayLookupController(_gatewayLookupService.Object)
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext()
            };
        }

        [Test]
        public void ShouldGetListOfOtherOrganizations()
        {
            var orgs = otherOrganizations();
            _gatewayLookupService.Setup(m => m.GetOtherOrgs(null)).Returns(orgs);


            var response = _fixture.GetOtherOrganizations();
           
            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<OtherOrganization>>>(response);
            // ReSharper disable once SuspiciousTypeConversion.Global
            var r = (OkNegotiatedContentResult<List<OtherOrganization>>)response;
            Assert.IsNotNull(r.Content);
            Assert.AreSame(orgs, r.Content);
        }
        [Test]
        [ExpectedException(typeof(HttpResponseException))]
        public void ShouldThrowAnException()
        {
            _gatewayLookupService.Setup(m => m.GetOtherOrgs(null)).Throws(new Exception());
            _fixture.GetOtherOrganizations();
        }

        private List<OtherOrganization> otherOrganizations()
        {
            return new List<OtherOrganization>()
            {
              new OtherOrganization(12,"sdfrtrtg"),
              new OtherOrganization(15,"dghjhnjmh"),
              new OtherOrganization(13, "gfhnnhmjm")
            };
        }
             
    }


}
