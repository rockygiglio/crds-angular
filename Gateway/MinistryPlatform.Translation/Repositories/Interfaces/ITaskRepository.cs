using System.Collections.Generic;
using MinistryPlatform.Translation.Models;


namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        List<MPTask> GetTasksToAutostart();

        void CompleteTask(string token, int taskId, bool rejected, string comments);
    }
}
