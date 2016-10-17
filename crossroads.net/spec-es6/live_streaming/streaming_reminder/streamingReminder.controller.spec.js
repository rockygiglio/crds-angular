import constants from 'crds-constants';
import StreamingReminderController from '../../../app/live_stream/streaming_reminder/streamingReminder.controller';
import Reminder from '../../../app/live_stream/models/reminder';
import Event from '../../../app/live_stream/models/event';
import ReminderService from '../../../app/live_stream/services/reminder.service';
import StreamspotService from '../../../app/live_stream/services/streamspot.service';
import Session from '../../../core/services/session_service';

describe('Streaming Reminder Controller', () => {
  let fixture,
      modalInstance,
      current,
      future,
      futureDuplicate,
      currentEvent,
      futureEvent,
      futureDuplicateEvent,
      httpBackend,
      Session,
      Profile,
      RootScope,
      Scope;

  const reminderEndpoint = `${__API_ENDPOINT__}`;
  
  let baseTime = new Date(2016, 9, 1); // set to 10/1/2016 - month appears to be 0 based index however

  modalInstance = {
    close: jasmine.createSpy('modalInstance.close'),
    dismiss: jasmine.createSpy('modalInstance.dismiss'),
    result: {
      then: jasmine.createSpy('modalInstance.result.then')
    }
  };
    
  beforeEach(angular.mock.module(constants.MODULES.LIVE_STREAM));

  beforeEach(inject(function ($injector) {
    StreamspotService = $injector.get('StreamspotService');
    ReminderService = $injector.get('ReminderService');
    httpBackend = $injector.get('$httpBackend');
    Session = $injector.get('Session');
    Profile = $injector.get('Profile');
    RootScope = $injector.get('$rootScope');
    Scope = RootScope.$new();
    fixture = new StreamingReminderController(modalInstance, StreamspotService, ReminderService, Session, Profile, Scope);

    jasmine.clock().mockDate(baseTime);

    current = {
      "start": moment(baseTime).tz(moment.tz.guess()).add(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).tz(moment.tz.guess()).add(2, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Current Month"
    },
    future = {
      "start": moment(baseTime).tz(moment.tz.guess()).add(1, 'month').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).tz(moment.tz.guess()).add(1, 'month').add(1, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Future Month"
    };
    futureDuplicate = {
      "start": moment(baseTime).tz(moment.tz.guess()).add(1, 'month').add(2, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "end": moment(baseTime).tz(moment.tz.guess()).add(1, 'month').add(3, 'hour').format('YYYY-MM-DD H:mm:ss'),
      "title": "Future Duplicate Month"
    };

    currentEvent         = Event.build(current);
    futureEvent          = Event.build(future);
    futureDuplicateEvent = Event.build(futureDuplicate);

    fixture.upcoming.push(currentEvent);
    fixture.upcoming.push(futureEvent);
    fixture.upcoming.push(futureDuplicateEvent);
  }));

  afterEach(() => {
    jasmine.clock().mockDate();
  });

  it('should return next date', () => {
    expect(fixture.nextDate()).toBe(currentEvent.start.format('MM/DD/YYYY'));
  });
  it('should return unique dates', () => {
    let uniqueDates = fixture.uniqueDates();
    expect(uniqueDates instanceof Array).toBe(true);
    expect(uniqueDates.length).toBe(2);
    expect(uniqueDates[0].title).toBe('Current Month');
    expect(uniqueDates[1].title).toBe('Future Month');
  });
  it('should group dates', () => {
    let grouped = fixture.groupedDates(),
        keys    = Object.keys(grouped);

    expect(keys.length).toBe(2);
    expect(keys).toContain(currentEvent.start.format('MM/DD/YYYY'));
    expect(keys).toContain(futureEvent.start.format('MM/DD/YYYY'));
    expect(keys).toContain(futureDuplicateEvent.start.format('MM/DD/YYYY'));
  });
  it('should set day', () => {
    fixture.setDay(currentEvent);
    expect(fixture.model.day).toBe(currentEvent.start.format('MM/DD/YYYY'));
  });
  it('should reset day', () => {
    fixture.setDay(false);
    expect(fixture.model.day).toBe(false);
    expect(fixture.model.time).toBe('');
  })
  it('should set time', () => {
    fixture.setTime(currentEvent);
    expect(fixture.model.time).toBe(currentEvent.start.format('h:mma z'));
  });
  it('should reset form', () => {
    fixture.setDay(futureEvent);
    fixture.setTime(futureEvent);
    expect(fixture.model.day).toBe(futureEvent.start.format('MM/DD/YYYY'));
    expect(fixture.model.time).toBe(futureEvent.start.format('h:mma z'));

    fixture.resetForm();

    expect(fixture.model.day).toBe(currentEvent.start.format('MM/DD/YYYY'));
    expect(fixture.model.time).toBe('')

  });
  it('should check if loading', () => {
    expect(fixture.isLoading()).toBe(false);

    fixture.loading = true;
    
    expect(fixture.isLoading()).toBe(true);
  });
  it('should set DateTime error on invalid date/time', () => {
    fixture.model.isDayValid = false;
    fixture.model.isTimeValid = false;

    fixture.submit(false);
    expect(fixture.dateTimeError).toBe(true);

    fixture.model.isDayValid = true;
    fixture.submit(false);
    expect(fixture.dateTimeError).toBe(true);

    fixture.model.isTimeValid = true;
    fixture.submit(false);
    expect(fixture.dateTimeError).toBe(false);
  })
  it('should submit the form', () => {
    let url = `${reminderEndpoint}api/sendEmailReminder`;
    let result = {}

    fixture.setDay(futureEvent);
    fixture.setTime(futureEvent);

    fixture.model.email = 'test@test.com';
    fixture.model.type  = 'email';

    fixture.model.isDayValid = true;
    fixture.model.isTimeValid = true;
    fixture.model.isEmailValid = true;


    expect(fixture.model.isValid()).toBe(true);

    fixture.submit(false);

    httpBackend.expectPOST(url).respond(200, result);
    httpBackend.flush();

  });
  it('should set user defaults', () => {
    fixture.setUserDefaults(1234);
    let expectedEmail = "user@test.com";
    let expectedPhone = "123-456-7890";
    let result = {  
      "emailAddress": expectedEmail,
      "mobilePhone": expectedPhone
    };

    let url = `${reminderEndpoint}api/profile`;
    httpBackend.expectGET(url).respond(200, result);
    httpBackend.flush();

    expect(fixture.model.email).toBe(expectedEmail);
    expect(fixture.model.phone).toBe(expectedPhone);
  })
})
