﻿using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class TaskServiceTest
    {
        private Mock<ITaskRepository> _taskRepository;
        private Mock<IApiUserRepository> _apiUserService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IUserImpersonationService> _impersonationService;
        private Mock<IUserRepository> _userService;

        private TaskService _fixture;

        [SetUp]
        public void SetUp()
        {
            _taskRepository = new Mock<ITaskRepository>(MockBehavior.Strict);
            _apiUserService = new Mock<IApiUserRepository>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _impersonationService = new Mock<IUserImpersonationService>(MockBehavior.Strict);
            _userService = new Mock<IUserRepository>();

            _fixture = new TaskService(_taskRepository.Object,
                                       _apiUserService.Object,
                                       _configurationWrapper.Object,
                                       _impersonationService.Object,
                                       _userService.Object);
        }

        [Test]
        public void ShouldCompleteTask()
        {
            // Arrange
            List<MPTask> testTasks = new List<MPTask>();
            MPTask testTask = new MPTask
            {
                Task_ID = 1234567,
                Title = "Task Title",
                Author_User_ID = 2345678,
                Assigned_User_ID = 3456789,
                StartDate = new DateTime(2016, 3, 28, 8, 30, 0),
                EndDate = new DateTime(2016, 3, 28, 8, 30, 0),
                Description = "Task Description",
                Record_ID = 1234567,
                Page_ID = 302,
                Process_Submission_ID = 3456789,
                Process_Step_ID = 4567890,
                Rejected = false,
                Escalated = false,
                Record_Description = "Task Record Description"
            };

            testTasks.Add(testTask);

            MpUser user = new MpUser();
            user.CanImpersonate = true;
            user.Guid = "DAFD35F3-C8AA-4B9D-B302-AA5F0160E544";
            user.UserEmail = "test@test.com";
            user.UserId = "test@test.com";
            user.UserRecordId = 2;

            string token = "1a2b3c4d5e6f7g8h";
            int taskID = 1234567;
            bool rejected = false;
            string comments = "Auto Completed";

            _apiUserService.Setup(m => m.GetToken()).Returns("1a2b3c4d5e6f7g8h");
            _userService.Setup(m => m.GetUserByRecordId(It.IsAny<int>())).Returns(user);
            _impersonationService.Setup(m => m.WithImpersonation(token, user.UserId, It.IsAny<Func<bool>>())).Returns((string lambdaToken, string userId, Func<bool> predicate) =>
            {
                return predicate.Invoke();
            });
               
            _taskRepository.Setup(m => m.GetTasksToAutostart()).Returns(testTasks);
            _taskRepository.Setup(m => m.CompleteTask(token, taskID, rejected, comments));

            // Act
            _fixture.AutoCompleteTasks();

            // Assert
            _taskRepository.VerifyAll();
        }
    }
}
