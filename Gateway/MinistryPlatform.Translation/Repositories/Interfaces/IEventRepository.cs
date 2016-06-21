﻿using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;
using Participant = MinistryPlatform.Translation.Models.People.Participant;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IEventRepository
    {
        int CreateEvent(MpEventReservationDto eventReservationReservation);
        int SafeRegisterParticipant(int participantId, int eventId, int groupId = 0, int groupParticipantId = 0);
        int RegisterParticipantForEvent(int participantId, int eventId, int groupId = 0, int groupParticipantId = 0);
        int UnregisterParticipantForEvent(int participantId, int eventId);
        List<MpEvent> GetEvents(string eventType, string token);
        List<MpEvent> GetEventsByTypeForRange(int eventTypeId, DateTime startDate, DateTime endDate, string token);
        List<MpGroup> GetGroupsForEvent(int eventId);
        bool EventHasParticipant(int eventId, int participantId);
        MpEvent GetEvent(int eventId);
        List<MpEvent> GetEventsByParentEventId(int parentEventId);
        IEnumerable<MpEvent> EventsByPageId(string token, int pageViewId);
        IEnumerable<MpEvent> EventsByPageViewId(string token, int pageViewId, string search);
        IEnumerable<Participant> EventParticipants(string token, int eventId);
        void SetReminderFlag(int eventId, string token);
        List<MpEventGroup> GetEventGroupsForEvent(int eventId, string token);
        //void CopyEventGroup(EventGroup eventGroup);
        void DeleteEventGroup(MpEventGroup eventGroup, string token);
        int CreateEventGroup(MpEventGroup eventGroup, string token);
        void UpdateEventGroup(MpEventGroup eventGroup, string token);

        List<MpEvent> GetEventsBySite(string site, string token, DateTime startDate, DateTime endDate);
        List<MpEvent> GetEventTemplatesBySite(string site, string token);
        void DeleteEventGroupsForEvent(int eventId, string token);
    }
}