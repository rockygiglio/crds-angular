using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    class CampService : ICampService
    {
        private readonly IContactRepository _contactService;
        private readonly ICampRepository _campService;
        private readonly IEventRepository _eventService;
        private readonly IEventService _crdsEventService;

        public CampService(
            IContactRepository contactService,
            ICampRepository campService,
            IEventRepository eventService,
            IEventService crdsEventService)
        {
            _contactService = contactService;
            _campService = campService;
            _eventService = eventService;
            _crdsEventService = crdsEventService;
        }

        public CampDTO GetCampEventDetails(int contactId, int eventId)
        {
            var campEvent = _campService.GetCampEventDetails(eventId);
            var campEventInfo = new CampDTO();
            foreach (var record in campEvent)
            {
                campEventInfo.EventId = record.EventId;
                campEventInfo.EventType = record.EventType;
                campEventInfo.StartDate = record.StartDate;
                campEventInfo.EndDate = record.EndDate;
                campEventInfo.OnlineProductId = record.OnlineProductId;
                campEventInfo.RegistrationEndDate = record.RegistrationEndDate;
                campEventInfo.RegistrationStartDate = record.RegistrationStartDate;
                campEventInfo.ProgramId = record.ProgramId;
            }

            return campEventInfo;
        }
    }
}
