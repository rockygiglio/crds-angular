using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class TaskService : ITaskService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (TaskService));

        private readonly MPInterfaces.ITaskRepository _taskRepository;
        private readonly MPInterfaces.IApiUserService _apiUserService;
        private readonly IConfigurationWrapper _configWrapper;
        private readonly IUserImpersonationService _impersonationService;
        private readonly MPInterfaces.IUserService _userService;

        public TaskService(MPInterfaces.ITaskRepository taskRepository,
                           MPInterfaces.IApiUserService apiUserService,
                           IConfigurationWrapper configWrapper,
                           IUserImpersonationService impersonationService,
                           MPInterfaces.IUserService userService)
        {
            _taskRepository = taskRepository;
            _apiUserService = apiUserService;
            _configWrapper = configWrapper;
            _impersonationService = impersonationService;
            _userService = userService;
        }

        public void AutoCompleteTasks()
        {
            try
            {
                var apiUserToken = _apiUserService.GetToken();
                var tasksToComplete = _taskRepository.GetTasksToAutostart();

                foreach (var task in tasksToComplete)
                {
                    var user = _userService.GetUserByRecordId(task.Assigned_User_ID);

                    try
                    {
                        _impersonationService.WithImpersonation(apiUserToken,
                                                                user.UserEmail,
                                                                () =>
                                                                {
                                                                    _taskRepository.CompleteTask(apiUserToken, task.Task_ID, task.Rejected, "Auto Completed");
                                                                    return true;
                                                                });

                    }
                    catch (Exception ex)
                    {
                        _logger.ErrorFormat("Auto complete task failed for Task {0} Detail: {1}", task.Task_ID, ex);
                    }
                }
            }
            catch (Exception outerException)
            {
                _logger.ErrorFormat("Could not process tasks for autocomplete, Detail: {0}", outerException);
            }
        }
    }
}