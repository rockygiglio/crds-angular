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
        private readonly ICampRepository _campService;
        private readonly IFormSubmissionRepository _formSubmissionRepository;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IParticipantRepository _participantRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IGroupService _groupService;
        private readonly IContactRepository _contactRepository;
        private readonly ICongregationRepository _congregationRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IEventParticipantRepository _eventParticipantRepository;

        public CampService(
            ICampRepository campService,
            IFormSubmissionRepository formSubmissionRepository,
            IConfigurationWrapper configurationWrapper,
            IParticipantRepository partcipantRepository,
            IEventRepository eventRepository,
            IApiUserRepository apiUserRepository,
            IGroupService groupService,
            IContactRepository contactRepository,
            ICongregationRepository congregationRepository,
            IGroupRepository groupRepository,
            IEventParticipantRepository eventParticipantRepository)
        {
            _campService = campService;
            _formSubmissionRepository = formSubmissionRepository;
            _configurationWrapper = configurationWrapper;
            _participantRepository = partcipantRepository;
            _eventRepository = eventRepository;
            _apiUserRepository = apiUserRepository;
            _groupService = groupService;
            _contactRepository = contactRepository;
            _congregationRepository = congregationRepository;
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
            var myContact = _contactRepository.GetMyProfile(token);
            var family = _contactRepository.GetHouseholdFamilyMembers(myContact.Household_ID);
            var otherFamily = _contactRepository.GetOtherHouseholdMembers(myContact.Contact_ID);
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

        public void SaveCampReservation(CampReservationDTO campReservation, int eventId, string token)
        {
            var gender = campReservation.Gender == 1 ? "Male" : "Female ";            
            var nickName = campReservation.PreferredName ?? campReservation.FirstName;
            var displayName = campReservation.PreferredName ?? campReservation.LastName + ", " + campReservation.FirstName;
            MpParticipant participant;
            var contactId = Convert.ToInt32(campReservation.ContactId);

            var minorContact = new MpContact
            {
                FirstName = campReservation.FirstName,
                LastName = campReservation.LastName,
                MiddleName = campReservation.MiddleName,
                BirthDate = Convert.ToDateTime(campReservation.BirthDate),
                Gender = campReservation.Gender,
                PreferredName = displayName,
                Nickname = nickName,
                SchoolAttending = campReservation.SchoolAttending,
                HouseholdId = (_contactRepository.GetMyProfile(token)).Household_ID,
                HouseholdPositionId = 2
            };

            if (campReservation.ContactId == null || campReservation.ContactId == 0)
            {
                var newMinorContact = _contactRepository.CreateContact(minorContact);
                contactId = newMinorContact[0].RecordId;
                participant = _participantRepository.GetParticipant(contactId);
            }
            else
            {
                var updateToDictionary = new Dictionary<string, object>
                {
                    {"Contact_ID", Convert.ToInt32(campReservation.ContactId) },
                    { "First_Name", minorContact.FirstName },
                    {"Last_Name", minorContact.LastName },
                    {"Middle_Name", minorContact.MiddleName },
                    {"Display_Name", displayName },
                    {"Date_Of_Birth", minorContact.BirthDate },
                    {"Gender", gender},
                    {"Current_School", minorContact.SchoolAttending },
                    {"Congregation_Name", (_congregationRepository.GetCongregationById(campReservation.CrossroadsSite)).Name }
                };
                _contactRepository.UpdateContact(Convert.ToInt32(campReservation.ContactId), updateToDictionary);
                participant = _participantRepository.GetParticipant(Convert.ToInt32(campReservation.ContactId));
            }
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

            var loggedInContact = _contactRepository.GetMyProfile(token);
            var family = _contactRepository.GetHouseholdFamilyMembers(loggedInContact.Household_ID);
            family.AddRange(_contactRepository.GetOtherHouseholdMembers(loggedInContact.Contact_ID));

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

        public List<CampWaiverDTO> GetCampWaivers(int eventId)
        {
            var waivers = _eventRepository.GetWaivers(eventId);
            return waivers.Select(waiver => new CampWaiverDTO
            {
                WaiverId = waiver.WaiverId, WaiverName = waiver.WaiverName, WaiverText = waiver.WaiverText, Required = waiver.Required
            }).ToList();
        }

        public void SaveWaivers(string token, int eventParticipantId, List<CampWaiverResponseDTO> waivers)
        {
            var loggedInContact = _contactRepository.GetMyProfile(token);
            var waiverResponses = waivers.Select(waiver => new MpWaiverResponse()
            {
                EventParticipantId = eventParticipantId,
                WaiverId = waiver.WaiverId,
                Accepted = waiver.WaiverAccepted,
                SigneeContactId = loggedInContact.Contact_ID
            }).ToList();
            _eventRepository.SetWaivers(waiverResponses);
        }

        public CampReservationDTO GetCamperInfo(string token, int eventId, int contactId)
        {
            var loggedInContact = _contactRepository.GetMyProfile(token);
            var family = _contactRepository.GetHouseholdFamilyMembers(loggedInContact.Household_ID);
            family.AddRange(_contactRepository.GetOtherHouseholdMembers(loggedInContact.Contact_ID));
            CampReservationDTO camperInfo = null;
            if (family.Where(f => f.ContactId == contactId).ToList().Count > 0)
            {
                var camperContact = _contactRepository.GetContactById(contactId);
                var participant = _participantRepository.GetParticipant(contactId);
                var gradeGroupTypeId = _configurationWrapper.GetConfigIntValue("AgeorGradeGroupType");
                var gradeGroup = (_groupService.GetGroupsByTypeForParticipant(token, participant.ParticipantId, gradeGroupTypeId)).FirstOrDefault();
                var currentGrade = gradeGroup.GroupName;

                camperInfo = new CampReservationDTO
                {
                    ContactId = camperContact.Contact_ID,
                    FirstName = camperContact.First_Name,
                    LastName = camperContact.Last_Name,
                    MiddleName = camperContact.Middle_Name,
                    PreferredName = camperContact.Display_Name,
                    CrossroadsSite = Convert.ToInt32(camperContact.Congregation_ID),
                    BirthDate = Convert.ToString(camperContact.Date_Of_Birth),
                    SchoolAttending = camperContact.Current_School,
                    Gender = Convert.ToInt32(camperContact.Gender_ID),
                    CurrentGrade = currentGrade
                };
            }
            return camperInfo;
        }
    }
}
