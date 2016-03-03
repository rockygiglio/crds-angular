using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class TaskRepository : BaseService, ITaskRespository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        private readonly int _recurringEventTaskPageViewId;
        private readonly int _tasksPageId;

        public TaskRepository(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) :
            base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;

            _recurringEventTaskPageViewId = _configurationWrapper.GetConfigIntValue("RecurringEventSetup");
            _tasksPageId = _configurationWrapper.GetConfigIntValue("TasksPageId");
        }

        public List<MPTask> GetRecurringEventTasks()
        {
            var records = _ministryPlatformService.GetPageViewRecords(_recurringEventTaskPageViewId, ApiLogin());

            var tasks = records.Select(record => new MPTask
            {
                Task_ID = record.ToInt("Task_ID"),
                Title = record.ToString("Title"),
                Author_User_ID = record.ToInt("Author_User_ID"),
                Assigned_User_ID = record.ToInt("Assigned_User_ID"),
                StartDate = record.ToNullableDate("Start_Date"),
                EndDate = record.ToNullableDate("End_Date"),
                Completed = record.ToBool("Completed"),
                Description = record.ToString("Description"),
                Domain_ID = record.ToInt("Domain_ID"),
                Record_ID = record.ToNullableInt("_Record_ID"),
                Page_ID = record.ToNullableInt("_Page_ID"),
                Process_Submission_ID = record.ToNullableInt("_Process_Submission_ID"),
                Process_Step_ID = record.ToNullableInt("_Process_Step_ID"),
                Rejected = record.ToBool("_Rejected"),
                Escalated = record.ToBool("_Escalated"),
                Record_Description = record.ToString("_Record_Description")
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
