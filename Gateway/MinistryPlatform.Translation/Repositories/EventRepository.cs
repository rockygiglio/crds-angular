using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        private readonly log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _eventParticipantSubPageId = Convert.ToInt32(AppSettings("EventsParticipants"));
        private readonly int _eventParticipantPageId = Convert.ToInt32(AppSettings("EventParticipant"));
        private readonly int _eventGroupsPageViewId = Convert.ToInt32(AppSettings("GroupsByEventId"));
        private readonly int _eventGroupsPageId = Convert.ToInt32(AppSettings("EventsGroups"));

        private readonly int _eventParticipantStatusDefaultId =
            Convert.ToInt32(AppSettings("Event_Participant_Status_Default_ID"));

        private readonly int _eventPageNeedReminders =
            Convert.ToInt32(AppSettings("EventsReadyForReminder"));

        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IGroupRepository _groupService;

        public EventRepository(IMinistryPlatformService ministryPlatformService,
                            IAuthenticationRepository authenticationService,
                            IConfigurationWrapper configurationWrapper,
                            IGroupRepository groupService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _groupService = groupService;
        }

        public int CreateEvent(MpEventReservationDto eventReservationReservation)
        {
            var token = ApiLogin();
            var eventPageId = _configurationWrapper.GetConfigIntValue("Events");

            var eventDictionary = new Dictionary<string, object>
            {
                {"Congregation_ID", eventReservationReservation.CongregationId},
                {"Primary_Contact", eventReservationReservation.ContactId},
                {"Description", eventReservationReservation.Description},
                {"On_Donation_Batch_Tool", eventReservationReservation.DonationBatchTool},
                {"Event_End_Date", eventReservationReservation.EndDateTime},
                {"Event_Type_ID", eventReservationReservation.EventTypeId},
                {"Meeting_Instructions", eventReservationReservation.MeetingInstructions},
                {"Minutes_for_Setup", eventReservationReservation.MinutesSetup},
                {"Minutes_for_Cleanup", eventReservationReservation.MinutesTeardown},
                {"Program_ID", eventReservationReservation.ProgramId},
                {"Reminder_Days_Prior_ID", eventReservationReservation.ReminderDaysId},
                {"Send_Reminder", eventReservationReservation.SendReminder},
                {"Event_Start_Date", eventReservationReservation.StartDateTime},
                {"Event_Title", eventReservationReservation.Title},
                {"Visibility_Level_ID", _configurationWrapper.GetConfigIntValue("EventVisibilityLevel")}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(eventPageId, eventDictionary, token, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Event Reservation, eventReservationReservation: {0}", eventReservationReservation);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public int SafeRegisterParticipant(int eventId, int participantId, int groupId = 0, int groupParticipantId = 0)
        {
            var eventParticipantId = GetEventParticipantRecordId(eventId, participantId);
            if (eventParticipantId != 0)
            {
                return eventParticipantId;
            }
            eventParticipantId = RegisterParticipantForEvent(participantId, eventId, groupId, groupParticipantId);
            return eventParticipantId;
        }

        public int RegisterParticipantForEvent(int participantId, int eventId, int groupId = 0, int groupParticipantId = 0)
        {
            _logger.Debug("Adding participant " + participantId + " to event " + eventId);
            var values = new Dictionary<string, object>
            {
                {"Participant_ID", participantId},
                {"Event_ID", eventId},
                {"Participation_Status_ID", _eventParticipantStatusDefaultId},
            };

            if (groupId != 0)
            {
                values.Add("Group_ID", groupId);
            }
            if (groupParticipantId != 0)
            {
                values.Add("Group_Participant_ID", groupParticipantId);
            }

            int eventParticipantId;
            try
            {
                eventParticipantId =
                    WithApiLogin<int>(
                        apiToken =>
                        {
                            return
                                (_ministryPlatformService.CreateSubRecord(_eventParticipantSubPageId,
                                                                          eventId,
                                                                          values,
                                                                          apiToken,
                                                                          true));
                        });
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("registerParticipantForEvent failed.  Participant Id: {0}, Event Id: {1}",
                                  participantId,
                                  eventId),
                    ex.InnerException);
            }

            _logger.Debug(string.Format("Added participant {0} to event {1}; record id: {2}",
                                        participantId,
                                        eventId,
                                        eventParticipantId));
            return (eventParticipantId);
        }

        public int UnregisterParticipantForEvent(int participantId, int eventId)
        {
            _logger.Debug("Removing participant " + participantId + " from event " + eventId);

            int eventParticipantId;
            try
            {
                // go get record id to delete
                var recordId = GetEventParticipantRecordId(eventId, participantId);
                eventParticipantId = _ministryPlatformService.DeleteRecord(_eventParticipantPageId, recordId, null, ApiLogin());
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("unRegisterParticipantForEvent failed.  Participant Id: {0}, Event Id: {1}",
                                  participantId,
                                  eventId),
                    ex.InnerException);
            }

            _logger.Debug(string.Format("Removed participant {0} from event {1}; record id: {2}",
                                        participantId,
                                        eventId,
                                        eventParticipantId));
            return (eventParticipantId);
        }

        public MpEvent GetEvent(int eventId)
        {
            var token = ApiLogin();
            var searchString = string.Format("{0},", eventId);
            var r = _ministryPlatformService.GetPageViewRecords("EventsWithDetail", token, searchString);
            if (r.Count == 1)
            {
                var record = r[0];
                var e = new MpEvent
                {
                    CongregationId = record.ToInt("Congregation_ID"),
                    EventEndDate = record.ToDate("Event_End_Date"),
                    EventId = record.ToInt("Event_ID"),
                    EventStartDate = record.ToDate("Event_Start_Date"),
                    EventTitle = record.ToString("Event_Title"),
                    ParentEventId = record.ToNullableInt("Parent_Event_ID"),
                    PrimaryContact = new MpContact
                    {
                        ContactId = record.ToInt("Contact_ID"),
                        EmailAddress = record.ToString("Email_Address")
                    },
                    ReminderDaysPriorId = record.ToInt("Reminder_Days_Prior_ID"),
                    Cancelled = record.ToBool("Cancelled")
                };


                return e;
            }
            if (r.Count == 0)
            {
                return null;
            }
            throw new ApplicationException(string.Format("Duplicate Event ID detected: {0}", eventId));
        }

        public int GetEventParticipantRecordId(int eventId, int participantId)
        {
            var search = "," + eventId + "," + participantId;
            var participant = _ministryPlatformService.GetPageViewRecords("EventParticipantByEventIdAndParticipantId", ApiLogin(), search).FirstOrDefault();
            return participant == null ? 0 : participant.ToInt("Event_Participant_ID");
        }

        public bool EventHasParticipant(int eventId, int participantId)
        {
            var searchString = "," + eventId + "," + participantId;
            var records = _ministryPlatformService.GetPageViewRecords("EventParticipantByEventIdAndParticipantId", ApiLogin(), searchString);
            return records.Count != 0;
        }

        public List<MpEvent> GetEvents(string eventType, string token)
        {
            //this is using the basic Events page, any concern there?
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Events"]);
            var search = ",," + eventType;
            var records = _ministryPlatformService.GetRecordsDict(pageId, token, search);

            return records.Select(record => new MpEvent
            {
                EventTitle = (string) record["Event_Title"],
                EventType = (string) record["Event_Type"],
                EventStartDate = (DateTime) record["Event_Start_Date"],
                EventEndDate = (DateTime) record["Event_End_Date"],
                EventId = (int) record["dp_RecordID"]
            }).ToList();
        }

        public List<MpEvent> GetEventsByTypeForRange(int eventTypeId, DateTime startDate, DateTime endDate, string token)
        {
            const string viewKey = "EventsWithEventTypeId";
            var search = ",," + eventTypeId;
            var eventRecords = _ministryPlatformService.GetPageViewRecords(viewKey, token, search);

            var events = eventRecords.Select(record => new MpEvent
            {
                EventTitle = record.ToString("Event Title"),
                EventType = record.ToString("Event Type"),
                EventStartDate = record.ToDate("Event Start Date", true),
                EventEndDate = record.ToDate("Event End Date", true),
                EventId = record.ToInt("dp_RecordID"),
                CongregationId = record.ToInt("Congregation_ID")
            }).ToList();

            //now we have a list, filter by date range.
            var filteredEvents =
                events.Where(e => e.EventStartDate.Date >= startDate.Date && e.EventStartDate.Date <= endDate.Date)
                    .ToList();
            return filteredEvents;
        }

        public List<MpEvent> GetEventsByParentEventId(int parentEventId)
        {
            var token = ApiLogin();
            var searchStr = string.Format(",,,{0}", parentEventId);
            var records = _ministryPlatformService.GetPageViewRecords("EventsByParentEventID", token, searchStr);

            var events = records.Select(record => new MpEvent
            {
                EventTitle = record.ToString("Event_Title"),
                EventType = record.ToString("Event_Type"),
                EventStartDate = record.ToDate("Event_Start_Date", true),
                EventEndDate = record.ToDate("Event_End_Date", true),
                EventId = record.ToInt("Event_ID"),
                PrimaryContact = new MpContact
                {
                    ContactId = record.ToInt("Contact_ID"),
                    EmailAddress = record.ToString("Email_Address")
                }
            }).ToList();

            return events;
        }

        public IEnumerable<MpEvent> EventsByPageId(string token, int pageViewId)
        {
            return _ministryPlatformService.GetRecordsDict(pageViewId, token).Select(record => new MpEvent()
            {
                EventId = (int) record["Event_ID"],
                EventTitle = (string) record["Event_Title"],
                EventStartDate = (DateTime) record["Event_Start_Date"],
                EventEndDate = (DateTime) record["Event_End_Date"],
                EventType = record.ToString("Event_Type"),
                PrimaryContact = new MpContact()
                {
                    ContactId = record.ToInt("Primary_Contact_ID"),
                    EmailAddress = record.ToString("Primary_Contact_Email_Address")
                }
            }).ToList();
        }

        public IEnumerable<MpEvent> EventsByPageViewId(string token, int pageViewId, string searchString)
        {
            return _ministryPlatformService.GetPageViewRecords(pageViewId, token, searchString).Select(record => new MpEvent()
            {
                EventId = (int) record["Event_ID"],
                EventTitle = (string) record["Event_Title"],
                EventStartDate = (DateTime) record["Event_Start_Date"],
                EventEndDate = (DateTime) record["Event_End_Date"],
                EventType = record.ToString("Event_Type"),
                PrimaryContact = new MpContact()
                {
                    ContactId = record.ToInt("Primary_Contact_ID"),
                    EmailAddress = record.ToString("Primary_Contact_Email_Address")
                }
            }).ToList();
        }

        public IEnumerable<MinistryPlatform.Translation.Models.People.Participant> EventParticipants(string token, int eventId)
        {
            return
                _ministryPlatformService.GetSubpageViewRecords("EventParticipantSubpageRegisteredView", eventId, token)
                    .Select(person => new MinistryPlatform.Translation.Models.People.Participant()
                    {
                        ParticipantId = person.ToInt("Participant_ID"),
                        ContactId = person.ToInt("Contact_ID"),
                        EmailAddress = person.ToString("Email_Address"),
                        DisplayName = person.ToString("Display_Name"),
                        Nickname = person.ToString("Nickname"),
                        GroupName = person.ToString("Group_Name")
                    });
        }

        public void SetReminderFlag(int eventId, string token)
        {
            var dict = new Dictionary<string, object>
            {
                {"Event_ID", eventId},
                {"Reminder_Sent", 1}
            };
            _ministryPlatformService.UpdateRecord(_eventPageNeedReminders, dict, token);
        }

        public List<MpGroup> GetGroupsForEvent(int eventId)
        {
            return _groupService.GetGroupsForEvent(eventId);
        }

        public List<MpEventGroup> GetEventGroupsForEvent(int eventId, string token)
        {
            var searchString =  string.Format("\"{0}\",", eventId);
            var records = _ministryPlatformService.GetPageViewRecords(_eventGroupsPageViewId, token, searchString);

            if (records == null)
            {
                return null;
            }

            return records.Select(record => new MpEventGroup
            {
                EventGroupId = record.ToInt("Event_Group_ID"),
                EventId = record.ToInt("Event_ID"),
                GroupId = record.ToInt("Group_ID"),
                RoomId = record.ToNullableInt("Room_ID"),
                Closed = record.ToBool("Closed"),
                EventRoomId = record.ToNullableInt("Event_Room_ID"),
                GroupTypeId = record.ToInt("Group_Type_ID")
            }).ToList();
        }

        public List<MpEventGroup> GetEventGroupsForGroup(int groupId, string token)
        {
            var searchString = string.Format(",,,\"{0}\"", groupId);
            var records = _ministryPlatformService.GetPageViewRecords(_eventGroupsPageViewId, token, searchString);

            if (records != null)
            {
                return null;
            }
            return records.Select(record => new MpEventGroup
            {
                EventGroupId = record.ToInt("Event_Group_ID"),
                EventId = record.ToInt("Event_ID"),
                GroupId = record.ToInt("Group_ID"),
                RoomId = record.ToNullableInt("Room_ID"),
                Closed = record.ToBool("Closed"),
                EventRoomId = record.ToNullableInt("Event_Room_ID")
            }).ToList();
        }

        public void DeleteEventGroup(MpEventGroup eventGroup, string token)
        {
            _ministryPlatformService.DeleteRecord(_eventGroupsPageId, eventGroup.EventGroupId, null, token);
        }

        public void DeleteEventGroupsForEvent(int eventId, string token)
        {
            // get event group ids
            var discardedEventGroupIds = GetEventGroupsForEvent(eventId, token).Select(r => r.EventGroupId).ToArray();

            // MP will throw an error if there are no elements to delete, so we need to exit the function before then
            if (discardedEventGroupIds.Length == 0)
            {
                return;
            }

            // create selection for event groups
            SelectionDescription eventGroupSelDesc = new SelectionDescription();
            eventGroupSelDesc.DisplayName = "DiscardedEventGroups " + DateTime.Now;
            eventGroupSelDesc.Kind = SelectionKind.Normal;
            eventGroupSelDesc.PageId = _eventGroupsPageId;
            var eventGroupSelId = _ministryPlatformService.CreateSelection(eventGroupSelDesc, token);

            // add events to selection
            _ministryPlatformService.AddToSelection(eventGroupSelId, discardedEventGroupIds, token);

            // delete the selection records
            _ministryPlatformService.DeleteSelectionRecords(eventGroupSelId, token);

            // delete the selection
            _ministryPlatformService.DeleteSelection(eventGroupSelId, token);
        }

        public List<MpEvent> GetEventsBySite(string site, string token, DateTime startDate, DateTime endDate)
        {
            StringBuilder dateSearchString = new StringBuilder();

            var dayOffset = endDate - startDate;

            // if the days at the same, this is done to create the basic search string
            if (dayOffset.Days == 0)
            {
                dateSearchString.Append(startDate.ToShortDateString());
            }

            for (int day = 0; day < dayOffset.Days; day++)
            {
                DateTime newDate = startDate.Date.AddDays(day);

                if (day != (dayOffset.Days - 1))
                {
                    dateSearchString.Append(newDate.ToShortDateString() + " OR ");
                }
                else if (day == (dayOffset.Days - 1))
                {
                    dateSearchString.Append(newDate.ToShortDateString());
                }
            }

            var searchString = string.Format(",,\"{0}\",,False,{1},{1}", site, dateSearchString);

            return GetEventsData(token, searchString);
        }

        public List<MpEvent> GetEventTemplatesBySite(string site, string token)
        {
            var searchString = string.Format(",,\"{0}\",,True,", site);

            return GetEventsData(token, searchString);
        }

        public int CreateEventGroup(MpEventGroup eventGroup, string token)
        {
            var groupDictionary = new Dictionary<string, object>
            {
                {"Event_ID", eventGroup.EventId},
                {"Group_ID", eventGroup.GroupId},
                {"Room_ID", eventGroup.RoomId},
                {"Domain_ID", 1},
                {"Closed", eventGroup.Closed},
                {"Event_Room_ID", eventGroup.EventRoomId}
            };

            try
            {
                return (_ministryPlatformService.CreateRecord(_eventGroupsPageId, groupDictionary, token, true));
            }
            catch (Exception e)
            {
                var msg = string.Format("Error creating Event Group, eventGroup: {0}", eventGroup);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        public void UpdateEventGroup(MpEventGroup eventGroup, string token)
        {
            var groupDictionary = new Dictionary<string, object>
            {
                {"Event_ID", eventGroup.EventId},
                {"Group_ID", eventGroup.GroupId},
                {"Room_ID", eventGroup.RoomId},
                {"Domain_ID", eventGroup.DomainId},
                {"Closed", eventGroup.Closed},
                {"Event_Room_ID", eventGroup.EventRoomId}
            };

            try
            {
                _ministryPlatformService.UpdateRecord(_eventGroupsPageViewId, groupDictionary, token);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error updating Event Group, eventGroup: {0}", eventGroup);
                _logger.Error(msg, e);
                throw (new ApplicationException(msg, e));
            }
        }

        private List<MpEvent> GetEventsData(string token, string searchString)
        {
            var pageViewId = _configurationWrapper.GetConfigIntValue("EventsBySite");
            var records = _ministryPlatformService.GetPageViewRecords(pageViewId, token, searchString);

            if (records == null || records.Count == 0)
            {
                return null;
            }

            return records.Select(record => new MpEvent
            {
                // this isn't a complete list of all event fields - we may need more for user info purposes
                EventId = record.ToInt("Event_ID"),
                Congregation = record.ToString("Congregation_Name"),
                EventTitle = record.ToString("Event_Title"),
            }).ToList();
        } 
    }
}