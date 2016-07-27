using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class InvitationControllerTest
    {
        private InvitationController _fixture;

        private Mock<IInvitationService> _invitationService;
        private const string AuthType = "type";
        private const string AuthToken = "token";
        private readonly string _token = string.Format("{0} {1}", AuthType, AuthToken);

        [SetUp]
        public void SetUp()
        {
            _invitationService = new Mock<IInvitationService>(MockBehavior.Strict);

            _fixture = new InvitationController(_invitationService.Object);
            _fixture.SetupAuthorization(AuthType, AuthToken);
        }

        [Test]
        public void TestCreateInvitation()
        {
            var dto = new Invitation
            {
                EmailAddress = "me@here.com",
                GroupRoleId = 11,
                InvitationType = 22,
                RecipientName = "me",
                RequestDate = DateTime.Now,
                SourceId = 33
            };

            var created = new Invitation
            {
                InvitationId = 123,
                InvitationGuid = "456"
            };

            _invitationService.Setup(mocked => mocked.ValidateInvitation(dto, _token)).Verifiable();
            _invitationService.Setup(mocked => mocked.CreateInvitation(dto)).Returns(created);

            var result = _fixture.CreateInvitation(dto);
            _invitationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Invitation>>(result);
            var okResult = (OkNegotiatedContentResult<Invitation>) result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(created, okResult.Content);
        }

        [Test]
        public void TestCreateInvitationInvalid()
        {
            var dto = new Invitation
            {
                EmailAddress = "me@here.com",
                GroupRoleId = 11,
                InvitationType = 22,
                RecipientName = "me",
                RequestDate = DateTime.Now,
                SourceId = 33
            };

            var validationException = new ValidationException("doh!");
            _invitationService.Setup(mocked => mocked.ValidateInvitation(dto, _token)).Throws(validationException).Verifiable();

            try
            {
                _fixture.CreateInvitation(dto);
                Assert.Fail("Expected exception not thrown");
            }
            catch (HttpResponseException e)
            {
                _invitationService.VerifyAll();

                var response = e.Response;
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }

        [Test]
        public void TestCreateInvitationFails()
        {
            var dto = new Invitation
            {
                EmailAddress = "me@here.com",
                GroupRoleId = 11,
                InvitationType = 22,
                RecipientName = "me",
                RequestDate = DateTime.Now,
                SourceId = 33
            };

            var exception = new Exception("doh!");
            _invitationService.Setup(mocked => mocked.ValidateInvitation(dto, _token)).Verifiable();
            _invitationService.Setup(mocked => mocked.CreateInvitation(dto)).Throws(exception);

            try
            {
                _fixture.CreateInvitation(dto);
                Assert.Fail("Expected exception not thrown");
            }
            catch (HttpResponseException e)
            {
                _invitationService.VerifyAll();

                var response = e.Response;
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }
    }
}
