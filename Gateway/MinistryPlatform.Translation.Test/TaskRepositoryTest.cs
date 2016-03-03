using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Configuration;
using crds_angular.App_Start;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    public class TaskRepositoryTest
    {
        private TaskRepository _fixture;

        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _configWrapper.Setup(mocked => mocked.GetConfigIntValue("RecurringEventSetup")).Returns(92465);
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });

            _fixture = new TaskRepository(_authService.Object, _configWrapper.Object, _ministryPlatformService.Object);
        }

        [Test]
        public void ShouldGetMPTaskList()
        {
            // Arrange
            List<Dictionary<string, object>> responseValues = new List<Dictionary<string, object>>();

            var response = new Dictionary<string, object>
            {
                {"Task_ID", 1234567},
                {"Title", "Task Title"},
                {"Author_User_ID", 2345678},
                {"Assigned_User_ID", 3456789},
                {"Start_Date", new DateTime(2016, 3, 28, 8, 30, 0)},
                {"End_Date", new DateTime(2016, 3, 28, 8, 30, 0)},
                {"Completed", false},
                {"Description", "Task Description"},
                {"Domain_ID", 1},
                {"_Record_ID", 1234567},
                {"_Page_ID", 302 },
                {"_Process_Submission_ID", 3456789},
                {"_Process_Step_ID", 4567890},
                {"_Rejected", false},
                {"_Escalated", false},
                {"_Record_Description", "Task Record Description"}
            };

            responseValues.Add(response);
            _ministryPlatformService.Setup(m => m.GetPageViewRecords(92465, It.IsAny<string>(), string.Empty, string.Empty, 0)).Returns(responseValues);

            // Act
            var reminders = _fixture.GetRecurringEventTasks();

            ////Assert
            _ministryPlatformService.VerifyAll();
            Assert.AreEqual(reminders[0].Task_ID, 1234567);
            Assert.AreEqual(reminders[0].Title, "Task Title");
            Assert.AreEqual(reminders[0].Author_User_ID, 2345678);
            Assert.AreEqual(reminders[0].Assigned_User_ID, 3456789);
            Assert.AreEqual(reminders[0].StartDate, new DateTime(2016, 3, 28, 8, 30, 0));
            Assert.AreEqual(reminders[0].EndDate, new DateTime(2016, 3, 28, 8, 30, 0));
            Assert.AreEqual(reminders[0].Description, "Task Description");
            Assert.AreEqual(reminders[0].Domain_ID, 1);
            Assert.AreEqual(reminders[0].Record_ID, 1234567);
            Assert.AreEqual(reminders[0].Page_ID, 302);
            Assert.AreEqual(reminders[0].Process_Submission_ID, 3456789);
            Assert.AreEqual(reminders[0].Process_Step_ID, 4567890);
            Assert.AreEqual(reminders[0].Rejected, false);
            Assert.AreEqual(reminders[0].Escalated, false);
            Assert.AreEqual(reminders[0].Record_Description, "Task Record Description");
        }

        [Test]
        public void ShouldCompleteTask()
        {
            // Arrange
            var token = "12b3c4d5e6f7g8h";
            var taskId = 1234567;
            var rejected = false;
            var comments = "comments";

            _ministryPlatformService.Setup(m => m.CompleteTask(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()));

            // Act
            _fixture.CompleteTask(token, taskId, rejected, comments);

            // Assert
            _ministryPlatformService.VerifyAll();
        }

    }
}
