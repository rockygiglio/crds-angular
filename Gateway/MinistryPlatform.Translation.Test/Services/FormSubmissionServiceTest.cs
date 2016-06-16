using System;
using System.Collections.Generic;
using System.Data;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class FormSubmissionServiceTest
    {
        private FormSubmissionRepository _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationRepository> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<IDbConnection> _dbConnection;
        private MpFormResponse _mockForm;
        private MpFormAnswer _mockAnswer1, _mockAnswer2, _mockAnswer3;
        private const int formResponsePageId = 424;
        private const int formAnswerPageId = 425;
        private const int responseId = 2;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationRepository>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _dbConnection = new Mock<IDbConnection>();

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });

            _fixture = new FormSubmissionRepository(_ministryPlatformService.Object, _dbConnection.Object, _authService.Object, _configWrapper.Object);

            _mockAnswer1 = new MpFormAnswer
            {
                FieldId = 375,
                FormResponseId = responseId,
                OpportunityResponseId = 7329,
                Response = "Test Last Name"
            };

            _mockAnswer2 = new MpFormAnswer
            {
                FieldId = 376,
                FormResponseId = responseId,
                OpportunityResponseId = 7329,
                Response = "Test First Name"
            };

            _mockAnswer3 = new MpFormAnswer
            {
                FieldId = 377,
                FormResponseId = responseId,
                OpportunityResponseId = 7329,
                Response = "Test Middle Initial"
            };

            _mockForm = new MpFormResponse
            {
                FormId = 17,
                ContactId = 2389887,
                OpportunityId = 313,
                OpportunityResponseId = 7329,
                FormAnswers = new List<MpFormAnswer>
                {
                    _mockAnswer1,
                    _mockAnswer2,
                    _mockAnswer3
                }
            };
        }

        [Test]
        public void SubmitFormResponse()
        {
            var expectedResponseDict = new Dictionary<string, object>
            {
                {"Form_ID", _mockForm.FormId},
                {"Response_Date", DateTime.Today},
                {"Contact_ID", _mockForm.ContactId},
                {"Opportunity_ID", _mockForm.OpportunityId},
                {"Opportunity_Response", _mockForm.OpportunityResponseId}, 
                {"Pledge_Campaign_ID", null}
            };

            var expectedAnswerDict1 = new Dictionary<string, object>
            {
                {"Form_Response_ID", _mockAnswer1.FormResponseId},
                {"Form_Field_ID", _mockAnswer1.FieldId},
                {"Response", _mockAnswer1.Response},
                {"Opportunity_Response", _mockAnswer1.OpportunityResponseId}
            };

            var expectedAnswerDict2 = new Dictionary<string, object>
            {
                {"Form_Response_ID", _mockAnswer2.FormResponseId},
                {"Form_Field_ID", _mockAnswer2.FieldId},
                {"Response", _mockAnswer2.Response},
                {"Opportunity_Response", _mockAnswer2.OpportunityResponseId}
            };

            var expectedAnswerDict3 = new Dictionary<string, object>
            {
                {"Form_Response_ID", _mockAnswer3.FormResponseId},
                {"Form_Field_ID", _mockAnswer3.FieldId},
                {"Response", _mockAnswer3.Response},
                {"Opportunity_Response", _mockAnswer3.OpportunityResponseId}
            };

            _ministryPlatformService.Setup(m => m.CreateRecord(formResponsePageId, expectedResponseDict, It.IsAny<string>(), true)).Returns(responseId);
            _ministryPlatformService.Setup(m => m.CreateRecord(formAnswerPageId, expectedAnswerDict1, It.IsAny<string>(), true));
            _ministryPlatformService.Setup(m => m.CreateRecord(formAnswerPageId, expectedAnswerDict2, It.IsAny<string>(), true));
            _ministryPlatformService.Setup(m => m.CreateRecord(formAnswerPageId, expectedAnswerDict3, It.IsAny<string>(), true));

            var result = _fixture.SubmitFormResponse(_mockForm);

            Assert.AreEqual(responseId, result);
            _ministryPlatformService.VerifyAll();
        }
    }
}
