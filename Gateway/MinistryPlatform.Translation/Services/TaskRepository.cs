using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class TaskRepository : BaseService, ITaskRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _autoStartedTaskPageViewId;

        public TaskRepository(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) :
            base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;

            _autoStartedTaskPageViewId = _configurationWrapper.GetConfigIntValue("TasksNeedingAutoStarted");
        }

        public List<MPTask> GetTasksToAutostart()
        {
            var records = _ministryPlatformService.GetPageViewRecords(_autoStartedTaskPageViewId, ApiLogin());

            var tasks = records.Select(record => new MPTask
            {
                Task_ID = record.ToInt("Task ID"),
                Title = record.ToString("Title"),
                Author_User_ID = record.ToInt("Author User ID"),
                Assigned_User_ID = record.ToInt("Assigned User ID"),
                StartDate = record.ToNullableDate("Start Date"),
                EndDate = record.ToNullableDate("End Date"),
                Completed = record.ToBool("Completed"),
                Description = record.ToString("Description"),
                Record_ID = record.ToNullableInt("Record ID"),
                Page_ID = record.ToNullableInt("Page ID"),
                Process_Submission_ID = record.ToNullableInt("Process Submission ID"),
                Process_Step_ID = record.ToNullableInt("Process Step ID"),
                Rejected = record.ToBool("Rejected"),
                Escalated = record.ToBool("Escalated"),
                Record_Description = record.ToString("Record Description")
            }).ToList();

            return tasks;
        }

        public void CompleteTask(string token, int taskId, bool rejected, string comments)
        {
            // note that this call needs to user impersonation
            _ministryPlatformService.CompleteTask(token, taskId, rejected, comments);
        }
    }
}
