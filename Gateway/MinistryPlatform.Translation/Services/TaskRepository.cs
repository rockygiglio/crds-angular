using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class TaskRepository : ITaskRespository
    {
        public List<MPTask> GetRecurringEventTasks()
        {
            return null;
        }

        public void UpdateTask(MPTask task)
        {
            
        }
    }
}
