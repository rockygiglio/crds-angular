using System.Collections.Generic;
using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICampRepository
    {
        List<MpCampEvent> GetCampEventDetails(int eventId);
       
    }
}
