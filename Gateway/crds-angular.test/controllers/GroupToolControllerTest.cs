using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class GroupToolControllerTest
    {
        private GroupToolController _fixture;

        private Mock<IGroupToolService> _groupToolService;
        private const string AuthType = "abc";
        private const string AuthToken = "123";
        private readonly string _auth = string.Format("{0} {1}", AuthType, AuthToken);

        [SetUp]
        public void SetUp()
        {
            _groupToolService = new Mock<IGroupToolService>(MockBehavior.Strict);
            _fixture = new GroupToolController(_groupToolService.Object);
            _fixture.SetupAuthorization(AuthType, AuthToken);
        }

        [Test]
        public void TestRemoveParticipantFromMyGroup()
        {
            _groupToolService.Setup(mocked => mocked.RemoveParticipantFromMyGroup(_auth, 1, 2, 3, "test"));
            var result = _fixture.RemoveParticipantFromMyGroup(1, 2, 3, "test");
            _groupToolService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void TestRemoveParticipantFromMyGroupWithGroupParticipantRemovalException()
        {
            var ex = new GroupParticipantRemovalException("message");
            _groupToolService.Setup(mocked => mocked.RemoveParticipantFromMyGroup(_auth, 1, 2, 3, "test")).Throws(ex);
            try
            {
                _fixture.RemoveParticipantFromMyGroup(1, 2, 3, "test");
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(ex.StatusCode, e.Response.StatusCode);
            }

            _groupToolService.VerifyAll();
        }

        [Test]
        public void TestRemoveParticipantFromMyGroupWithOtherException()
        {
            var ex = new Exception();
            _groupToolService.Setup(mocked => mocked.RemoveParticipantFromMyGroup(_auth, 1, 2, 3, "test")).Throws(ex);
            try
            {
                _fixture.RemoveParticipantFromMyGroup(1, 2, 3, "test");
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.Response.StatusCode);
            }

            _groupToolService.VerifyAll();
        }

        [Test]
        public void TestApproveDenyInquiryFromMyGroup()
        {
            const int groupTypeId = 1;
            const int groupId = 2;
            const bool approve = true;
            var inquiry = new Inquiry();
            const string message = "message";

            _groupToolService.Setup(mocked => mocked.ApproveDenyInquiryFromMyGroup(_auth, groupTypeId, groupId, approve, inquiry, message)).Verifiable();

            var result = _fixture.ApproveDenyInquiryFromMyGroup(groupTypeId, groupId, approve, inquiry, message);
            _groupToolService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void TestApproveDenyInquiryFromMyGroupWithGroupParticipantRemovalException()
        {
            var ex = new GroupParticipantRemovalException("message");
            const int groupTypeId = 1;
            const int groupId = 2;
            const bool approve = true;
            var inquiry = new Inquiry();
            const string message = "message";

            _groupToolService.Setup(mocked => mocked.ApproveDenyInquiryFromMyGroup(_auth, groupTypeId, groupId, approve, inquiry, message)).Throws(ex);
            try
            {
                _fixture.ApproveDenyInquiryFromMyGroup(groupTypeId, groupId, approve, inquiry, message);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(ex.StatusCode, e.Response.StatusCode);
            }

            _groupToolService.VerifyAll();
        }

        [Test]
        public void TestApproveDenyInquiryFromMyGroupWithOtherException()
        {
            var ex = new Exception();
            const int groupTypeId = 1;
            const int groupId = 2;
            const bool approve = true;
            var inquiry = new Inquiry();
            const string message = "message";

            _groupToolService.Setup(mocked => mocked.ApproveDenyInquiryFromMyGroup(_auth, groupTypeId, groupId, approve, inquiry, message)).Throws(ex);
            try
            {
                _fixture.ApproveDenyInquiryFromMyGroup(groupTypeId, groupId, approve, inquiry, message);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.Response.StatusCode);
            }

            _groupToolService.VerifyAll();
        }

    }
}
