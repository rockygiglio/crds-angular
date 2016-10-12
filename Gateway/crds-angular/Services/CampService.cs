using System;
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

        public CampService(
            IContactRepository contactService,
            ICampRepository campService,
            IFormSubmissionRepository formSubmissionRepository,
            IConfigurationWrapper configurationWrapper)
        {
            _contactService = contactService;
            _campService = campService;
            _formSubmissionRepository = formSubmissionRepository;
            _configurationWrapper = configurationWrapper;
        }

        public CampDTO GetCampEventDetails(int eventId)
        {
            var campEvent = _campService.GetCampEventDetails(eventId);
            var campEventInfo = new CampDTO();
            foreach (var record in campEvent)
            {
                campEventInfo.EventId = record.EventId;
                campEventInfo.EventTitle = record.EventTitle;
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

        public void SaveCampReservation(CampReservationDTO campReservation, int eventId, string token)
        {
            var parentContact = _contactService.GetMyProfile(token);
            var displayName = campReservation.PreferredName;
            var nickName = displayName ?? campReservation.FirstName;
            displayName = displayName != null ? campReservation.LastName + ',' + campReservation.PreferredName : campReservation.LastName + ',' + campReservation.FirstName;
            
            var minorContact = new MpMinorContact
            {
                FirstName = campReservation.FirstName,
                LastName = campReservation.LastName,
                MiddleName = campReservation.MiddleName,
                BirthDate = campReservation.BirthDate,
                Gender = campReservation.Gender,
                PreferredName = displayName,
                NickName = nickName,
                SchoolAttending = campReservation.SchoolAttending,
                HouseholdId = parentContact.Household_ID,
                HouseholdPositionId = 2
            };

            var newMinorContact = _campService.CreateMinorContact(minorContact);
            var contactId = newMinorContact[0].RecordId;
            var eventParticipantId = _campService.AddAsCampParticipant(contactId, eventId);

            //form response
            var answers = new List<MpFormAnswer>
            {
                new MpFormAnswer {Response = campReservation.CurrentGrade,FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.CurrentGrade"),EventParticipantId =  eventParticipantId.Value.RecordId},
                new MpFormAnswer {Response = campReservation.SchoolAttendingNext, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.SchoolAttendingNextYear"),EventParticipantId =  eventParticipantId.Value.RecordId},
                new MpFormAnswer {Response = campReservation.RoomMate, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.PreferredRoommate"),EventParticipantId =  eventParticipantId.Value.RecordId}
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
