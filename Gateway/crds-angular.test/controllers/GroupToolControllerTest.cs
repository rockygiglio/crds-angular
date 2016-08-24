using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Models.Json;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class GroupToolControllerTest
    {
        private GroupToolController _fixture;

        private Mock<IGroupToolService> _groupToolService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private const string AuthType = "abc";
        private const string AuthToken = "123";
        private readonly string _auth = string.Format("{0} {1}", AuthType, AuthToken);

        [SetUp]
        public void SetUp()
        {
            _groupToolService = new Mock<IGroupToolService>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("GroupTypeSmallId")).Returns(1);
            _fixture = new GroupToolController(_groupToolService.Object, _configurationWrapper.Object);
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
            inquiry.Message = "message";

            _groupToolService.Setup(mocked => mocked.ApproveDenyInquiryFromMyGroup(_auth, groupTypeId, groupId, approve, inquiry, inquiry.Message)).Verifiable();
            
            var result = _fixture.ApproveDenyInquiryFromMyGroup(groupTypeId, groupId, approve, inquiry);
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
            inquiry.Message = "message";

            _groupToolService.Setup(mocked => mocked.ApproveDenyInquiryFromMyGroup(_auth, groupTypeId, groupId, approve, inquiry, inquiry.Message)).Throws(ex);
            try
            {
                _fixture.ApproveDenyInquiryFromMyGroup(groupTypeId, groupId, approve, inquiry);
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
            inquiry.Message = "message";

            _groupToolService.Setup(mocked => mocked.ApproveDenyInquiryFromMyGroup(_auth, groupTypeId, groupId, approve, inquiry, inquiry.Message)).Throws(ex);
            try
            {
                _fixture.ApproveDenyInquiryFromMyGroup(groupTypeId, groupId, approve, inquiry);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.Response.StatusCode);
            }

            _groupToolService.VerifyAll();
        }

	    [Test]
        public void TestPostGroupMessage()
        {
            _groupToolService.Setup(mocked => mocked.SendAllGroupParticipantsEmail(_auth, 123, 1, "subject", "message"));
            var result = _fixture.PostGroupMessage(123, 1, new GroupMessageDTO
            {
                Subject = "subject",
                Body = "message"
            });
            _groupToolService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void TestSearchGroupsNoGroupsFound()
        {
            const int groupTypeId = 123;
            const string keywords = "kw1,kw2";
            const string location = "123 main st";

            _groupToolService.Setup(mocked => mocked.SearchGroups(groupTypeId, keywords, location)).Returns(new List<GroupDTO>());
            var result = _fixture.SearchGroups(groupTypeId, keywords, location);
            _groupToolService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RestHttpActionResult<List<GroupDTO>>>(result);
            var restResult = (RestHttpActionResult<List<GroupDTO>>)result;
            Assert.AreEqual(HttpStatusCode.NotFound, restResult.StatusCode);
        }

        [Test]
        [ExpectedException(typeof(HttpResponseException))]
        public void TestSearchGroupsWithException()
        {
            const int groupTypeId = 123;
            const string keywords = "kw1,kw2";
            const string location = "123 main st";
            var exception = new Exception("whoa nelly");

            _groupToolService.Setup(mocked => mocked.SearchGroups(groupTypeId, keywords, location)).Throws(exception);
            _fixture.SearchGroups(groupTypeId, keywords, location);
        }

        [Test]
        public void TestSearchGroups()
        {
            const int groupTypeId = 123;
            const string keywords = "kw1,kw2";
            const string location = "123 main st";
            var searchResults = new List<GroupDTO>
            {
                new GroupDTO(),
                new GroupDTO()
            };

            _groupToolService.Setup(mocked => mocked.SearchGroups(groupTypeId, keywords, location)).Returns(searchResults);
            var result = _fixture.SearchGroups(groupTypeId, keywords, location);
            _groupToolService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<GroupDTO>>>(result);
            var restResult = (OkNegotiatedContentResult<List<GroupDTO>>)result;
            Assert.AreSame(searchResults, restResult.Content);
        }

        [Test]
        public void ShouldEndGroupSuccessfully()
        {
            var groupId = 9876;
            var groupReasonEndedId = 1;
            string token = "abc 123";
         
            _groupToolService.Setup(mocked => mocked.EndGroup(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            _groupToolService.Setup(mocked => mocked.VerifyCurrentUserIsGroupLeader(token, groupId)).Returns(new MyGroup());

            IHttpActionResult result = _fixture.EndSmallGroup(groupId, groupReasonEndedId);

            _groupToolService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);

        }

        [Test]
        public void ShouldNotEndGroup()
        {
            var groupId = 9876;
            var groupReasonEndedId = 1;
            string token = "1234frd32";
            Exception ex = new Exception();

            _groupToolService.Setup(mocked => mocked.EndGroup(It.IsAny<int>(), It.IsAny<int>())).Throws(ex);
            _groupToolService.Setup(mocked => mocked.VerifyCurrentUserIsGroupLeader(It.IsAny<string>(), It.IsAny<int>())).Returns(new MyGroup());
            IHttpActionResult result = _fixture.EndSmallGroup(groupId, groupReasonEndedId);

            _groupToolService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public void ShouldNotEndGroupNotALeader()
        {
            var groupId = 1234;
            var groupReasonEndedId = 1;
            string token = "abc 123";

            _groupToolService.Setup(mocked => mocked.VerifyCurrentUserIsGroupLeader(It.IsAny<string>(), It.IsAny<int>())).Throws(new NotGroupLeaderException("User is not a leader"));

            IHttpActionResult result = _fixture.EndSmallGroup(groupId, groupReasonEndedId);

            _groupToolService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
    }
}
