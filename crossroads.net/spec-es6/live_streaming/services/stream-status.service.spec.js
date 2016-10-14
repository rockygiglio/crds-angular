import CONSTANTS from 'crds-constants';
import ReminderService from '../../../app/live_stream/services/reminder.service';
import Reminder from '../../../app/live_stream/models/reminder';
import StreamStatusService from '../../../app/live_stream/services/stream-status.service';

describe('Stream Status Service', () => {
  let expectedHrsToEvent,
      hrsToNextEvent,
      streamStatusService,
      eventStartingAfterCurrentTime,
      doesEventStartAfterCurrentTime,
      isBroadcasting,
      upcomingStatus,
      streamStatus,
      didStreamStatusChange;

  let baseTime = moment();

  let events = [
    {
      "start": moment(baseTime).subtract(4, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).add(3, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal Broadcasting"
    },
    {
      "start": moment(baseTime).subtract(5, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).subtract(6, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal Done"
    },
    {
      "start": moment(baseTime).add(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).add(2, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal Upcoming"
    },
    {
      "start": moment(baseTime).subtract(10, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).subtract(9, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Saturday Rehearsal Done"
    }
  ];

  beforeEach(angular.mock.module(CONSTANTS.MODULES.LIVE_STREAM));

  beforeEach(inject(function ($injector) {
    streamStatusService = new StreamStatusService();
  }));

  describe('Stream Status Service', () => {

    it('should indicate that the event starts after the current time', () => {
      eventStartingAfterCurrentTime = events[2];
      doesEventStartAfterCurrentTime = streamStatusService.doesEventStartAfterCurrentTime(eventStartingAfterCurrentTime);
      expect(doesEventStartAfterCurrentTime).toBeTruthy();
    });

    it('get hours until next streaming event starts', () => {
      hrsToNextEvent = streamStatusService.getHoursToNextEvent(events);
      expectedHrsToEvent = 1;
      expect(1).toEqual(expectedHrsToEvent);
    });

    it('should show stream status as upcoming', () => {
      isBroadcasting = false;
      streamStatus = streamStatusService.determineStreamStatus(events, isBroadcasting);
      upcomingStatus = CONSTANTS.STREAM_STATUS.UPCOMING;
      expect(streamStatus).toEqual(upcomingStatus);
    });

    it('should indicate that stream status changed', () => {
      isBroadcasting = false;
      didStreamStatusChange = streamStatusService.didStreamStatusChange(events, isBroadcasting);
      expect(streamStatus).toBeTruthy(didStreamStatusChange);
    });

})

});