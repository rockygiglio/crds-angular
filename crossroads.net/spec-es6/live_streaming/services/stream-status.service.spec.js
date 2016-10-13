import CONSTANTS from 'crds-constants';
import ReminderService from '../../../app/live_stream/services/reminder.service';
import Reminder from '../../../app/live_stream/models/reminder';
import StreamStatusService from '../../../app/live_stream/services/stream-status.service';
//import * as ProfileService from "../../../app/profile/services/profile.service.js";

describe('Stream Status Service', () => {
  let service,
      reminder,
      http,
      modal,
      httpBackend,
      expectedHrsToEvent,
      hrsToNextEvent,
      currentTime,
      person,
      streamStatusService,
      profileService;

  let baseTime = moment('2016-10-12');

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
    //profileService = typeof ProfileService;
  }));

  afterEach(() => {
    // httpBackend.verifyNoOutstandingExpectation();
    // httpBackend.verifyNoOutstandingRequest();
  });

  describe('Stream Status Service', () => {

    // it('get hours until next streaming event starts', () => {
    //   currentTime = baseTime;
    //   console.log('Current time created in test: ' + currentTime);
    //   //let typeOfTime = typeof currentTime;
    //   hrsToNextEvent = streamStatusService.getHoursToNextEvent(events, currentTime);
    //   expectedHrsToEvent = 1;
    //   expect(1).toEqual(expectedHrsToEvent);
    // });

  })

});