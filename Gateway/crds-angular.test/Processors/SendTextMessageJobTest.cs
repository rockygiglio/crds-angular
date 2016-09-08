using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads;
using crds_angular.Processors;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using Quartz;
using Quartz.Impl;

namespace crds_angular.test.Processors
{
    [TestFixture]
    public class SendTextMessageJobTest
    {
        private SendTextMessageJob _fixture;
        private Mock<ITextCommunicationService> _textCommunicationService;
        private Mock<ICommunicationRepository> _communicationRepository;
        private Mock<IJobExecutionContext> _mockJobExecutionContext;

        [SetUp]
        public void Setup()
        {
            _textCommunicationService = new Mock<ITextCommunicationService>();
            _communicationRepository = new Mock<ICommunicationRepository>();
            _mockJobExecutionContext = new Mock<IJobExecutionContext>();


            _fixture = new SendTextMessageJob(_textCommunicationService.Object,_communicationRepository.Object);
        }

        [Test]
        public void TestExecute()
        {
            IJobDetail jobDetail = new JobDetailImpl("jobsettings", typeof(SendTextMessageJob));
            _mockJobExecutionContext.Setup(mock => mock.JobDetail).Returns(jobDetail);

            MpMessageTemplate mockMessageTemplate = new MpMessageTemplate()
            {
                Body = "<div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">Hi there!<br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\"><br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">Welcome to the [Event_Date] [EVent_Start_Time] Crossroads service. </div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\"><br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">Click here to join us:  https://[BaseUrl]/live/stream</div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\"><br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">We\'re glad you\'re here,</div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">-Crossroads</div></div>"
            };

            string mockParsedTemplate =
                "<div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">Hi there!<br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\"><br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">Welcome to the word up Crossroads service. </div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\"><br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">Click here to join us:  https://localhost/live/stream</div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\"><br /></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">We\'re glad you\'re here,</div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 12.8px; background-color: rgb(255, 255, 255);\">-Crossroads</div></div>";

            TextCommunicationDto dto = new TextCommunicationDto()
            {
                MergeData = new Dictionary<string, object>(),
                StartDate = DateTime.Now,
                TemplateId = 10,
                ToPhoneNumber = "+12345678910"
            };
            dto.MergeData.Add("Event_Date", "datey mcdateface");
            dto.MergeData.Add("Event_Start_Time", "starty mctimeface");

            IDictionary<string, object> keyValuePairs = new Dictionary<string, object>()
            {
                { "TemplateId", dto.TemplateId },
                { "MergeData", dto.MergeData },
                { "StartDate", dto.StartDate },
                { "ToPhoneNumber", dto.ToPhoneNumber }
            };

            JobDataMap jobDataMap = new JobDataMap(keyValuePairs);

            _mockJobExecutionContext.SetupGet(p => p.JobDetail.JobDataMap).Returns(jobDataMap);

            _communicationRepository.Setup(mock => mock.GetTemplate(dto.TemplateId)).Returns(mockMessageTemplate);
            _communicationRepository.Setup(mock => mock.ParseTemplateBody(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(mockParsedTemplate);

            var expectedBody = HtmlHelper.StripHTML(mockParsedTemplate);

            var spiedBody = "";
            var spiedPhone = "";
            _textCommunicationService.Setup(mock => mock.SendTextMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((phone, body) =>
                {
                    spiedBody = body;
                    spiedPhone = phone;
                });
            _fixture.Execute(_mockJobExecutionContext.Object);
            Assert.AreEqual(dto.ToPhoneNumber,spiedPhone);
            Assert.AreEqual(expectedBody, spiedBody);

        }
    }
}
