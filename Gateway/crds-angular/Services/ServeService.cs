﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using crds_angular.Enum;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    public class ServeService : MinistryPlatformBaseService, IServeService
    {
        private readonly IContactService _contactService;
        private readonly IContactRelationshipService _contactRelationshipService;
        private readonly IEventService _eventService;
        private readonly IGroupParticipantService _groupParticipantService;
        private readonly IGroupService _groupService;
        private readonly IOpportunityService _opportunityService;
        private readonly IParticipantService _participantService;
        private readonly ICommunicationService _communicationService;

        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServeService(IContactService contactService, IContactRelationshipService contactRelationshipService,
            IOpportunityService opportunityService, IEventService eventService, IParticipantService participantService,
            IGroupParticipantService groupParticipantService, IGroupService groupService,
            ICommunicationService communicationService)
        {
            _contactService = contactService;
            _contactRelationshipService = contactRelationshipService;
            _opportunityService = opportunityService;
            _eventService = eventService;
            _participantService = participantService;
            _groupParticipantService = groupParticipantService;
            _groupService = groupService;
            _communicationService = communicationService;
        }

        public List<FamilyMember> GetImmediateFamilyParticipants(int contactId, string token)
        {
            var relationships = new List<FamilyMember>();
            // get for contact Id
            var me = _participantService.GetParticipant(contactId);
            var myParticipant = new FamilyMember
            {
                ContactId = contactId,
                Email = me.EmailAddress,
                LoggedInUser = true,
                ParticipantId = me.ParticipantId,
                PreferredName = me.PreferredName
            };
            relationships.Add(myParticipant);

            // get family for contact Id
            var contactRelationships =
                _contactRelationshipService.GetMyImmediatieFamilyRelationships(contactId, token).ToList();
            var family = contactRelationships.Select(contact => new FamilyMember
            {
                ContactId = contact.Contact_Id,
                Email = contact.Email_Address,
                LastName = contact.Last_Name,
                LoggedInUser = false,
                ParticipantId = contact.Participant_Id,
                PreferredName = contact.Preferred_Name,
                RelationshipId = contact.Relationship_Id,
                Age = contact.Age
            }).ToList();

            relationships.AddRange(family);

            return relationships;
        }

        public DateTime GetLastServingDate(int opportunityId, string token)
        {
            logger.Debug(string.Format("GetLastOpportunityDate({0}) ", opportunityId));
            return _opportunityService.GetLastOpportunityDate(opportunityId, token);
        }

        public List<QualifiedServerDto> GetQualifiedServers(int groupId, int contactId, string token)
        {
            var qualifiedServers = new List<QualifiedServerDto>();
            var immediateFamilyParticipants = GetImmediateFamilyParticipants(contactId, token);

            foreach (var participant in immediateFamilyParticipants)
            {
                var membership = _groupService.ParticipantGroupMember(groupId, participant.ParticipantId);
                var opportunityResponse = _opportunityService.GetMyOpportunityResponses(participant.ContactId, 115,
                    token);
                var qualifiedServer = new QualifiedServerDto();
                qualifiedServer.ContactId = participant.ContactId;
                qualifiedServer.Email = participant.Email;
                qualifiedServer.LastName = participant.LastName;
                qualifiedServer.LoggedInUser = participant.LoggedInUser;
                qualifiedServer.MemberOfGroup = membership;
                qualifiedServer.Pending = opportunityResponse != null;
                qualifiedServer.ParticipantId = participant.ParticipantId;
                qualifiedServer.PreferredName = participant.PreferredName;
                qualifiedServers.Add(qualifiedServer);
            }
            return qualifiedServers;
        }

        public List<ServingDay> GetServingDays(string token, int contactId, long from, long to)
        {
            var family = GetImmediateFamilyParticipants(contactId, token);
            var participants = family.OrderBy(f => f.ParticipantId).Select(f => f.ParticipantId).ToList();
            var servingParticipants = _groupParticipantService.GetServingParticipants(participants, from, to, contactId);
            var servingDays = new List<ServingDay>();
            var dayIndex = 0;

            foreach (var record in servingParticipants)
            {
                var eventDateOnly = record.EventStartDateTime.Date.ToString("d");
                var day = servingDays.SingleOrDefault(d => d.Day == eventDateOnly);
                if (day != null)
                {
                    //this day is already in list

                    var time =
                        day.ServeTimes.SingleOrDefault(t => t.Time == record.EventStartDateTime.TimeOfDay.ToString());
                    if (time != null)
                    {
                        //this time already in collection

                        var team = time.ServingTeams.SingleOrDefault(t => t.GroupId == record.GroupId);
                        if (team != null)
                        {
                            //team already in collection
                            var member = team.Members.SingleOrDefault(m => m.ContactId == record.ContactId);
                            if (member == null)
                            {
                                team.Members.Add(NewTeamMember(record));
                            }
                            else
                            {
                                member.Roles.Add(NewServingRole(record));
                                //if we have already found rsvp, skip
                                if (member.ServeRsvp == null)
                                {
                                    member.ServeRsvp = NewServeRsvp(record);
                                }
                            }
                        }
                        else
                        {
                            time.ServingTeams.Add(NewServingTeam(record));
                        }
                    }
                    else
                    {
                        day.ServeTimes.Add(NewServingTime(record));
                    }
                }
                else
                {
                    //new day for the list
                    var servingDay = new ServingDay();
                    dayIndex = dayIndex + 1;
                    servingDay.Index = dayIndex;
                    servingDay.Day = record.EventStartDateTime.Date.ToString("d");
                    servingDay.Date = record.EventStartDateTime;
                    servingDay.ServeTimes = new List<ServingTime> {NewServingTime(record)};

                    servingDays.Add(servingDay);
                }
            }

            return servingDays;
        }

        public Capacity OpportunityCapacity(int opportunityId, int eventId, int? minNeeded, int? maxNeeded, string token)
        {
            var opportunity = _opportunityService.GetOpportunityResponses(opportunityId, token);
            var min = minNeeded;
            var max = maxNeeded;
            var signedUp = opportunity.Count(r => r.Event_ID == eventId);

            var capacity = new Capacity {Display = true};

            if (max == null && min == null)
            {
                capacity.Display = false;
                return capacity;
            }

            int calc;
            if (max == null)
            {
                capacity.Minimum = min.GetValueOrDefault();

                //is this valid?? max is null so put min value in max?
                capacity.Maximum = capacity.Minimum;

                calc = capacity.Minimum - signedUp;
            }
            else if (min == null)
            {
                capacity.Maximum = max.GetValueOrDefault();
                //is this valid??
                capacity.Minimum = capacity.Maximum;
                calc = capacity.Maximum - signedUp;
            }
            else
            {
                capacity.Maximum = max.GetValueOrDefault();
                capacity.Minimum = min.GetValueOrDefault();
                calc = capacity.Minimum - signedUp;
            }

            if (signedUp < capacity.Maximum && signedUp < capacity.Minimum)
            {
                capacity.Message = string.Format("{0} Needed", calc);
                capacity.BadgeType = BadgeType.LabelInfo.ToString();
                capacity.Available = calc;
                capacity.Taken = signedUp;
            }
            else if (signedUp >= capacity.Maximum)
            {
                capacity.Message = "Full";
                capacity.BadgeType = BadgeType.LabelDefault.ToString();
                capacity.Available = calc;
                capacity.Taken = signedUp;
            }

            return capacity;
        }

        public bool SaveServeRsvp(string token,
            int contactId,
            int opportunityId,
            List<int> opportunityIds,
            int eventTypeId,
            DateTime startDate,
            DateTime endDate,
            bool signUp,
            bool alternateWeeks)
        {
            //get participant id for Contact
            var participant = _participantService.GetParticipant(contactId);

            //get events in range
            var events = GetEventsInRange(token, eventTypeId, startDate, endDate);
            var templateId = GetRsvpTemplate(signUp);
            var opportunity = GetOpportunity(token, opportunityId, opportunityIds);
            var groupContact = _contactService.GetContactById(opportunity.GroupContactId);

            var toContact = _contactService.GetContactById(contactId);
            Opportunity previousOpportunity = null;
            try
            {
                var increment = alternateWeeks ? 14 : 7;
                var sequenceDate = startDate;
                for (var i = 0; i < events.Count(); i++)
                {
                    var @event = events[i];
                    sequenceDate = IncrementSequenceDate(@event, sequenceDate, increment);
                    if (@event.EventStartDate.Date != sequenceDate.Date) continue;

                    var response = CreateRsvp(token, opportunityId, opportunityIds, signUp, participant, @event);
                    previousOpportunity = PreviousOpportunity(response, previousOpportunity);
                    templateId = GetTemplateId(templateId, response);
                    sequenceDate = sequenceDate.AddDays(increment);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            var mergeData = SetupMergeData(contactId, opportunityId, previousOpportunity, opportunity, startDate,
                endDate, groupContact);
            var communication = SetupCommunication(templateId, groupContact, toContact, mergeData);
            _communicationService.SendMessage(communication);
            return true;
        }

        private static DateTime IncrementSequenceDate(Event @event, DateTime sequenceDate, int increment)
        {
            if (@event.EventStartDate.Date > sequenceDate.Date)
            {
                sequenceDate = sequenceDate.AddDays(increment);
            }
            return sequenceDate;
        }

        private static int GetTemplateId(int templateId, Dictionary<string, object> response)
        {
            templateId = (templateId != AppSetting("RsvpChangeTemplate"))
                ? response.ToInt("templateId")
                : templateId;
            return templateId;
        }

        private static Opportunity PreviousOpportunity(Dictionary<string, object> response, Opportunity previousOpportunity)
        {
            if (response.ToNullableObject<Opportunity>("previousOpportunity") != null)
                previousOpportunity = response.ToNullableObject<Opportunity>("previousOpportunity");
            return previousOpportunity;
        }

        private Dictionary<string, object> CreateRsvp(string token, int opportunityId, List<int> opportunityIds, bool signUp, Participant participant,
            Event @event)
        {
            var response = signUp
                ? HandleYesRsvp(participant, @event, opportunityId, opportunityIds, token)
                : HandleNoRsvp(participant, @event, opportunityIds, token);
            return response;
        }

        private Opportunity GetOpportunity(string token, int opportunityId, List<int> opportunityIds)
        {
            var opportunity = (opportunityId > 0)
                ? _opportunityService.GetOpportunityById(opportunityId, token)
                : _opportunityService.GetOpportunityById(opportunityIds.FirstOrDefault(), token);
            return opportunity;
        }

        private static int GetRsvpTemplate(bool signUp)
        {
            var templateId = signUp ? AppSetting("RsvpYesTemplate") : AppSetting("RsvpNoTemplate");
            return templateId;
        }

        private List<Event> GetEventsInRange(string token, int eventTypeId, DateTime startDate, DateTime endDate)
        {
            var events =
                _eventService.GetEventsByTypeForRange(eventTypeId, startDate, endDate, token)
                    .OrderBy(o => o.EventStartDate)
                    .ToList();
            return events;
        }

        private Dictionary<string, object> HandleYesRsvp(Participant participant, Event e, int opportunityId,
            IReadOnlyCollection<int> opportunityIds, String token)
        {
            var templateId = AppSetting("RsvpYesTemplate");
            var deletedRSVPS = new List<int>();
            Opportunity previousOpportunity = null;

            //Try to register this user for the event
            _eventService.registerParticipantForEvent(participant.ParticipantId, e.EventId);

            // Make sure we are only rsvping for 1 opportunity by removing all existing responses
            deletedRSVPS.AddRange(from oid in opportunityIds
                let deletedResponse =
                    _opportunityService.DeleteResponseToOpportunities(participant.ParticipantId, oid, e.EventId)
                where deletedResponse != 0
                select oid);

            if (deletedRSVPS.Count > 0)
            {
                templateId = AppSetting("RsvpChangeTemplate");
                if (opportunityIds.Count != deletedRSVPS.Count)
                {
                    //Changed yes to yes
                    //prevOppId = deletedRSVPS.First();
                    previousOpportunity = _opportunityService.GetOpportunityById(deletedRSVPS.First(), token);
                }
            }
            var comments = string.Empty; //anything of value to put in comments?
            _opportunityService.RespondToOpportunity(participant.ParticipantId, opportunityId, comments,
                e.EventId, true);
            return new Dictionary<string, object>()
            {
                {"templateId", templateId},
                {"previousOpportunity", previousOpportunity},
                {"rsvp", true}
            };
        }

        private Dictionary<string, object> HandleNoRsvp(Participant participant, Event e, List<int> opportunityIds,
            string token)
        {
            var templateId = AppSetting("RsvpNoTemplate");
            Opportunity previousOpportunity = null;

            try
            {
                templateId = AppSetting("RsvpChangeTemplate");
                //opportunityId = opportunityIds.First();
                _eventService.unRegisterParticipantForEvent(participant.ParticipantId, e.EventId);
            }
            catch (ApplicationException ex)
            {
                logger.Debug(ex.Message + ": There is no need to remove the event participant because there is not one.");
                templateId = AppSetting("RsvpNoTemplate");
            }

            // Responding no means that we are saying no to all opportunities for this group for this event            
            foreach (var oid in opportunityIds)
            {
                var comments = string.Empty; //anything of value to put in comments?
                var updatedOpp = _opportunityService.RespondToOpportunity(participant.ParticipantId, oid, comments,
                    e.EventId, false);
                if (updatedOpp > 0)
                {
                    previousOpportunity = _opportunityService.GetOpportunityById(oid, token);
                }
            }
            return new Dictionary<string, object>()
            {
                {"templateId", templateId},
                {"previousOpportunity", previousOpportunity},
                {"rsvp", false}
            };
        }

        private Communication SetupCommunication(int templateId, MyContact groupContact, MyContact toContact, Dictionary<string, object> mergeData)
        {
            var template = _communicationService.GetTemplate(templateId);
            return new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContactId = groupContact.Contact_ID,
                FromEmailAddress = groupContact.Email_Address,
                ReplyContactId = groupContact.Contact_ID,
                ReplyToEmailAddress = groupContact.Email_Address,
                ToContactId = toContact.Contact_ID,
                ToEmailAddress = toContact.Email_Address,
                MergeData = mergeData
            };
        }

        private Dictionary<string, object> SetupMergeData(int contactId, int opportunityId,
            Opportunity previousOpportunity, Opportunity currentOpportunity, DateTime startDate, DateTime endDate,
            MyContact groupContact)
        {
            return new Dictionary<string, object>
            {
                {"Opportunity_Name", opportunityId == 0 ? "Not Available" : currentOpportunity.OpportunityName},
                {"Start_Date", startDate.ToShortDateString()},
                {"End_Date", endDate.ToShortDateString()},
                {"Shift_Start", currentOpportunity.ShiftStart.FormatAsString() ?? string.Empty},
                {"Shift_End", currentOpportunity.ShiftEnd.FormatAsString() ?? string.Empty},
                {"Room", currentOpportunity.Room ?? string.Empty},
                {"Group_Contact", groupContact.Nickname + " " + groupContact.Last_Name},
                {"Group_Name", currentOpportunity.GroupName},
                {
                    "Previous_Opportunity_Name",
                    previousOpportunity != null ? previousOpportunity.OpportunityName : @"Not Available"
                }
            };
        }

        private ServeRole NewServingRole(GroupServingParticipant record)
        {
            return new ServeRole
            {
                Name = record.OpportunityTitle + " " + record.OpportunityRoleTitle,
                RoleId = record.OpportunityId,
                Room = record.Room,
                Minimum = record.OpportunityMinimumNeeded,
                Maximum = record.OpportunityMaximumNeeded,
                ShiftEndTime = record.OpportunityShiftEnd.FormatAsString(),
                ShiftStartTime = record.OpportunityShiftStart.FormatAsString()
            };
        }

        private ServingTime NewServingTime(GroupServingParticipant record)
        {
            return new ServingTime
            {
                Index = record.RowNumber,
                ServingTeams = new List<ServingTeam> {NewServingTeam(record)},
                Time = record.EventStartDateTime.TimeOfDay.ToString()
            };
        }

        private ServingTeam NewServingTeam(GroupServingParticipant record)
        {
            return new ServingTeam
            {
                Index = record.RowNumber,
                EventId = record.EventId,
                EventType = record.EventType,
                EventTypeId = record.EventTypeId,
                GroupId = record.GroupId,
                Members = new List<TeamMember> {NewTeamMember(record)},
                Name = record.GroupName,
                PrimaryContact = record.GroupPrimaryContactEmail
            };
        }

        private TeamMember NewTeamMember(GroupServingParticipant record)
        {
            // new team member
            var member = new TeamMember
            {
                ContactId = record.ContactId,
                EmailAddress = record.ParticipantEmail,
                Index = record.RowNumber,
                LastName = record.ParticipantLastName,
                Name = record.ParticipantNickname,
                Participant = new Participant {ParticipantId = record.ParticipantId}
            };

            member.Roles.Add(NewServingRole(record));

            member.ServeRsvp = NewServeRsvp(record);
            return member;
        }

        private ServeRsvp NewServeRsvp(GroupServingParticipant record)
        {
            if (record.Rsvp != null && !((bool) record.Rsvp))
            {
                return new ServeRsvp {Attending = false, RoleId = 0};
            }
            else if (record.Rsvp != null && ((bool) record.Rsvp))
            {
                return new ServeRsvp {Attending = (bool) record.Rsvp, RoleId = record.OpportunityId};
            }
            return null;
        }

        //public for testing
        public ServeRsvp GetRsvp(int opportunityId, int eventId, TeamMember member)
        {
            if (member.Responses == null)
            {
                return null;
            }
            var r =
                member.Responses.Where(t => t.Opportunity_ID == opportunityId && t.Event_ID == eventId)
                    .Select(t => t.Response_Result_ID)
                    .ToList();
            return r.Count <= 0 ? null : new ServeRsvp {Attending = (r[0] == 1), RoleId = opportunityId};
        }
    }
}