using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ParticipantControllerTest
    {
        private ParticipantController _fixture;
        private Mock<IGroupService> _groupService;
        private const string AuthType = "Bearer";
        private const string AuthToken = "tok123";


        [SetUp]
        public void SetUp()
        {
            _groupService = new Mock<IGroupService>(MockBehavior.Strict);
            _fixture = new ParticipantController(_groupService.Object);

            _fixture.SetupAuthorization(AuthType, AuthToken);
        }

        [Test]
        public void TestGetParticipant()
        {
            var participant = new MinistryPlatform.Translation.Models.Participant();
            _groupService.Setup(mocked => mocked.GetParticipantRecord(string.Format("{0} {1}", AuthType, AuthToken))).Returns(participant);

            var result = _fixture.GetParticipant();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<MinistryPlatform.Translation.Models.Participant>>(result);
            var okResult = (OkNegotiatedContentResult<MinistryPlatform.Translation.Models.Participant>) result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(participant, okResult.Content);
        }
    }
}
