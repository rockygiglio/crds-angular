using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;
using FsCheck;

namespace crds_angular.test.controllers
{
    public class GoVolunteerControllerTest
    {
        private GoVolunteerController _fixture;
        private Mock<IOrganizationService> _organizationService;
        private Mock<IGroupConnectorService> _groupConnectorService;

        [SetUp]
        public void Setup()
        {
            _organizationService = new Mock<IOrganizationService>();
            _fixture = new GoVolunteerController(_organizationService.Object, _groupConnectorService.Object)
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext()
            };
        }

        [Test]
        public void ShouldGetOrganizationByName()
        {            
    
            Prop.ForAll<int, int, string>((contactId, orgId, name) =>
            {
                var returnValue = ValidOrganization(contactId, orgId, name);
                _organizationService.Setup(m => m.GetOrganizationByName(name)).Returns(returnValue);
                var response = _fixture.GetOrganization(name);
                Assert.IsNotNull(response);
                Assert.IsInstanceOf<OkNegotiatedContentResult<Organization>>(response);
                // ReSharper disable once SuspiciousTypeConversion.Global
                var r = (OkNegotiatedContentResult<Organization>)response;
                Assert.IsNotNull(r.Content);
                Assert.AreSame(returnValue, r.Content);
            }).VerboseCheckThrowOnFailure();

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



        private static Organization ValidOrganization(int contactId, int orgId, string name)
        {
            return new Organization()
            {
                ContactId = contactId,
                EndDate = new DateTime(),
                StartDate = new DateTime(),
                Name = name,
                OpenSignup = true,
                OrganizationId = orgId
            };
        }
    }
}
