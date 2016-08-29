using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Processors;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Messaging.Interfaces;
using Moq;
using NUnit.Framework;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Messaging;
using Newtonsoft.Json;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class TextCommunicationControllerTest
    {
        private TextCommunicationController _fixture;
        private Mock<IMessageFactory> _messageFactory;
        private Mock<IMessageQueueFactory> _messageQueueFactory;
        private Mock<ITextCommunicationService> _textCommunicationService;
        private Mock<IMessageQueue> _messageQueue;
 
        [SetUp]
        public void Setup()
        {
            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigValue("TextEventQueueName")).Returns("queue name");

            _textCommunicationService = new Mock<ITextCommunicationService>();
            _messageFactory = new Mock<IMessageFactory>();
            _messageQueueFactory = new Mock<IMessageQueueFactory>();
            _messageQueue = new Mock<IMessageQueue>();
            _fixture = new TextCommunicationController(_textCommunicationService.Object, configuration.Object, _messageQueueFactory.Object, _messageFactory.Object, _messageQueue.Object);
        }

        [Test]
        public void TestPostReminderIsSuccessful()
        {
            TextCommunicationDto textData = new TextCommunicationDto
            {
                TemplateId = 264567,
                MergeData = new Dictionary<string, object>(),
                ToPhoneNumber = "1234567890",
                StartDate = DateTime.Now
            };

            ScheduledJob scheduledJob = new ScheduledJob();
            scheduledJob.StartDateTime = textData.StartDate.Value;
            scheduledJob.JobType = typeof(SendTextMessageJob);
            scheduledJob.Dto = textData;

            var msg = new Mock<Message>();
            ScheduledJob spiedScheduledJob = new ScheduledJob();
            _messageFactory.Setup(mock => mock.CreateMessage(It.IsAny<ScheduledJob>(), null))
                .Callback<object, IMessageFormatter>((obj, imf) => spiedScheduledJob = (ScheduledJob) obj)
                .Returns(msg.Object);

            var spiedMsg = new Object();
            _messageQueue.Setup(mock => mock.Send(It.IsAny<Object>(), It.IsAny<MessageQueueTransactionType>()))
                .Callback<Object, MessageQueueTransactionType>(( obj, type) => spiedMsg = obj )
                .Verifiable();

            _fixture.PostReminder(textData);
            _messageFactory.Verify(mock => mock.CreateMessage(It.IsAny<ScheduledJob>(), null), Times.Once());             
            Assert.AreEqual(JsonConvert.SerializeObject(scheduledJob), JsonConvert.SerializeObject(spiedScheduledJob));
            _messageQueue.Verify();

        }
    }
}
