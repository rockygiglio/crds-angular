using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EventParticipantService : BaseService, IEventParticipantService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public EventParticipantService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public bool AddDocumentsToTripParticipant(List<MpTripDocuments> documents, int eventParticipantId)
        {
            try
            {
                var token = ApiLogin();
                foreach (var d in documents)
                {
                    var values = new Dictionary<string, object>
                    {
                        {"Document_ID", d.DocumentId},
                        {"Received", false}
                    };
                    _ministryPlatformService.CreateSubRecord("EventParticipantDocuments", eventParticipantId, values, token, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("AddDocumentsToTripParticipant failed.  Event Participant: {0}", eventParticipantId),
                    ex);
            }
        }

        public List<MpTripParticipant> TripParticipants(string search)
        {
            try
            {
                var records =
                    WithApiLogin(
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("GoTripParticipants",
                                                                         apiToken,
                                                                         search)));
                return records.Select(viewRecord => new MpTripParticipant
                {
                    EventParticipantId = viewRecord.ToInt("Event_Participant_ID"),
                    EventId = viewRecord.ToInt("Event_ID"),
                    EventTitle = viewRecord.ToString("Event_Title"),
                    Nickname = viewRecord.ToString("Nickname"),
                    Lastname = viewRecord.ToString("Last_Name"),
                    EmailAddress = viewRecord.ToString("Email_Address"),
                    EventStartDate = viewRecord.ToDate("Event_Start_Date"),
                    EventEndDate = viewRecord.ToDate("Event_End_Date"),
                    EventType = viewRecord.ToString("Event_Type"),
                    ParticipantId = viewRecord.ToInt("Participant_ID"),
                    ProgramId = viewRecord.ToInt("Program_ID"),
                    ProgramName = viewRecord.ToString("Program_Name"),
                    CampaignId = viewRecord.ToInt("Campaign_ID"),
                    CampaignName = viewRecord.ToString("Campaign_Name"),
                    DonorId = viewRecord.ToInt("Donor_ID"),
                    ContactId = viewRecord.ToInt("Contact_ID"),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("TripParticipants failed.  search: {0}", search),
                    ex);
            }
        }

        public List<MpEventParticipant> GetChildCareParticipants(int daysBeforeEvent)
        {
            try
            {
                var search = string.Format("\"{0}\",", daysBeforeEvent);

                var records =
                    WithApiLogin(
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("EventParticipantsChildCarePageView",
                                                                         apiToken,
                                                                         search)));

                return records.Select(viewRecord => new MpEventParticipant
                {
                    ChildcareRequired = viewRecord.ToBool("Child_Care_Requested"),
                    ContactId = viewRecord.ToInt("Contact_ID"),
                    EventId = viewRecord.ToInt("Event_ID"),
                    EventParticipantId = viewRecord.ToInt("Event_Participant_ID"),
                    EventStartDateTime = viewRecord.ToDate("Event_Start_Date"),
                    EventTitle = viewRecord.ToString("Event_Title"),
                    GroupId = viewRecord.ToInt("Group_ID"),
                    GroupName = viewRecord.ToString("Group_Name"),
                    GroupParticipantId = viewRecord.ToInt("Group_Participant_ID"),
                    ParticipantEmail = viewRecord.ToString("Email_Address"),
                    ParticipantId = viewRecord.ToInt("Participant_ID")
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("GetChildCareParticipants failed.", ex);
            }
        }

        public List<MpEventParticipant> GetEventParticipants(int eventId, int? roomId = null)
        {
            var searchString = roomId == null ? null : string.Format(",,,\"{0}\"", roomId);

            try
            {
                var records = 
                    WithApiLogin(
                        apiToken => _ministryPlatformService.GetSubpageViewRecords("EventParticipantAssignedToRoomApiSubPageView", eventId, apiToken, searchString));
                return records.Select(viewRecord => new MpEventParticipant
                {
                    EventParticipantId = viewRecord.ToInt("Event_Participant_ID"),
                    ParticipantId = viewRecord.ToInt("Participant_ID"),
                    ParticipantStatus = viewRecord.ToInt("Participation_Status_ID"),
                    RoomId = viewRecord.ToInt("Room_ID")
                }).ToList();
            }
            catch (Exception e)
            {
                throw new ApplicationException("GetEventParticipants failed", e);
            }
        }
    }
}