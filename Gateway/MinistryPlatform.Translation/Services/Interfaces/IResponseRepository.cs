using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IResponseRepository
    {
        List<MPServeReminder> GetServeReminders(string token);
    }
}
