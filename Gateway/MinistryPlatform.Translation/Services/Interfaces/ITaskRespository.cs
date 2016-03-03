using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinistryPlatform.Models;


namespace MinistryPlatform.Translation.Services.Interfaces
{
    interface ITaskRespository
    {
        List<MPTask> GetRecurringEventTasks();

        void UpdateTask(MPTask task);
    }
}
