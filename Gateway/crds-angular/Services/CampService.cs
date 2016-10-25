using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IEventParticipantRepository _eventParticipantRepository;

        public CampService(
            IContactRepository contactService,
            ICampRepository campService,
            IFormSubmissionRepository formSubmissionRepository,
            IConfigurationWrapper configurationWrapper,
            IParticipantRepository partcipantRepository,
            IEventRepository eventRepository,
            IApiUserRepository apiUserRepository,
            IGroupRepository groupRepository,
            IEventParticipantRepository eventParticipantRepository)
        {
            _contactService = contactService;
            _campService = campService;
            _formSubmissionRepository = formSubmissionRepository;
            _configurationWrapper = configurationWrapper;
            _participantRepository = partcipantRepository;
            _eventRepository = eventRepository;
            _apiUserRepository = apiUserRepository;
            _groupRepository = groupRepository;
            _eventParticipantRepository = eventParticipantRepository;
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

        public List<CampFamilyMember> GetEligibleFamilyMembers(int eventId, string token)
        {
            var myContact = _contactService.GetMyProfile(token);
            var family = _contactService.GetHouseholdFamilyMembers(myContact.Household_ID);
            var otherFamily = _contactService.GetOtherHouseholdMembers(myContact.Contact_ID);
            family.AddRange(otherFamily);

            var apiToken = _apiUserRepository.GetToken(); 
                
            family = family.Where((member) => member.HouseholdPosition == "Minor Child").ToList();                
            return family.Select(member => new CampFamilyMember()
            {
                ContactId = member.ContactId,
                IsEligible = _groupRepository.IsMemberOfEventGroup(member.ContactId, eventId, apiToken),
                SignedUpDate = _eventParticipantRepository.EventParticipantSignupDate(member.ContactId, eventId, apiToken),
                LastName = member.LastName,
                PreferredName = member.Nickname ?? member.FirstName
            }).ToList();                       
        }

        public void SaveCamperEmergencyContactInfo(CampReservationDTO campReservation, int eventId, int contactId)
        {
            var participant = _participantRepository.GetParticipant(contactId);
            var eventParticipantId = _eventRepository.RegisterParticipantForEvent(participant.ParticipantId, eventId);

            var answers = new List<MpFormAnswer>
            {
                new MpFormAnswer {Response = campReservation.EmergencyContactFirstName,FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.EmergenyContactFirstName"),EventParticipantId =  eventParticipantId},
                new MpFormAnswer {Response = campReservation.EmergencyContactLastName, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.EmergencyContactLastName"),EventParticipantId =  eventParticipantId},
                new MpFormAnswer {Response = campReservation.EmergecyContactMobilePhone, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.EmergencyContactMobilePhone"),EventParticipantId =  eventParticipantId},
                new MpFormAnswer {Response = campReservation.EmergencyContactEmail, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.EmergencyContactEmail"),EventParticipantId =  eventParticipantId},
                new MpFormAnswer {Response = campReservation.EmergencyContactRelationship, FieldId = _configurationWrapper.GetConfigIntValue("SummerCampForm.EmergencyContactRelationship"),EventParticipantId =  eventParticipantId}
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

        public void SaveCampReservation(CampReservationDTO campReservation, int eventId, string token)
        {
            var parentContact = _contactService.GetMyProfile(token);
            var displayName = campReservation.PreferredName;
            var nickName = displayName ?? campReservation.FirstName;
            displayName = displayName != null ? campReservation.LastName + ", " + campReservation.PreferredName : campReservation.LastName + ", " + campReservation.FirstName;
            
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
            var eventParticipantId = _eventRepository.RegisterParticipantForEvent(participant.ParticipantId, eventId);
           
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

        public List<MyCampDTO> GetMyCampInfo(string token)
        {
            var apiToken = _apiUserRepository.GetToken();
            var campType = _configurationWrapper.GetConfigValue("CampEventTypeName");

            var dashboardData = new List<MyCampDTO>();

            var loggedInContact = _contactService.GetMyProfile(token);
            var family = _contactService.GetHouseholdFamilyMembers(loggedInContact.Household_ID);
            family.AddRange(_contactService.GetOtherHouseholdMembers(loggedInContact.Contact_ID));

            var camps = _eventRepository.GetEvents(campType, apiToken);
            foreach (var camp in camps.Where(c => c.EventEndDate >= DateTime.Today))
            {
                var campers = _eventRepository.EventParticipants(apiToken, camp.EventId).ToList();
                if (campers.Any())
                {
                    foreach (var member in family)
                    {
                        if (campers.Any(c => c.ContactId == member.ContactId))
                        {
                            dashboardData.Add(new MyCampDTO
                            {
                                CamperContactId = member.ContactId,
                                CamperNickName = member.Nickname ?? member.FirstName,
                                CamperLastName = member.LastName,
                                CampName = camp.EventTitle,
                                CampStartDate = camp.EventStartDate,
                                CampEndDate = camp.EventEndDate
                            });
                        }
                    }
                }
            }

            return dashboardData;
        }
    }
}
