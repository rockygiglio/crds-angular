using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICampRepository
    {
        List<MpCampEvent> GetCampEventDetails(int eventId);
        List<MpRecordID> CreateMinorContact(MpMinorContact minorContact);
        Result<MpRecordID> AddAsCampParticipant(int contactId, int eventId);
    }
}
