using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System.Web.Http;
using FsCheck;

namespace crds_angular.test.controllers
{
    public class GoVolunteerControllerTest
    {
        private GoVolunteerController _fixture;
        private Mock<IOrganizationService> _organizationService;

        [SetUp]
        public void Setup()
        {
            _organizationService = new Mock<IOrganizationService>();
            _fixture = new GoVolunteerController(_organizationService.Object);
            _fixture.Request = new HttpRequestMessage();
            _fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void ShouldGetOrganizationByName()
        {            
            const string name = "whatever";
            var returnValue = ValidOrganization(name);
            _organizationService.Setup(m => m.GetOrganizationByName(name)).Returns(returnValue);
            var response = _fixture.GetOrganization(name);

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Organization>>(response);
            // ReSharper disable once SuspiciousTypeConversion.Global
            var r = (OkNegotiatedContentResult<Organization>) response;
            Assert.IsNotNull(r.Content);
            Assert.AreSame(returnValue, r.Content);
        }

        [Test]
        public void ShouldHandleNullOrganization()
        {          
            _organizationService.Setup(m => m.GetOrganizationByName(It.IsAny<string>())).Returns((Organization)null);            
            Prop.ForAll<string>(st =>
            {
                var response = _fixture.GetOrganization(st);
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<NotFoundResult>(response);
            }).QuickCheckThrowOnFailure();               
        }

        [Test]
        public void ShouldUsFSCheck()
        {
            Prop.ForAll<int[]>(xs => xs.Reverse().Reverse().SequenceEqual(xs)).QuickCheckThrowOnFailure();
        }

        private static Organization ValidOrganization(string name)
        {
            return new Organization()
            {
                ContactId = 12345,
                EndDate = new DateTime(),
                StartDate = new DateTime(),
                Name = name,
                OpenSignup = true,
                OrganizationId = 1234567890
            };
        }
    }
}
