using System.Collections.Generic;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class CampService : ICampService
    {
        private readonly IContactRepository _contactService;
        private readonly ICampRepository _campService;
        private readonly IFormSubmissionRepository _formSubmissionRepository;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IParticipantRepository _participantRepository;
        private readonly IEventRepository _eventRepository;

        public CampService(
            IContactRepository contactService,
            ICampRepository campService,
            IFormSubmissionRepository formSubmissionRepository,
            IConfigurationWrapper configurationWrapper,
            IParticipantRepository partcipantRepository,
            IEventRepository eventRepository)
        {
            _contactService = contactService;
            _campService = campService;
            _formSubmissionRepository = formSubmissionRepository;
            _configurationWrapper = configurationWrapper;
            _participantRepository = partcipantRepository;
            _eventRepository = eventRepository;
        }

        public CampDTO GetCampEventDetails(int eventId)
        {
            var campEvent = _campService.GetCampEventDetails(eventId);
            var campEventInfo = new CampDTO();

            campEventInfo.EventId = campEvent.EventId;
            campEventInfo.EventTitle = campEvent.EventTitle;
            campEventInfo.EventType = campEvent.EventType;
            campEventInfo.StartDate = campEvent.StartDate;
            campEventInfo.EndDate = campEvent.EndDate;
            campEventInfo.OnlineProductId = campEvent.OnlineProductId;
            campEventInfo.RegistrationEndDate = campEvent.RegistrationEndDate;
            campEventInfo.RegistrationStartDate = campEvent.RegistrationStartDate;
            campEventInfo.ProgramId = campEvent.ProgramId;

            return campEventInfo;
        }

        public void SaveCampReservation(CampReservationDTO campReservation, int eventId, string token)
        {
            var parentContact = _contactService.GetMyProfile(token);
            var displayName = campReservation.PreferredName;
            var nickName = displayName ?? campReservation.FirstName;
            displayName = displayName != null ? campReservation.LastName + ',' + campReservation.PreferredName : campReservation.LastName + ',' + campReservation.FirstName;
            
            var minorContact = new MpContact
            {
                FirstName = campReservation.FirstName,
                LastName = campReservation.LastName,
                MiddleName = campReservation.MiddleName,
                BirthDate = campReservation.BirthDate,
                Gender = campReservation.Gender,
                PreferredName = displayName,
                Nickname = nickName,
                SchoolAttending = campReservation.SchoolAttending,
                HouseholdId = parentContact.Household_ID,
                HouseholdPositionId = 2
            };

            var newMinorContact = _contactService.CreateContact(minorContact);
            var contactId = newMinorContact[0].RecordId;
            var participant = _participantRepository.GetParticipant(contactId);
            var participantId = participant.ParticipantId;
            var eventParticipantId = _eventRepository.RegisterParticipantForEvent(participantId, eventId);
           
            //form response
            var answers = new List<MpFormAnswer>
            {
                new MpFormAnswer {Response = campReservation.CurrentGrade,FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.CurrentGrade"),EventParticipantId =  eventParticipantId},
                new MpFormAnswer {Response = campReservation.SchoolAttendingNext, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear"),EventParticipantId =  eventParticipantId},
                new MpFormAnswer {Response = campReservation.RoomMate, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.PreferredRoommate"),EventParticipantId =  eventParticipantId}
            };

            var formId = _configurationWrapper.GetConfigIntValue("SummerCampFormID");
            var formResponse = new MpFormResponse
            {
                ContactId = contactId,
                FormId = formId,
                FormAnswers = answers
            };
            
            _formSubmissionRepository.SubmitFormResponse(formResponse);
        }
    }
}
