using System;
using System.Net;
using crds_angular.Exceptions;
using NUnit.Framework;

namespace crds_angular.test.Exceptions
{
    public class GroupParticipantRemovalExceptionTest
    {
        [Test]
        public void TestConstructWithMessage()
        {
            var fixture = new GroupParticipantRemovalException("message");
            Assert.AreEqual(fixture.Message, "message");
            Assert.AreEqual(HttpStatusCode.InternalServerError, fixture.StatusCode);
        }

        [Test]
        public void TestConstructWithMessageAndException()
        {
            var ex = new Exception();
            var fixture = new GroupParticipantRemovalException("message", ex);
            Assert.AreSame(ex, fixture.InnerException);
            Assert.AreEqual(HttpStatusCode.InternalServerError, fixture.StatusCode);
        }

        [Test]
        public void TestConstructGroupNotFoundForParticipantException()
        {
            var fixture = new GroupNotFoundForParticipantException("message");
            Assert.AreEqual(fixture.Message, "message");
            Assert.AreEqual(HttpStatusCode.NotFound, fixture.StatusCode);
        }

        [Test]
        public void TestConstructNotGroupLeaderException()
        {
            var fixture = new NotGroupLeaderException("message");
            Assert.AreEqual(fixture.Message, "message");
            Assert.AreEqual(HttpStatusCode.Forbidden, fixture.StatusCode);
        }
    }
}
