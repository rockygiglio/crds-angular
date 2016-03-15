using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinistryPlatform.Models;


namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ITaskRepository
    {
        List<MPTask> GetTasksToAutostart();

        void CompleteTask(string token, int taskId, bool rejected, string comments);
    }
}
