using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Connect;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IConnectRepository
    {
        ConnectPinDto GetPinDetails(int participantId);
    }
}
